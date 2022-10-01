using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using System.Threading;
/// <summary>
/// 敌人AI
/// </summary>
public class EnemiesSimpleAI : MonoBehaviour
{
    [Tooltip("ID")]
    public int id;
    [Tooltip("视野范围")]
    public float lookAngle = 80f;
    [Tooltip("视野距离")]
    public float lookDistance = 10f;
    [Tooltip("转身速度")]
    public float rotatSpeed = 5f;
    [Tooltip("追踪目标")]
    public GameObject target;
    [Tooltip("目标当前帧位置")]
    public Vector3 targetPosition;
    [Tooltip("目标上一帧位置")]
    private Vector3 targetPositionBuffer;
    [Tooltip("围绕障碍检测线数目")]
    public int asideRayNumber = 16;
    [Tooltip("围绕障碍检测线距离")]
    private float asideRayDistant = 5f;
    [Tooltip("移动速度")]
    private float moveSpeed;


    // 下面参数属于射线检测
    /// <summary>
    /// 障碍检测线数组
    /// </summary>
    private Ray[] asideRayArray;
    /// <summary>
    /// 障碍检测线起始位置
    /// </summary>
    private Vector3 start;
    /// <summary>
    /// 障碍检测线末端位置
    /// </summary>
    private Vector3 end;
    /// <summary>
    /// hit信息
    /// </summary>
    private RaycastHit hit;
    /// <summary>
    /// 各线权重
    /// </summary>
    private float[] asideRayWeight;
    /// <summary>
    /// 距离追踪目标的方向
    /// </summary>
    private Vector3 targetDirection;
    /// <summary>
    /// 追踪目标的检测线末端
    /// </summary>
    private Vector3 targetLineEnd;
    // Follow 用
    /// <summary>
    /// 朝向追踪目标的旋转四元数
    /// </summary>
    private Quaternion LookToTargetRotation;
    /// <summary>
    /// 自身位置
    /// </summary>
    private Vector3 selfPos;
    /// <summary>
    /// 协助画视野范围的辅助变量
    /// </summary>
    private Vector3 auxiliaryVarV3;
    /// <summary>
    /// 朝向相关
    /// </summary>
    private Vector3 dir;
    /// <summary>
    /// 视野范围内，是否正在追踪
    /// </summary>
    private bool isActive;
    private PlayerManager playerManager;


    void Start()
    {
        moveSpeed = GetComponent<Enemies>().moveSpeed;
        asideRayArray = new Ray[asideRayNumber];
        asideRayWeight = new float[asideRayNumber];
        //isActive = false;
        playerManager = PlayerManager.Instance;

        InitRay();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeTarget();
        if(!isActive)
        {
            return;
        }
        RefreshRay();
        ComputeRay();
        Follow();
    }


