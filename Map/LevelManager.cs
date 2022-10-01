using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 关卡管理器
/// </summary>
public class LevelManager : SingletonMonoBase<LevelManager>
{
    [Tooltip("地图种子")]
    public string seed = "";
    [Tooltip("最大房间块尺寸")]
    public int maxCellSize;
    [Tooltip("最小房间块尺寸")]
    public int minCellSize;
    [Tooltip("房间块大小方差")]
    public float cellSizeDispersion;
    [Tooltip("地图尺寸")]
    public int mapSize;
    [Tooltip("地图块数量")]
    public int cellNumber;
    [Tooltip("显示地图块调试信息")]
    public bool isDraw = true;
    [Tooltip("链接宽度")]
    public int linkWidth = 4;
    [Tooltip("地图块预设")]
    public GameObject cellGameObject;
    [Tooltip("链接预设")]
    public GameObject linkGameObject;
    [Tooltip("瓦片预设")]
    public GameObject tileGameObject;
    /// <summary>
    /// 局内地图种子，用于各种随机事件
    /// </summary>
    private string gameSeed = "";
    private int mapXSeed = 0;
    private int mapYSeed = 0;
    /// <summary>
    /// 房间块框架生成器
    /// </summary>
    private MapGenerator mapGenerator;
    /// <summary>
    /// 链接生成器
    /// </summary>
    private ConnectGenerator connectGenerator;
    /// <summary>
    /// 地图模型生成器
    /// </summary>
    private MapModel mapModel;
    void Start()
    {
        MapRandSeed();
        mapGenerator = gameObject.AddComponent(typeof(MapGenerator)) as MapGenerator;
        mapGenerator.SetSeed(mapXSeed, mapYSeed);
        mapGenerator.SetCellSize(maxCellSize, minCellSize);
        mapGenerator.SetCellGameObject(cellGameObject);
        mapGenerator.SetCellNumber(cellNumber);
        mapGenerator.SetMapSize(mapSize);
        mapGenerator.SetCellSizeDispersion(cellSizeDispersion);
        mapGenerator.IsDrawCell(isDraw);
        mapGenerator.InitMap();
        mapGenerator.Generate();

        connectGenerator = gameObject.AddComponent(typeof(ConnectGenerator)) as ConnectGenerator;
        connectGenerator.minCellSize = minCellSize;
        connectGenerator.SetCell(mapGenerator.getCells());
        connectGenerator.InitCells();
        connectGenerator.IsDrawLink(isDraw);

        mapModel = gameObject.AddComponent(typeof(MapModel)) as MapModel;
        mapModel.linkWidth = linkWidth;
        mapModel.setTileGameObject(tileGameObject);
        mapModel.setLinkGameObject(linkGameObject);
        mapModel.GenerateMapModel(mapGenerator.getCells());

        mapModel.setV2(connectGenerator.getV2());
        mapModel.GenerateConnectModel(connectGenerator.getMinGraph());
    }


    // Update is called once per frame
    void Update()
    {
        connectGenerator.Update();
    }
    public void MapRandSeed()
    {
        GameSeed.SetSeed(seed);
        GameSeed.computeOutResult();
        gameSeed = GameSeed.GetSeed();
        int.TryParse(gameSeed.Substring(0, 5), out mapXSeed);
        int.TryParse(gameSeed.Substring(6, 3), out mapYSeed);
    }
}
