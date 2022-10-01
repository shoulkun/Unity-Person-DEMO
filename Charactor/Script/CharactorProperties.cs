using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色属性
/// </summary>
public class CharactorProperties : MonoBehaviour
{
    public string playerName;   // 角色名称
    public GameObject player;   // 绑定的GameObject
    public CharacterPreset characterPreset; // 角色预设
    public float damage= 10;   // 伤害
    public float hitPush= 5;   // 击退能力
    public Dictionary<string, float> extraBulletsEffects;   // 子弹特效
    public int health;  // 生命值
    public float moveSpeed; // 移动速度
    public float fireInterval;  // 开火间隔
    public float fireRange; // 射程

    void Start()
    {
        hitPush = characterPreset.hitPush;
        damage = characterPreset.damage;
        health = characterPreset.health;
        moveSpeed = characterPreset.moveSpeed;
        fireInterval = characterPreset.fireInterval;
        fireRange = characterPreset.fireRange;
    }

    void Update()
    {
        
    }

    /// <summary>
    /// 角色初始化
    /// </summary>
    /// <param name="playerName"></param>
    public void init(string playerName)
    {
        player = GameObject.Find(playerName);
        this.playerName = playerName;
    }
}
