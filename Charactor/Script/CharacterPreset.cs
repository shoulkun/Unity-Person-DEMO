using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Data",menuName ="CharacterManager/CharacterPreset")]

/// <summary>角色预设信息</summary>
public class CharacterPreset : ScriptableObject
{
    [Header("properties Info")]
    public float hitPush;
    public float damage;
    public int health;
    public float moveSpeed;
    public float fireInterval;
    public float fireRange;
}
