using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 局内角色管理器，单列
/// </summary>
public class PlayerManager : SingletonMonoBase<PlayerManager>
{
    public int playerNumber = 1;    // 玩家数量
    public int me = 0;  // 本机玩家控制的角色序列
    [Tooltip("可选的预设")]
    public List<CharacterPreset> characterPresetList;   // 可选预设
    [Tooltip("玩家控制器")]
    public GameObject playerControllerPrefab; // 本机玩家绑定的控制器预设
    [Tooltip("跟随相机")]
    public GameObject followCameraPrefab; // 本机玩家绑定的跟随相机预设
    //[HideInInspector]
    public GameObject followCamera; // 本机玩家绑定的跟随相机


    [Tooltip("各玩家选择预设")]
    private List<int> playerSelectPreset;   // 局内玩家预设
    [Tooltip("玩家列表")]
    private List<CharactorProperties> charactorPropertiesList;  // 玩家属性
    public Dictionary<string, CharactorProperties> charactorPropertiesMap;  // 玩家属性
    public List<GameObject> playerList; // 当局玩家列表

    // public static PlayerManager _instance; // static关键字。 单例模式 ,必须挂载到unity对象上

    new void Awake()
    {
        base.Awake();// 确保单例模式在使用前已被初始化

    }
    void Start()
    {
        playerSelectPreset = new List<int>();   // 初始化当局玩家的预设
        for (int i = 0; i < playerNumber; i++)
        {
            playerSelectPreset.Add(0);
        }

        playerList = new List<GameObject>();
        charactorPropertiesList = new List<CharactorProperties>();
        charactorPropertiesMap = new Dictionary<string, CharactorProperties>();

        initPlayer();   // 生成角色
        
        followCamera = Instantiate(followCameraPrefab);   // 初始化跟随相机
        followCamera.GetComponent<CameraFollow>().target = playerList[me].transform;    // 绑定跟随相机
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /// <summary>
    /// 初始化局内玩家
    /// </summary>
    void initPlayer()
    {
        int x_offset = 0;   // 出生点偏移
        int step = 2;   // 步长
        for (int i = 0; i < playerNumber; i++)
        {
            GameObject player = Instantiate(playerControllerPrefab);  // 初始化角色
            player.name = "Player "+i.ToString();   // 初始化玩家名称

            playerList.Add(player); // 添加进当局玩家列表

            // 移动每个玩家
            Vector3 pos = player.transform.position;
            pos.x = x_offset;
            player.transform.position = pos;

            x_offset += step;   // 设置偏移量

            // 在manager内生成每个玩家属性
            CharactorProperties charactorProperties = gameObject.AddComponent<CharactorProperties>();
            charactorProperties.init(player.name);
            // 读取角色属性预设
            charactorProperties.characterPreset = characterPresetList[playerSelectPreset[i]];

            charactorPropertiesList.Add(charactorProperties);
            charactorPropertiesMap.Add(player.name, charactorProperties);
        }
    }
}
