using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playfield : MonoBehaviour
{
    public static Playfield instance;

    public int gridSizeX, gridSizeY, gridSizeZ;

    [Header("Blocks")]
    public GameObject[] blockList;
    public GameObject[] ghostList;

    [Header("Playfield Visuals")]
    public GameObject bottomPlane;
    public GameObject N, S, W, E;

    public Transform[,,] theGrid;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        theGrid = new Transform[gridSizeX, gridSizeY, gridSizeZ];
        SpawnNewBlock();
    }

    public Vector3 Round(Vector3 vec)
    {
        return new Vector3(Mathf.RoundToInt(vec.x), Mathf.RoundToInt(vec.y), Mathf.RoundToInt(vec.z));
    }

    public bool CheckInsideGrid(Vector3 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < gridSizeX
             && (int)pos.z >= 0 && (int)pos.z < gridSizeZ
             && (int)pos.y >= 0);
    }

    public void UpdateGrid(Block bl)
    {
        //Delete possible parent objects
        for (int x = 0; x < gridSizeX; x++)
            for (int z = 0; z < gridSizeZ; z++)
                for (int y = 0; y < gridSizeY; y++)
                {
                    if (theGrid[x, y, z] != null)
                    {
                        if (theGrid[x, y, z].parent == bl.transform)
                        {
                            theGrid[x, y, z] = null;
                        }
                    }
                }

        //Fill in all child objects
        foreach (Transform child in bl.transform)
        {
            Vector3 pos = Round(child.position);
            if (pos.y < gridSizeY)
            {
                theGrid[(int)pos.x, (int)pos.y, (int)pos.z] = child;
            }
        }
    }

    public Transform GetTransformOnGridPos(Vector3 pos)
    {
        if (pos.y > gridSizeY - 1)
        {
            return null;
        }
        else
        {
            return theGrid[(int)pos.x, (int)pos.y, (int)pos.z];
        }
    }

    public void SpawnNewBlock()
    {
        Vector3 spawnPoint = new Vector3((int)(transform.position.x + (float)gridSizeX / 2),
                                         (int)(transform.position.y + gridSizeY),
                                         (int)(transform.position.z + (float)gridSizeZ / 2));

        int randomIndex = Random.Range(0, blockList.Length);

        //Spawn block
        GameObject newBlock = Instantiate(blockList[randomIndex], spawnPoint, Quaternion.identity);

        //Spawn ghost block
        GameObject newGhost = Instantiate(ghostList[randomIndex], spawnPoint, Quaternion.identity);
        newGhost.GetComponent<GhostBlock>().SetParent(newBlock);
    }

    void OnDrawGizmos()
    {
        if (bottomPlane != null)
        {
            //Resize bottom plane
            Vector3 scaler = new Vector3((float)gridSizeX / 10, 1, (float)gridSizeZ / 10);
            bottomPlane.transform.localScale = scaler;

            //Reposition
            bottomPlane.transform.position = new Vector3(transform.position.x + (float)gridSizeX / 2,
                                                         transform.position.y,
                                                         transform.position.z + (float)gridSizeZ / 2);

            //Retile material
            bottomPlane.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeZ);
        }

        if (N != null)
        {
            //Resize bottom plane
            Vector3 scaler = new Vector3((float)gridSizeX / 10, 1, (float)gridSizeY / 10);
            N.transform.localScale = scaler;

            //Reposition
            N.transform.position = new Vector3(transform.position.x + (float)gridSizeX / 2,
                                                         transform.position.y + (float)gridSizeY / 2,
                                                         transform.position.z + gridSizeZ);

            //Rescale material
            N.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeY);
        }

        if (S != null)
        {
            //Resize bottom plane
            Vector3 scaler = new Vector3((float)gridSizeX / 10, 1, (float)gridSizeY / 10);
            S.transform.localScale = scaler;

            //Reposition
            S.transform.position = new Vector3(transform.position.x + (float)gridSizeX / 2,
                                                         transform.position.y + (float)gridSizeY / 2,
                                                         transform.position.z);

            //Rescale material
            S.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeX, gridSizeY);
        }

        if (E != null)
        {
            //Resize bottom plane
            Vector3 scaler = new Vector3((float)gridSizeZ / 10, 1, (float)gridSizeY / 10);
            E.transform.localScale = scaler;

            //Reposition
            E.transform.position = new Vector3(transform.position.x + gridSizeX,
                                                         transform.position.y + (float)gridSizeY / 2,
                                                         transform.position.z + (float)gridSizeZ / 2);

            //Retile material
            E.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeZ, gridSizeY);
        }

        if (W != null)
        {
            //Resize bottom plane
            Vector3 scaler = new Vector3((float)gridSizeZ / 10, 1, (float)gridSizeY / 10);
            W.transform.localScale = scaler;

            //Reposition
            W.transform.position = new Vector3(transform.position.x,
                                                         transform.position.y + (float)gridSizeY / 2,
                                                         transform.position.z + (float)gridSizeZ / 2);

            //Rescale material
            W.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale = new Vector2(gridSizeZ, gridSizeY);
        }
    }

    public void DeleteLayer()
    {
        int layers = 0;
        //Check all y positions
        for (int y = gridSizeY - 1; y >= 0; y--)
        {
            if (CheckFullLayer(y))
            {
                DeleteLayerAt(y);
                //Move all other layers by 1
                MoveAllLayerDown(y);
                layers++;
            }
        }
        if (layers > 0)
        {
            GameManager.instance.LayersCleared(layers);
        }
    }

    //Check all positions in the grid based on the y position
    bool CheckFullLayer(int y)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                //if found a space on the grid without a block return false
                if (theGrid[x, y, z] == null)
                {
                    //This means that the layer is not full
                    return false;
                }
            }
        }
        return true;
    }

    //Delete line created
    void DeleteLayerAt(int y)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                Destroy(theGrid[x, y, z].gameObject);
                theGrid[x, y, z] = null;
            }
        }
    }

    void MoveAllLayerDown(int y)
    {
        for (int i = y; i < gridSizeY; i++)
        {
            MoveOneLayerDown(i);
        }
    }

    void MoveOneLayerDown(int y)
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                if (theGrid[x, y, z] != null)
                {
                    theGrid[x, y - 1, z] = theGrid[x, y, z];
                    theGrid[x, y, z] = null;
                    theGrid[x, y - 1, z].position += Vector3.down;
                }
            }
        }
    }
}
