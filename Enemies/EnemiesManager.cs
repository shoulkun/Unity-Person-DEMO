using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using System;
/// <summary>
/// 局内敌人管理器
/// </summary>
public class EnemiesManager : SingletonMonoBase<EnemiesManager>
{
    [Tooltip("难度计时器(秒)")]
    public Timer difficultyTimer;   //难度计时器(秒)
    [Tooltip("最大难度时间(分钟)")]
    public float MaxDifficultyTime = 30f;    //最大难度时间(分钟)
    [Tooltip("各难度层级敌人数目")]
    public int[] difficultyMaxEnemiesNumber;    // 各难度的最大地人数
    [Tooltip("难度时刻表")]
    public int[] difficultyChangeTime;          // 难度对应时刻表(秒)

    [Tooltip("敌人预设")]
    public GameObject EnemiesPrefab;
    [Tooltip("敌人数目")]
    public int EnemiesNumber = 10;
    [Tooltip("敌人列表")]
    public List<GameObject> EnemiesLists;
    [Tooltip("敌人AI列表")]
    public Dictionary<int, EnemiesSimpleAI> EnemiesSimpleAIList;
    [Tooltip("碰撞检测线数目")]
    public int asideRayNumber = 16;
    [Tooltip("测试用：出生位置偏移")]
    private int offset = -5;
    [Tooltip("当前难度最大敌人数")]
    private int nowMaxEnemies;      // 当前难度最大敌人数
    [Tooltip("当前时间(秒)")]
    public float difficultyTime;     // 当前时间(秒)

    new void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        /// <summary>
        /// 难度计时器
        /// </summary>
        /// <param name="gameObject.name">计时器名称</param>
        /// <param name="MaxDifficultyTime">最大时间</param>
        /// <param name="null">updateAction</param>
        /// <param name="()">callAction</param>
        /// <returns></returns>
        difficultyTimer = TimerManager.Instance.AddTimer
                            (
                                gameObject.name,
                                new Timer
                                (
                                    MaxDifficultyTime,
                                    null,
                                    ()=>
                                    {
                                        TimerManager.Instance.RemoveTimer(gameObject.name);
                                        difficultyTime = MaxDifficultyTime; // 计时结束时，难度计时停留在最大难度
                                    },
                                    null    ///intiAction
                                )
                            );
        EnemiesLists = new List<GameObject>();
        EnemiesSimpleAIList = new Dictionary<int, EnemiesSimpleAI>();
        InitEnemies();
    }

    /// <summary>
    /// 测试：初始化敌人
    /// </summary>
    void InitEnemies()
    {
        EnemiesSimpleAI temp;
        
        for (int i = 0; i < EnemiesNumber; i++)
        {
            EnemiesLists.Add(Instantiate(EnemiesPrefab));   // 初始化敌人，并加入当局敌人列表
            EnemiesLists[i].transform.position = new Vector3(offset + i*2, 2, 0);   // 初始化敌人位置

            temp = EnemiesLists[i].GetComponent<EnemiesSimpleAI>() as EnemiesSimpleAI;  // 获取AI
            temp.id = i;
            temp.asideRayNumber = asideRayNumber;   // 设置碰撞检测线
            EnemiesSimpleAIList.Add(temp.id, temp); // 加入AI列表
        }
    }

    /// <summary>
    /// 难度修改检测
    /// </summary>
    void CheckDifficulty()
    {
        for (int i = 0; i < difficultyChangeTime.Length; i++)
        {
            nowMaxEnemies = difficultyMaxEnemiesNumber[i];
            if(difficultyTime < difficultyChangeTime[i])
            {  
                break;
            }
        }
    }


    void Update()
    {
        difficultyTime = (MaxDifficultyTime - difficultyTimer.LeftTime);
        CheckDifficulty();

        // ProcessRaycastCommand();
        // PullBackRaycastCommand();
        // InitCommand();
    }
    // Update is called once per frame
    void LastUpdate()
    {

    }
}
