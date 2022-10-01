using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapGenerator : MonoBehaviour
{
    /// <summary>
    /// 最大房间块尺寸
    /// </summary>
    private int maxCellSize = 20;
    /// <summary>
    /// 最小房间块尺寸
    /// </summary>
    private int minCellSize = 5;
    private int mapXSeed = 0;
    private int mapYSeed = 0;
    /// <summary>
    /// 房间块数量
    /// </summary>
    private int cellNumber = 10;
    /// <summary>
    /// 地图大小
    /// </summary>
    private int mapSize = 100;
    /// <summary>
    /// 房间块大小差
    /// </summary>
    private float cellSizeDispersion = 5;
    /// <summary>
    /// 房间块数组
    /// </summary>
    private Cell[] cells;
    /// <summary>
    /// 房间块预设
    /// </summary>
    private GameObject cellGameObject;
    public bool isDrawCell = false;
    public Cell[] getCells()
    {
        return cells;
    }
    public void InitMap()
    {
        cells = new Cell[cellNumber];
        for (int i = 0; i < cellNumber; i++)
        {
            GameObject cell = Instantiate(cellGameObject);
            cell.name = "Cell" + i.ToString();
            cells[i] = cell.GetComponent<Cell>();
            cells[i].cellId = i;
        }
    }
    public void Generate()
    {
        int avrageSize = (int)(maxCellSize + minCellSize)/2;
        for (int i = 0; i < cellNumber; i++)
        {
            // 上一个区块
            Cell cellPrevious = cells[i];

            if(i == 0)
            {
                // 初始区块
                cells[i].cellX = 0;
                cells[i].cellY = 0;
            }
            else
            {
                //int p = Random.Range(1, i);
                //Debug.Log(p);
                cellPrevious = cells[i-1];
                // 初始后续区块
                // 中心位置
                Random.InitState((int)(mapXSeed + cellPrevious.cellX * 10000));
                cells[i].cellX = (int)((Random.value - 0.5f) * mapSize);
                Random.InitState((int)(mapXSeed + cellPrevious.cellY * 1000));
                cells[i].cellY = (int)((Random.value - 0.5f) * mapSize);

                // cells[i].cellX = NormalRandom.Rand((int)(mapXSeed + cellPrevious.cellX * 10000), 0, cellBetweenDispersion);
                // cells[i].cellY = NormalRandom.Rand((int)(mapYSeed + cellPrevious.cellX * 1000), 0, cellBetweenDispersion);
            }
            // 大小
            cells[i].cellHight = (int)NormalRandom.Rand(mapYSeed, (float)avrageSize + (float)cellPrevious.cellHight/10f, cellSizeDispersion);
            cells[i].cellWidth = (int)NormalRandom.Rand(mapXSeed, (float)avrageSize + (float)cellPrevious.cellWidth/10f, cellSizeDispersion);

            if(cells[i].cellWidth < minCellSize)
            {
                cells[i].cellWidth = minCellSize;
            }
            if(cells[i].cellHight < minCellSize)
            {
                cells[i].cellHight = minCellSize;
            }

            float shiftMultiplier = 1;
            bool isOverlapping = false;
            bool lastIsOverlapping = false;

            // 重叠判断
            for (int j = 0; j < i; j++)
            {   
                if(j == i)continue;
                if(lastIsOverlapping) j = 0;
                Cell p = cells[j];
                // 如果重叠了
                if((p.cellWidth + cells[i].cellWidth)/2 > Mathf.Abs(p.cellX - cells[i].cellX) && 
                    (p.cellHight + cells[i].cellHight)/2 > Mathf.Abs(p.cellY - cells[i].cellY)){isOverlapping = true;}

                if(!isOverlapping)
                {
                    lastIsOverlapping = isOverlapping;
                    continue;
                }

                if(Mathf.Abs(p.cellX - cells[i].cellX) < Mathf.Abs(p.cellY - cells[i].cellY))
                {
                    if(p.cellX - cells[i].cellX <= 0)
                    {
                        cells[i].cellX = (int)(p.cellX + shiftMultiplier*((p.cellWidth + cells[i].cellWidth)/2));
                    }
                    if(p.cellX - cells[i].cellX > 0)
                    {
                        cells[i].cellX = (int)(p.cellX - shiftMultiplier*((p.cellWidth + cells[i].cellWidth)/2));
                    }
                }
                else
                {
                    if(p.cellY - cells[i].cellY <= 0)
                    {
                        cells[i].cellY = (int)(p.cellY + shiftMultiplier*((p.cellHight + cells[i].cellHight)/2));
                    }
                    if(p.cellY - cells[i].cellY > 0)
                    {
                        cells[i].cellY = (int)(p.cellY - shiftMultiplier*((p.cellHight + cells[i].cellHight)/2));
                    }
                }
                lastIsOverlapping = isOverlapping;
                isOverlapping=false;
                shiftMultiplier+=0.1f;
                j = -1;
            }
            cells[i].InitCell();
            cells[i].isDrawCell = isDrawCell;
        }
    }
    public void IsDrawCell(bool isDrawCell)
    {
        this.isDrawCell = isDrawCell;
    }

    public void SetCellGameObject(GameObject cellGameObject)
    {
        this.cellGameObject = cellGameObject;
    }

    public void SetSeed(int mapXSeed, int mapYSeed)
    {
        this.mapXSeed = mapXSeed;
        this.mapYSeed = mapYSeed;
    }

    public void SetCellSize(int maxCellSize, int minCellSize)
    {
        this.maxCellSize = maxCellSize;
        this.minCellSize = minCellSize;
    }
    public void SetCellNumber(int cellNumber)
    {
        this.cellNumber = cellNumber;
    }
    public void SetMapSize(int mapSize)
    {
        this.mapSize = mapSize;
    }
    public void SetCellSizeDispersion(float cellSizeDispersion)
    {
        this.cellSizeDispersion = cellSizeDispersion;
    }
}
