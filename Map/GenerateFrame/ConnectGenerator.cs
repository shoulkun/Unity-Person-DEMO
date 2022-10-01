using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectGenerator : MonoBehaviour
{
    /// <summary>
    /// 最小房间块大小
    /// </summary>
    public float minCellSize;
    /// <summary>
    /// 房间块数组
    /// </summary>
    private Cell[] cells;
    /// <summary>
    /// 房间块中心位置
    /// </summary>
    private Vector2[] v2;
    /// <summary>
    /// 房间块排序数组
    /// </summary>
    private int[] sortedV2Index; 
    /// <summary>
    /// BowyerWatson中临时三角形数组
    /// </summary>
    private List<Triangles> tempTriangles;
    /// <summary>
    /// BowyerWatson中三角形数组
    /// </summary>
    private List<Triangles> triangles;
    /// <summary>
    /// BowyerWatson中的超级三角形
    /// </summary>
    private Triangles superTriangle;
    /// <summary>
    /// 所有房间块全连接的边
    /// </summary>
    private List<Edge> edgeBuffer;
    /// <summary>
    /// 绘制边，调试用
    /// </summary>
    private bool isDrawLink;
    /// <summary>
    /// 链接二维表
    /// </summary>
    private float[][] connectGraph;
    /// <summary>
    /// 最短路径表
    /// </summary>
    private float[][] minGraph;

    public float[][] getMinGraph()
    {
        return minGraph;
    }
    public Vector2[] getV2()
    {
        return v2;
    }
    public Cell[] getCells()
    {
        return cells;
    }
    public void SetCell(Cell[] cells)
    {
        this.cells = cells;
    }

    private class Edge
    {
        public Vector2[] point = new Vector2[2];
        public Edge(Vector2 a, Vector2 b)
        {
            point[0] = a;
            point[1] = b;

            for (int i = 0; i < point.Length; i++)
            {
                for (int j = 0; j < point.Length; j++)
                {
                    Vector2 temp = point[j];
                    if(point[i].magnitude > point[j].magnitude)
                    {
                        point[j] = point[i];
                        point[i] = temp;
                    }
                }
            }
        }
        public bool isEque(Edge t)
        {
            bool flag = false;
            if (t.point[0] == this.point[0] && t.point[1] == this.point[1] ||
                t.point[1] == this.point[0] && t.point[0] == this.point[1])
            {
                flag = true;
            }
            return flag;
        }
    }
    private class Triangles
    {
        public Vector2[] point = new Vector2[3];

        public Triangles(Vector2 a, Vector2 b, Vector2 c)
        {
            point[0] = a;
            point[1] = b;
            point[2] = c;


            for (int i = 0; i < point.Length; i++)
            {
                for (int j = 0; j < point.Length; j++)
                {
                    Vector2 temp = point[j];
                    if(point[i].magnitude > point[j].magnitude)
                    {
                        point[j] = point[i];
                        point[i] = temp;
                    }
                }
            }
        }

        /// <summary>
        /// 获取三角形的端点
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        public void GetPoint(out Vector2 a, out Vector2 b, out Vector2 c)
        {
            a = this.point[0];
            b = this.point[1];
            c = this.point[2];
        }
        /// <summary>
        /// 判断是否与超级三角形有相连接的情况
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool isRelated(Triangles t)
        {
            bool flag = false;
            // this
            for (int i = 0; i < t.point.Length; i++)
            {
                for (int j = 0; j < this.point.Length; j++)
                {
                    if(t.point[i] == this.point[j])
                    {
                        flag = true;
                        return flag;
                    }
                }
            }
            return flag;
            // if (this.point[0] == t.point[0] && this.point[1] == t.point[1] ||
            //     this.point[0] == t.point[0] && this.point[2] == t.point[2] ||
            //     this.point[1] == t.point[1] && this.point[2] == t.point[2]
            //     )
            // {
            //     return true;
            // }
            // return false;
        }
        /// <summary>
        /// 判断三角形是否相同
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public bool isEque(Triangles t)
        {
            // if(this.point[0] == t.point[0] && this.point[1] == t.point[1] && this.point[2] == t.point[2])
            // {
            //     return true;
            // }
            for (int i = 0; i < this.point.Length; i++)
            {
                for (int j = 0; j < this.point.Length; j++)
                {
                    if (i == j){continue;}
                    for (int k = 0; k < this.point.Length; k++)
                    {
                        if (j == k || i == k){continue;}
                        for (int x = 0; x < t.point.Length; x++)
                        {
                            for (int y = 0; y < t.point.Length; y++)
                            {
                                if(x == y){continue;}
                                for (int z = 0; z < t.point.Length; z++)
                                {
                                    if(y == z || x == z){continue;}
                                    if(this.point[i] == t.point[x] && this.point[j] == t.point[y] && this.point[k] == t.point[z])
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
    /// <summary>
    /// 初始化链接块
    /// </summary>
    public void InitCells()
    {
        tempTriangles = new List<Triangles>();
        triangles = new List<Triangles>();

        v2 = new Vector2[cells.Length];
        sortedV2Index = new int[cells.Length];

        connectGraph = new float[cells.Length][];
        minGraph = new float[cells.Length][];
        for (int i = 0; i < cells.Length; i++)
        {
            connectGraph[i] = new float[cells.Length];
            minGraph[i] = new float[cells.Length];
            
            for (int j = 0; j < cells.Length; j++)
            {
                connectGraph[i][j] = float.MaxValue;
                minGraph[i][j] = float.MaxValue;
            }
        }
        

        for (int i = 0; i < cells.Length; i++)
        {
            v2[i] = new Vector2(cells[i].cellX, cells[i].cellY);
            sortedV2Index[i] = i;
        }

        BowyerWatson();
        GenerateGraph();
    }

    /// <summary>
    /// 生成最终链接图
    /// </summary>
    private void GenerateGraph()
    {
        // 构造连通图
        for (int i = 0; i < triangles.Count; i++)
        {
            // 遍历三角形的每一个点
            for (int j = 0; j < triangles[i].point.Length; j++)
            {
                int startIndex = 0;
                // 寻找该点在v2中的index
                for (int n = 0; n < v2.Length; n++)
                {
                    if(triangles[i].point[j] == v2[n])
                    {
                        startIndex = n;
                        break;
                    }
                }
                for (int k = 0; k < triangles[i].point.Length; k++)
                {
                    int endIndex = 0;
                    if (j == k){continue;}
                    for (int n = 0; n < v2.Length; n++)
                    {
                        if(triangles[i].point[k] == v2[n])
                        {
                            endIndex = n;
                            break;
                        }
                    }
                    if(connectGraph[startIndex][endIndex] == float.MaxValue)
                    {
                        connectGraph[startIndex][endIndex] = (triangles[i].point[j] - triangles[i].point[k]).magnitude;
                        connectGraph[endIndex][startIndex] = connectGraph[startIndex][endIndex];
                    }
                }
            }
        }
        int root = 0;
        Prim(root);
        GenerateShortcut();
    }
    /// <summary>
    /// 生成最短连通
    /// </summary>
    private void GenerateShortcut()
    {
        Cell startCell;
        Cell endCell;
        Vector2 sv2, ev2;
        float dis;
        float minDis = float.MaxValue;
        float diffDis;
        
        for (int i = 0; i < minGraph.Length; i++)
        {
            for (int j = 0; j < minGraph.Length; j++)
            {
                if(minGraph[i][j] < minDis)
                {
                    minDis = minGraph[i][j];
                }
            }
        }

        for (int i = 0; i < minGraph.Length; i++)
        {
            for (int j = 0; j < minGraph.Length; j++)
            {
                // 没有通路
                if(minGraph[i][j] == float.MaxValue)
                {
                    startCell = cells[i];
                    endCell = cells[j];
                    sv2 = new Vector2(startCell.cellX, startCell.cellY);
                    ev2 = new Vector2(endCell.cellX, endCell.cellY);
                    // 比较 dx和 dy差距
                    // dx 比 dy 大
                    if(Mathf.Abs(sv2.x - ev2.x) > Mathf.Abs(sv2.y - ev2.y))
                    {
                        diffDis = Mathf.Abs(sv2.x - ev2.x) - (startCell.cellWidth/2 + endCell.cellWidth/2);
                    }
                    else
                    {
                        diffDis = Mathf.Abs(sv2.y - ev2.y) - (startCell.cellHight/2 + endCell.cellHight/2);
                    }
                    if(minCellSize/2 < diffDis && diffDis < minCellSize)
                    {
                        dis = (sv2-ev2).magnitude;
                        // Debug.Log("start" + startCell.cellId);
                        // Debug.Log("end" + endCell.cellId);
                        // Debug.Log(diffDis);
                        // Debug.Log("连接近路");
                        minGraph[i][j] = dis;
                        minGraph[j][i] = dis;
                    }
                    // dis = (sv2-ev2).magnitude;
                    // if(dis < minDis)
                    // {                        
                    //     Debug.Log("连接近路");
                    //     minGraph[i][j] = dis;
                    //     minGraph[j][i] = dis;
                    // }
                }
            }
        }
    }
    /// <summary>
    /// 普里姆算法，最小生成树
    /// </summary>
    /// <param name="root"></param>
    private void Prim(int root)
    {
        // 已添加的点
        List<int> point = new List<int>();
        point.Add(root);

        while(point.Count < connectGraph.Length)
        {
            float minDis = float.MaxValue;
            int minDisStartPoint = root;
            int minDisEndPoint = root;
            // 遍历已添加点
            for (int i = 0; i < point.Count; i++)
            {
                //下面是纯树状地图代码
                //遍历已添加点的连接点，找出距离最短的点
                for (int j = 0; j < connectGraph[point[i]].Length; j++)
                {
                    if(connectGraph[point[i]][j] < minDis)
                    {
                        // End点是否已经是其他点是其他点的End点
                        bool isExistInPoint = false;
                        // 忽略已添加的边
                        if(minGraph[point[i]][j] != float.MaxValue || minGraph[j][point[i]] != float.MaxValue){continue;}
                        for (int k = 0; k < point.Count; k++)
                        {
                            if(j == point[k])
                            {
                                isExistInPoint = true;
                            }
                        }
                        if(isExistInPoint){continue;}
                        minDisStartPoint = point[i];
                        minDisEndPoint = j;
                        minDis = connectGraph[minDisStartPoint][minDisEndPoint];
                        // Debug.Log("起点="+minDisStartPoint);
                        // Debug.Log("终点="+minDisEndPoint);
                        // Debug.Log("距离="+minDis);
                    }
                }
            }

                // 下面是有迂回版本
                // 遍历已添加点的连接点，找出距离最短的点
            //     for (int j = 0; j < connectGraph[point[i]].Length; j++)
            //     {
            //         if(connectGraph[point[i]][j] < minDis)
            //         {
            //             // End点是否已经是其他点是其他点的End点
            //             // bool isExistInPoint = false;
            //             // 忽略已添加的边
            //             if(minGraph[point[i]][j] != float.MaxValue || minGraph[j][point[i]] != float.MaxValue){continue;}
            //             // for (int k = 0; k < point.Count; k++)
            //             // {
            //             //     if(j == point[k])
            //             //     {
            //             //         isExistInPoint = true;
            //             //     }
            //             // }
            //             // if(isExistInPoint){continue;}
            //             minDisStartPoint = point[i];
            //             minDisEndPoint = j;
            //             minDis = connectGraph[minDisStartPoint][minDisEndPoint];
            //             Debug.Log("起点="+minDisStartPoint);
            //             Debug.Log("终点="+minDisEndPoint);
            //             Debug.Log("距离="+minDis);
            //         }
            //     }
            // }
            point.Add(minDisEndPoint);

            // point 去重
            // for (int i = 0; i < point.Count; i++)
            // {
            //     for (int j = 0; j < point.Count; j++)
            //     {
            //         if(i == j){continue;}
            //         if (point[i] == point[j])
            //         {
            //             point.Remove(point[j]);
            //             i = -1;
            //             break;
            //         }
            //     }
            // }

            minGraph[minDisStartPoint][minDisEndPoint] = minDis;
            // Debug.Log("最终决定:");
            // Debug.Log("起点="+minDisStartPoint);
            // Debug.Log("终点="+minDisEndPoint);
            // Debug.Log("距离="+minDis);
        }
    }
    /// <summary>
    /// 生成超级三角形
    /// </summary>
    private void GenerateSuperTriangle()
    {
        Vector2 maxABSHight, maxABSWidth, maxABSWidthLeft;
        maxABSHight = maxABSWidth = maxABSWidthLeft = Vector2.zero;

        for (int i = 0; i < cells.Length; i++)
        {
            if(Mathf.Abs(v2[i].y) > maxABSHight.y)
            {
                maxABSHight.y = Mathf.Abs(v2[i].y) * 40;
                maxABSWidth.y = -maxABSHight.y / 2;
            }

            if(Mathf.Abs(v2[i].x) > maxABSWidth.x)
            {
                maxABSWidth.x = Mathf.Abs(v2[i].x) * 40;
            }
        }
        maxABSWidth.y -= maxABSHight.y * 1/10;
        maxABSWidthLeft = maxABSWidth;
        maxABSWidthLeft.x = -maxABSWidthLeft.x;

        Vector2 a,b,c;
        a = maxABSHight;

        b = maxABSWidth;

        c = maxABSWidthLeft;

        superTriangle = new Triangles(a, b, c);
        tempTriangles.Add(superTriangle);
        triangles.Add(superTriangle);
    }
    /// <summary>
    /// BowyerWatson算法，生成各房间块之间的三角形链接
    /// </summary>
    private void BowyerWatson()
    {
        if(cells.Length < 3)
        {
            return;
        }

        // 点顺序从左到右排列
        for (int i = 0; i < v2.Length; i++)
        {
            for (int j = i; j < v2.Length; j++)
            {
                if(v2[sortedV2Index[j]].x < v2[sortedV2Index[i]].x)
                {
                    int temp = sortedV2Index[j];
                    sortedV2Index[j] = sortedV2Index[i];
                    sortedV2Index[i] = temp;
                }
            }
        }

        //将超级三角形保存至未确定三角形列表（temp triangles）
        GenerateSuperTriangle();
        //遍历基于sortedV2Index顺序的vertices中每一个点
        for (int i = 0; i < sortedV2Index.Length; i++)
        {
            //初始化边缓存数组（edge buffer）
            edgeBuffer = new List<Edge>();
            //遍历temp triangles中的每一个三角形
            for(int j = 0; j < tempTriangles.Count; j++)
            {
                //计算该三角形的圆心和半径
                float r;
                Vector2 center;
                GetTriangleExcenterRadius(tempTriangles[j], out r, out center);
                // 待检测点
                Vector2 tempPoint = v2[sortedV2Index[i]];
                //如果该点在外接圆的右侧
                if ((tempPoint - center).magnitude > r && tempPoint.x > center.x)
                {
                    //则该三角形为Delaunay三角形，保存到triangles
                    triangles.Add(tempTriangles[j]);
                    //并在temp里去除掉
                    tempTriangles.Remove(tempTriangles[j]);
                    j = -1;
                    continue;
                }
                //如果该点在外接圆外（即也不是外接圆右侧）
                if ((tempPoint - center).magnitude > r && !(tempPoint.x > center.x))
                {
                    continue;
                }
                //如果该点在外接圆内
                if((tempPoint - center).magnitude < r)
                {
                    //则该三角形不为Delaunay三角形
                    //将三边保存至edge buffer
                    edgeBuffer.Add(new Edge(tempTriangles[j].point[0], tempTriangles[j].point[1]));
                    edgeBuffer.Add(new Edge(tempTriangles[j].point[1], tempTriangles[j].point[2]));
                    edgeBuffer.Add(new Edge(tempTriangles[j].point[2], tempTriangles[j].point[0]));
                    //在temp中去除掉该三角形
                    tempTriangles.Remove(tempTriangles[j]);
                    j = -1;
                }
            }
            //对edge buffer进行去重,去双边！！！
            for (int n = 0; n < edgeBuffer.Count; n++)
            {
                for (int m = 0; m < n; m++)
                {
                    if(edgeBuffer[n].isEque(edgeBuffer[m]))
                    {
                        // 去双边！
                        Edge temp = edgeBuffer[n];
                        edgeBuffer.Remove(edgeBuffer[m]);
                        edgeBuffer.Remove(temp);

                        n = -1;
                        m = -1;
                        break;
                    }
                }
            }

            //将edge buffer中的边与当前的点进行组合成若干三角形并保存至temp triangles中
            for (int n = 0; n < edgeBuffer.Count; n++)
            {
                Triangles t = new Triangles(edgeBuffer[n].point[0], edgeBuffer[n].point[1], v2[sortedV2Index[i]]);
                tempTriangles.Add(t);
            }
        }
        //将triangles与temp triangles进行合并
        for (int n = 0; n < triangles.Count; n++)
        {
            for (int m = 0; m < tempTriangles.Count; m++)
            {
                if(triangles[n].isEque(tempTriangles[m]))
                {
                    tempTriangles.Remove(tempTriangles[m]);
                    m = -1;
                }
            }
        }
        for (int n = 0; n < tempTriangles.Count; n++)
        {
            triangles.Add(tempTriangles[n]);
        }

        //除去与超级三角形有关的三角形
        for (int n = 0; n < triangles.Count; n++)
        {
            if(triangles[n].isRelated(superTriangle))
            {
                triangles.Remove(triangles[n]);
                n = -1;
            }
        }


    }

    public void IsDrawLink(bool isDrawLink)
    {
        this.isDrawLink = isDrawLink;
    }

    public void Update()
    {
        if(isDrawLink)
        {
            DrawLink();
        }
    }
    /// <summary>
    /// 绘制调试信息
    /// </summary>
    private void DrawLink()
    {
        Vector3 sv30 = new Vector3(superTriangle.point[0].x, 0, superTriangle.point[0].y);
        Vector3 sv31 = new Vector3(superTriangle.point[1].x, 0, superTriangle.point[1].y);
        Vector3 sv32 = new Vector3(superTriangle.point[2].x, 0, superTriangle.point[2].y);

        Debug.DrawLine(sv30, sv31, Color.red);
        Debug.DrawLine(sv31, sv32, Color.red);
        Debug.DrawLine(sv32, sv30, Color.red);

        for (int i = 0; i < minGraph.Length; i++)
        {
            for (int j = 0; j < minGraph.Length; j++)
            {
                if(minGraph[i][j] != float.MaxValue)
                {
                    Vector3 v30 = new Vector3(v2[i].x, 0, v2[i].y);
                    Vector3 v31 = new Vector3(v2[j].x, 0, v2[j].y);

                    Debug.DrawLine(v30, v31, Color.yellow);
                }
            }
        }
    }
    /// <summary>
    /// 获取三角形外接圆
    /// </summary>
    /// <param name="triangles"></param>
    /// <param name="R"></param>
    /// <param name="center"></param>
    private void GetTriangleExcenterRadius(Triangles triangles, out float R, out Vector2 center)
    {
        Vector2 A, B, C;
        triangles.GetPoint(out A, out B, out C);
        if (A == B && A == C)
        {
            R = 0;
            center = A;
            return;
        }
        float x1 = A.x, x2 = B.x, x3 = C.x, y1 = A.y, y2 = B.y, y3 = C.y;
        float C1 = Mathf.Pow(x1, 2) + Mathf.Pow(y1, 2) - Mathf.Pow(x2, 2) - Mathf.Pow(y2, 2);
        float C2 = Mathf.Pow(x2, 2) + Mathf.Pow(y2, 2) - Mathf.Pow(x3, 2) - Mathf.Pow(y3, 2);
        float centery = (C1 * (x2 - x3) - C2 * (x1 - x2)) / (2 * (y1 - y2) * (x2 - x3) - 2 * (y2 - y3) * (x1 - x2));
        float centerx = (C1 - 2 * centery * (y1 - y2)) / (2 * (x1 - x2));
        center = new Vector2(centerx, centery);
        R = GetDistance(A, center);
    }
    /// <summary>
    /// 获取连点间距离
    /// </summary>
    /// <param name="A"></param>
    /// <param name="B"></param>
    /// <returns></returns>
    private float GetDistance(Vector2 A, Vector2 B)
    {
        return Mathf.Sqrt(Mathf.Pow((A.x - B.x), 2) + Mathf.Pow((A.y - B.y), 2));
    }
}
