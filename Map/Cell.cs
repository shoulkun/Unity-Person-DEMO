using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地图房间块
/// </summary>
public class Cell : MonoBehaviour
{
    [Tooltip("房间块ID")]
    public int cellId;
    [Tooltip("房间块长度尺寸")]
    public int cellHight;
    [Tooltip("房间块宽度尺寸")]
    public int cellWidth;
    [Tooltip("房间块x位置")]
    public int cellX;
    [Tooltip("房间块y位置")]
    public int cellY;
    [Tooltip("显示房间块调试")]
    public bool isDrawCell = false;

    private Color[] colors = new Color[5]{Color.black, Color.green, Color.blue, Color.yellow, Color.red};


    /// <summary>
    /// 房间块四角顶点位置信息
    /// </summary>
    private int[,] cellVertex;    // [0, 0] = x0.    [0, 1] = y0
    
    private BoxCollider cellCollider;
    private void Start()
    {
    }

    private void Update()
    {
        if(isDrawCell)
        {
            DrawCell();
        }
    }

    /// <summary>
    /// 初始化房间块
    /// </summary>
    public void InitCell()
    {
        transform.position = new Vector3(cellX, 0, cellY);  //房间块位置信息
        cellCollider = gameObject.GetComponent<BoxCollider>() as BoxCollider;
        cellCollider.size = new Vector3(cellWidth, 10, cellHight);  //房间检测范围

        cellVertex = new int[4,2];
        UpdateCellPos();

        gameObject.GetComponent<CellCameraSwitch>().setCameraLimit(cellWidth/2, cellHight/2);
    }

    /// <summary>
    /// 更新房间块四角信息
    /// </summary>
    private void UpdateCellPos()
    {
        cellVertex[0,0] = cellX - cellWidth/2;
        cellVertex[0,1] = cellY + cellHight/2;
        
        cellVertex[1,0] = cellX + cellWidth/2;
        cellVertex[1,1] = cellY + cellHight/2;

        cellVertex[2,0] = cellX + cellWidth/2;
        cellVertex[2,1] = cellY - cellHight/2;

        cellVertex[3,0] = cellX - cellWidth/2;
        cellVertex[3,1] = cellY - cellHight/2;
    }

    private void DrawCell()
    {
        cellX = (int)transform.position.x;
        cellY = (int)transform.position.z;
        UpdateCellPos();

        int VertexNum = cellVertex.GetLength(0);
        for (int i = 0; i < VertexNum; i++)
        {
            Debug.DrawLine
            (
                new Vector3(cellVertex[i,0], 0, cellVertex[i,1]),
                new Vector3(cellVertex[(i + 1) % VertexNum, 0], 0, cellVertex[(i + 1) % VertexNum, 1])
            );
        }
    }
}
