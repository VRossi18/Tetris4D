using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    float prevTime;
    float fallTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        ButtonInputs.instance.SetActiveBlock(gameObject, this);
        fallTime = GameManager.instance.ReadFallSpeed();
        if (!CheckValidMove())
        {
            GameManager.instance.SetIsGameOver();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Diferent way to make a coroutine
        if (Time.time - prevTime > fallTime)
        {
            //Moving down the piece
            transform.position += Vector3.down;
            if (!CheckValidMove())
            {
                transform.position += Vector3.up;
                //If player makes a line
                Playfield.instance.DeleteLayer();
                enabled = false;
                if (!GameManager.instance.ReadIsGameOver())
                {
                    Playfield.instance.SpawnNewBlock();
                }
            }
            else
            {
                //Update grid
                Playfield.instance.UpdateGrid(this);
            }

            prevTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetInput(Vector3.left);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetInput(Vector3.right);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            SetRotationInput(new Vector3(90, 0, 0));
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            SetRotationInput(new Vector3(-90, 0, 0));
        }
    }

    bool CheckValidMove()
    {
        foreach (Transform child in transform)
        {
            Vector3 pos = Playfield.instance.Round(child.position);
            if (!Playfield.instance.CheckInsideGrid(pos))
            {
                return false;
            }
        }

        foreach (Transform child in transform)
        {
            Vector3 pos = Playfield.instance.Round(child.position);
            Transform t = Playfield.instance.GetTransformOnGridPos(pos);
            if (t != null && t.parent != transform)
            {
                return false;
            }
        }

        return true;
    }

    public void SetInput(Vector3 direction)
    {
        transform.position += direction;
        if (!CheckValidMove())
        {
            transform.position -= direction;
        }
        else
        {
            Playfield.instance.UpdateGrid(this);
        }
    }

    public void SetRotationInput(Vector3 rotation)
    {
        transform.Rotate(rotation, Space.World);
        if (!CheckValidMove())
        {
            transform.Rotate(-rotation, Space.World);
        }
        else
        {
            Playfield.instance.UpdateGrid(this);
        }
    }

    public void SetSpeed()
    {
        fallTime = 0.1f;
    }
}
