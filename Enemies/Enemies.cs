using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人基本属性
/// </summary>
public class Enemies : MonoBehaviour
{
    [Tooltip("生命值")]
    public float health = 100;
    [Tooltip("碰撞伤害")]
    public float hitDamage = 10;
    [Tooltip("移动速度")]
    public float moveSpeed = 20;
    private Rigidbody rig;
    /// <summary>
    /// 受击后退中间参数
    /// </summary>
    private Vector3 onAttack;
    // Start is called before the first frame update
    public void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    public void Update()
    {
        if(onAttack != Vector3.zero)
        {
            transform.position += onAttack;
            onAttack = Vector3.Lerp(onAttack, Vector3.zero, 5*Time.deltaTime);
        }
        CheckDeath();
    }

    void CheckDeath()
    {
        if(health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 受击
    /// </summary>
    /// <param name="hitDiraction">受力方向</param>
    /// <param name="damage">受到伤害</param>
    /// <param name="hitPush">推动能力</param>
    public void OnAttack(Vector3 hitDiraction, float damage, float hitPush)
    {
        hitDiraction.y = 0;
        Debug.DrawLine(transform.position, hitDiraction + transform.position, Color.black, 2, false);
        onAttack = hitDiraction * hitPush * 0.1f;
        health -= damage;
    }   
}
