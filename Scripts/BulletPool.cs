using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 子弹池
/// </summary>
public class BulletPool : SingletonMonoBase<BulletPool>
{
    /// <summary>
    /// 池内最大子弹数
    /// </summary>
    public int bulletMaxNumber;
    /// <summary>
    /// 子弹预设
    /// </summary>
    public GameObject pre_bullet;
    /// <summary>
    /// 子弹GO的列表
    /// </summary>
    private GameObject[] bullets;
    private int head;
    private int end;
    private Vector3 _poolPosition;
    

    //public static BulletPool _instance; // static关键字。 单例模式 ,必须挂载到unity对象上

    // public void Awake()
    // {
    //     _instance = this;// 确保单例模式在使用前已被初始化
    // }

    // Start is called before the first frame update
    void Start()
    {
        _poolPosition = Vector3.zero;
        _poolPosition.y = -100;
        InitBulletArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitBulletArray()
    {
        bullets = new GameObject[bulletMaxNumber];
        head = 0;
        end = bulletMaxNumber/2;
        for (int i = 0; i < bulletMaxNumber; i++)
        {
            bullets[i] = Instantiate(pre_bullet);
            bullets[i].name = "bullet"+i.ToString();
            bullets[i].transform.position = _poolPosition;
            //bullets[i].SetActive(false);
        }
    }

    public GameObject GetBullet()
    {
        head = (head+1) % bulletMaxNumber;
        end = (end+1) % bulletMaxNumber;
        // 解冻待发射子弹
        bullets[end].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        //bullets[end].SetActive(false);
        bullets[end].transform.position = _poolPosition;
        return bullets[head];
    }

}
