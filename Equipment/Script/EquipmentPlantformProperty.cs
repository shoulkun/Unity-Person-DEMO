using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 装备平台
/// </summary>
public class EquipmentPlantformProperty : MonoBehaviour
{
    [Tooltip("底座位置")]
    public Transform placePosition;
    [Tooltip("陈放物品")]
    public GameObject placing;
    [Tooltip("陈放物品旋转速度")]
    public float placingRotateSpeed;
    [Tooltip("陈放物品位置偏移")]
    public Vector3 placingPositionOffset;

    private GameObject _placing;
    
    // Start is called before the first frame update
    void Start()
    {
        _placing = Instantiate(placing);
        _placing.transform.parent = placePosition.transform;
        _placing.transform.localPosition  = placingPositionOffset;
    }

    // Update is called once per frame
    void Update()
    {
        AutoRotate();
    }

    /// <summary>
    /// 陈放物品自动旋转
    /// </summary>
    void AutoRotate()
    {
        placePosition.Rotate(Vector3.up * placingRotateSpeed * Time.deltaTime, Space.Self);
    }

    // OnTriggerEnter 不带碰撞的 勾选is Trigger
    // OnCollisionEnterd 带碰撞的，不勾选 is Trigger
    void OnTriggerEnter(Collider other) {
        
        // 输出被碰撞到该 Collider 的所在的 GameObject名字
        if (other.gameObject.tag == "Player")
        {
            Destroy(_placing);
        }
    }
}
