using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInputs : MonoBehaviour
{
    public static ButtonInputs instance;

    public GameObject[] rotateCanvases;
    public GameObject moveCanvas;

    GameObject activeBlock;
    Block activeTetris;

    bool moveIsOn = true;
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetInputs();
    }

    void Update()
    {
        RepositionToActiveBlock();
        if (Input.GetKeyDown(KeyCode.J))
        {
            SwitchInputs();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            SetHighSpeed();
        }
    }

    public void SetActiveBlock(GameObject block, Block tBlock)
    {
        activeBlock = block;
        activeTetris = tBlock;
    }

    void RepositionToActiveBlock()
    {
        if (activeBlock != null)
        {
            transform.position = activeBlock.transform.position;
        }
    }

    public void MoveBlock(string direction)
    {
        if (activeBlock != null)
        {
            if (direction == "left")
            {
                activeTetris.SetInput(Vector3.left);
            }
            if (direction == "right")
            {
                activeTetris.SetInput(Vector3.right);
            }
            if (direction == "forward")
            {
                activeTetris.SetInput(Vector3.forward);
            }
            if (direction == "back")
            {
                activeTetris.SetInput(Vector3.back);
            }
        }
    }

    public void RotateBlock(string rotation)
    {
        if (activeBlock != null)
        {
            if (rotation == "posX")
            {
                activeTetris.SetRotationInput(new Vector3(90, 0, 0));
            }
            if (rotation == "negX")
            {
                activeTetris.SetRotationInput(new Vector3(-90, 0, 0));
            }
            if (rotation == "posY")
            {
                activeTetris.SetRotationInput(new Vector3(0, 90, 0));
            }
            if (rotation == "negY")
            {
                activeTetris.SetRotationInput(new Vector3(0, -90, 0));
            }
            if (rotation == "posZ")
            {
                activeTetris.SetRotationInput(new Vector3(0, 0, 90));
            }
            if (rotation == "negZ")
            {
                activeTetris.SetRotationInput(new Vector3(0, 0, -90));
            }

        }
    }

    public void SwitchInputs()
    {
        moveIsOn = !moveIsOn;
        SetInputs();
    }

    private void SetInputs()
    {
        moveCanvas.SetActive(moveIsOn);
        foreach (GameObject c in rotateCanvases)
        {
            c.SetActive(!moveIsOn);
        }
    }

    public void SetHighSpeed()
    {
        activeTetris.SetSpeed();
    }
}