    bool IsInView(Vector3 targetPos)
    {
        auxiliaryVarV3 = Vector3.zero;
        auxiliaryVarV3.y = lookAngle;
        Debug.DrawLine(transform.position, Quaternion.Euler(auxiliaryVarV3) * transform.forward * lookDistance);
        auxiliaryVarV3.y = -lookAngle;
        Debug.DrawLine(transform.position, Quaternion.Euler(auxiliaryVarV3) * transform.forward * lookDistance);

        // 自身位置
        selfPos = transform.position;

        // 原始版本
        if (!Physics.Linecast(transform.position, targetPos, out hit)) { return false; }
        if (hit.collider.tag == "Untagged")
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);
            return false;
        }

        if (Vector3.Distance(selfPos, targetPos) > lookDistance)
            return false;

        if (Vector3.Angle(targetPos - selfPos, transform.forward) < lookAngle)
            return true;

        return false;
    }

    /// <summary>
    /// 更换追踪目标
    /// </summary>
    void ChangeTarget()
    {
        target = playerManager.playerList[(int)UnityEngine.Random.value * playerManager.playerNumber];
        if (target == null)
        {
            return;
        }


        targetPosition = target.transform.position;
        if (IsInView(target.transform.position))
        {
            targetPosition = target.transform.position;
            targetPositionBuffer = targetPosition;
            isActive = true;
        }
        else
        {
            targetPosition = targetPositionBuffer;
        }
    }
    /// <summary>
    /// 更新碰撞检测射线
    /// </summary>
    public void RefreshRay()
    {
        for (int i = 0; i < asideRayNumber; i++)
        {
            //asideRayArrayPrefab[i] = new Ray(transform.position, Quaternion.Euler(new Vector3(0,angle,0)) * (Vector3.forward * asideRayDistant));
            asideRayArray[i].origin = transform.position;
        }
    }
    /// <summary>
    /// 初始化碰撞检测射线
    /// </summary>
    void InitRay()
    {
        float angle = 0;
        for (int i = 0; i < asideRayNumber; i++)
        {
            asideRayArray[i] = new Ray(transform.position, Quaternion.Euler(new Vector3(0, angle, 0)) * (Vector3.forward * asideRayDistant));
            //asideRayArray[i] = new Ray(transform.position, Quaternion.Euler(new Vector3(0,angle,0)) * (Vector3.forward * asideRayDistant));
            angle += 360 / asideRayNumber;
        }
    }
    /// <summary>
    /// 计算碰撞检测射线
    /// </summary>
    public void ComputeRay()
    {
        //Array.Clear(hit, 0, asideRayNumber);
        // // 目标方向
        targetDirection = (targetPosition - transform.position).normalized;
        targetDirection.y = 0;
        // 目标线终点
        targetLineEnd = transform.position + targetDirection * asideRayDistant;

        dir = Vector3.zero;

        //原始版本
        for (int i = 0; i < asideRayNumber; i++)
        {
            start = asideRayArray[i].origin;
            end = start + (asideRayArray[i].direction * asideRayDistant);
            // 点乘，根据被追踪位置和自身位置修改各检测线的权重
            asideRayWeight[i] = (Vector3.Dot((targetLineEnd - transform.position).normalized, (end - start).normalized) + 1) / 2;
            end = start + (asideRayArray[i].direction * asideRayDistant * asideRayWeight[i]);

            if (Physics.Linecast(start, end, out hit))
            {
                Debug.DrawLine(start, hit.point, Color.red);
                // 如果已碰撞， 则根据障碍物与自身位置修改检测线权重
                asideRayWeight[i] = (hit.point - start).magnitude / asideRayDistant;
            }
            else
            {
                Debug.DrawLine(start, end, Color.green);
            }

            dir += asideRayArray[i].direction * asideRayWeight[i];
        }
        // 绘制与被追踪目标连线
        Debug.DrawLine(transform.position, targetLineEnd);
    }

    /// <summary>
    /// 追踪
    /// </summary>
    void Follow()
    {
        // dir = Vector3.zero; 
        // for (int i = 0; i < asideRayNumber; i++)
        // {
        //     dir += asideRayArray[i].direction * asideRayWeight[i];
        // }
    
        // 绘制朝向
        Debug.DrawLine(transform.position, transform.position + dir.normalized * asideRayDistant, Color.blue);

        LookToTargetRotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
        //LookToTargetRotation = Quaternion.LookRotation(targetPosition - transform.position, Vector3.up);
        //Quaternion LookToTargetRotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
        LookToTargetRotation.x = 0;
        LookToTargetRotation.z = 0;
        // 更新自身旋转
        transform.rotation = Quaternion.Slerp(transform.rotation, LookToTargetRotation, rotatSpeed * Time.deltaTime);
        
        // if (Vector3.Angle(loorDir, transform.forward) < moveAngle)
        // {
        // 更新自身位置
        transform.position += dir.normalized * 0.1f * moveSpeed * Time.deltaTime;
        // }

        // if ((targetPosition - transform.position).magnitude < 0.1f)
        // {
        //     isActive = false;
        // }
    }
}
