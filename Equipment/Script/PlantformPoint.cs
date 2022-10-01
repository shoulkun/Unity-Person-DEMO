using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 底座碰撞点
/// </summary>
public class PlantformPoint : MonoBehaviour
{
    [Tooltip("回弹速度")]
    [Range(0f, 30f)]
    public float reboundSpeed = 5f;
    [Tooltip("力清零速度")]
    [Range(0f, 10f)]
    public float zeroingSpeed = 2f;
    private Rigidbody rig;
    private float yOffset;
    // Start is called before the first frame update
    void Start()
    {
        yOffset = transform.localPosition.y;
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        RecoveryPosition();
    }
    void RecoveryPosition()
    {
        rig.velocity = Vector3.Lerp(rig.velocity, Vector3.zero, zeroingSpeed*Time.deltaTime);
        transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0, yOffset, 0), reboundSpeed * Time.deltaTime);
    }
}
