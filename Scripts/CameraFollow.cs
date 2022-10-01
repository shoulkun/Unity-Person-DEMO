using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //宝宝跟随的目标
    public Transform target;
    //宝宝跟随目标的偏移量
    public Vector3 offset;
    public float speed;


    // Use this for initialization
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
    // Debug.Log(Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, 0, 10), Time.deltaTime));
    }
    void LateUpdate()
    {
        //offset = target.forward * -2 + target.up * 2;
        if(target != null)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, target.position.y/4, target.position.z) + offset, speed * Time.deltaTime);
        }
    }
}