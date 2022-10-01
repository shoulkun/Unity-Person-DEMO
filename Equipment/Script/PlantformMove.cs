using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 平台被碰撞移动
/// </summary>
public class PlantformMove : MonoBehaviour
{
    [Tooltip("来回频率")]
    [Range(0f, 10f)]
    public float frequency = 2f;
    [Tooltip("回弹阻尼, 1时不能收敛")]
    [Range(0f, 1f)]
    public float damping = 0.5f;
    [Tooltip("初始响应, 负时会先朝反向蓄力. 大于1时会超过目标")]
    [Range(-5f, 5f)]
    public float response = 0.5f;

    [Tooltip("静止位置")]
    public Transform point;

    private Rigidbody rig;
    private Vector3 pointBuffer;
    private float initPositionLong;
    private Vector3 initPosition;
    private SceondOrderDynamics secondOrderDynamics;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        initPosition = transform.localPosition;
        initPositionLong = initPosition.sqrMagnitude;

        secondOrderDynamics = gameObject.AddComponent<SceondOrderDynamics>();   //二阶动力学方法
        secondOrderDynamics.init(frequency, damping, response, transform.localPosition);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.localPosition = secondOrderDynamics.ComputeSOD(speed * Time.deltaTime, test.localPosition, Vector3.zero);
        //PushCheck();
        RecoveryPosition();
        secondOrderDynamics.changePropties(frequency, damping, response);
        pointBuffer = point.transform.localPosition;
    }
    // void PushCheck()
    // {
    //     if (rig.velocity.sqrMagnitude < new Vector3(0.1f, 0.1f, 0.1f).sqrMagnitude)
    //     {
    //         isPush=false;
    //         return;
    //     }
    //     isPush=true;
    //     rig.velocity = Vector3.Lerp(rig.velocity, Vector3.zero, speed * Time.deltaTime);
    //     point.localPosition = transform.localPosition;
    // }

    /// <summary>
    /// 做往复运动
    /// </summary>
    void RecoveryPosition()
    {
        transform.localPosition = secondOrderDynamics.ComputeSOD(Time.deltaTime, point.localPosition, (point.transform.localPosition - pointBuffer)/Time.deltaTime);
    }
    // OnTriggerEnter 不带碰撞的 勾选is Trigger
    // OnCollisionEnterd 带碰撞的，不勾选 is Trigger
    void OnTriggerEnter(Collider other) {
        
        // 输出被碰撞到该 Collider 的所在的 GameObject名字
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("player");
            Vector3 exclude = other.transform.position - transform.position;    // 排斥方向
            Vector3 playerDir = other.transform.forward;    // 拾取玩家朝向
            exclude.y = 0f;
            other.GetComponent<ImpactReceiver>().AddImpact((exclude.normalized + playerDir.normalized).normalized, 5f); // 给玩家排斥力
        }
    }
}
