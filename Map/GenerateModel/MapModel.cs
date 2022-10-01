using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapModel : MonoBehaviour
{
    [Tooltip("房间块瓦片预设")]
    private GameObject tileGameObject;
    [Tooltip("链接块瓦片预设")]
    public GameObject linkGameObject;
    /// <summary>
    /// 初始化瓦片旋转
    /// </summary>
    private Quaternion Qzero = new Quaternion(0,0,0,0);
    /// <summary>
    /// 链接起点与终点坐标
    /// </summary>
    private Vector2[] v2;


    private void Start()
    {
        
    }
    // Update is called once per frame
    public void Update()
    {
        
    }
    public void setTileGameObject(GameObject tileGameObject)
    {
        this.tileGameObject = tileGameObject;
    }
    public void setLinkGameObject(GameObject linkGameObject)
    {
        this.linkGameObject = linkGameObject;
    }
    public void setV2(Vector2[] v2)
    {
        this.v2 = v2;
    }
    /// <summary>
    /// 合并Mesh的Mesh数据
    /// </summary>
    private CombineInstance[] combine;
    private int index = 0;
    private GameObject tempGameObject;
    /// <summary>
    /// 瓦片的Transform
    /// </summary>
    private Transform tileTransform;
    /// <summary>
    /// 合并后的Transform
    /// </summary>
    private Transform combineTransform;
    private MeshFilter meshFilter;

    public void GenerateMapModel(Cell[] cells)
    {
        for (int i = 0; i < cells.Length; i++)
        {
            int hight = cells[i].cellHight;
            int width = cells[i].cellWidth;
            tileTransform = cells[i].transform.Find("Tile");
            combineTransform = cells[i].transform.Find("Combine");

            combine = new CombineInstance[width * hight];

            index = 0;
            for (int j = 0; j < hight; j++)
            {
                for (int k = 0; k < width; k++)
                {
                    tempGameObject = Instantiate(tileGameObject, new Vector3(cells[i].cellX - width/2 + k, 0, cells[i].cellY - hight/2 + j), Qzero);
                    tempGameObject.transform.parent = tileTransform;

                    // 提取单个tile的Mesh

                    meshFilter = tempGameObject.GetComponent<MeshFilter>();
                    combine[index].mesh = meshFilter.sharedMesh;
                    combine[index].transform = meshFilter.transform.localToWorldMatrix;
                    index++;
                }
            }
            Destroy(tileTransform.gameObject);
            combineTransform.GetComponent<MeshFilter>().mesh = new Mesh();
            combineTransform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
            combineTransform.GetComponent<MeshFilter>().transform.position = Vector3.zero;
            combineTransform.gameObject.SetActive(true);
        }
    }
    public int linkWidth;
    private string linkName;
    private GameObject linkParent;
    public void GenerateConnectModel(float[][] minGraph)
    {
        for (int i = 0; i < minGraph.Length; i++)
        {
            for (int j = 0; j < minGraph.Length; j++)
            {
                if(minGraph[i][j] != float.MaxValue)
                {
                    int disX = (int)(v2[i].x - v2[j].x);
                    int disY = (int)(v2[i].y - v2[j].y);

                    linkName = "link "+i.ToString()+"-"+j.ToString();
                    linkParent = Instantiate(linkGameObject, new Vector3(v2[i].x, 0, v2[i].y), Qzero);
                    linkParent.name = linkName;

                    index = 0;
                    combine = new CombineInstance[(Mathf.Abs(disX) + Mathf.Abs(disY)) * linkWidth];
                    tileTransform = linkParent.transform.Find("Tile");
                    combineTransform = linkParent.transform.Find("Combine");
                
                    InstantitateX(v2[i], disX);
                    InstantitateY(new Vector2(v2[i].x - disX, v2[i].y), disY);
                    Destroy(tileTransform.gameObject);


                    combineTransform.GetComponent<MeshFilter>().mesh = new Mesh();
                    combineTransform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
                    combineTransform.GetComponent<MeshFilter>().transform.position = Vector3.zero;
                    combineTransform.gameObject.SetActive(true);
                }
            }
        }

    }

    private bool isNegative = false;
    private void InstantitateX(Vector2 start, int dis)
    {
        if(dis < 0)
        {
            isNegative = true;
            dis = -dis;
        }
        else
        {
            isNegative = false;
        }

        for (int i = 0; i < dis; i++)
        {
            int tempI = i;
            if(isNegative) tempI = -i;

            for (int j = 0; j < linkWidth; j++)
            {

                tempGameObject = Instantiate(tileGameObject, new Vector3(start.x - tempI, 0, start.y - linkWidth/2 + j), Qzero);
                tempGameObject.transform.parent = tileTransform;

                // 提取单个tile的Mesh
                meshFilter = tempGameObject.GetComponent<MeshFilter>();
                combine[index].mesh = meshFilter.sharedMesh;
                combine[index].transform = meshFilter.transform.localToWorldMatrix;
                index++;
            }
        }

        // if(dis > 0)
        // {
        //     for (int i = 0; i < dis; i++)
        //     {
        //         for (int j = 0; j < linkWidth; j++)
        //         {
        //             tempGameObject = Instantiate(tileGameObject, new Vector3(start.x - i, 0, start.y - linkWidth/2 + j), Qzero);
        //         }
        //     }
        // }
        // else if(dis < 0)
        // {
        //     for (int i = 0; i < -dis; i++)
        //     {
        //         for (int j = 0; j < linkWidth; j++)
        //         {
        //             tempGameObject = Instantiate(tileGameObject, new Vector3(start.x + i, 0, start.y - linkWidth/2 + j), Qzero);  
        //         }
        //     } 
        // }
    }
    private void InstantitateY(Vector2 start, int dis)
    {

        if(dis < 0)
        {
            isNegative = true;
            dis = -dis;
        }
        else
        {
            isNegative = false;
        }

        for (int i = 0; i < dis; i++)
        {
            int tempI = i;
            if(isNegative) tempI = -i;

            for (int j = 0; j < linkWidth; j++)
            {

                tempGameObject = Instantiate(tileGameObject, new Vector3(start.x - linkWidth/2 + j, 0, start.y - tempI), Qzero);  
                tempGameObject.transform.parent = tileTransform;

                // 提取单个tile的Mesh
                meshFilter = tempGameObject.GetComponent<MeshFilter>();
                combine[index].mesh = meshFilter.sharedMesh;
                combine[index].transform = meshFilter.transform.localToWorldMatrix;
                index++;
            }
        }

        // if(dis > 0)
        // {
        //     for (int i = 0; i < dis; i++)
        //     {
        //         for (int j = 0; j < linkWidth; j++)
        //         {
        //             tempGameObject = Instantiate(tileGameObject, new Vector3(start.x - linkWidth/2 + j, 0, start.y - i), Qzero);    
        //         }
        //     }
        // }
        // else if(dis < 0)
        // {
        //     for (int i = 0; i < -dis; i++)
        //     {
        //         for (int j = 0; j < linkWidth; j++)
        //         {
        //             tempGameObject = Instantiate(tileGameObject, new Vector3(start.x - linkWidth/2 + j, 0, start.y + i), Qzero);
        //         }
        //     } 
        // }
    }
}
