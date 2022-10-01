using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 房间块内摄像机切换
/// </summary>
public class CellCameraSwitch : MonoBehaviour
{
    [Tooltip("绑定跟随相机")]
    public GameObject followCamera;
    [Tooltip("跟随相机的跟随点位")]
    public GameObject followPoint;
    [Tooltip("跟随相机脚本")]
    private CameraFollow cameraFollow;
    [Tooltip("纵向限制偏移")]
    public int YLimitOffset;
    [Tooltip("跟随相机移动限制")]
    public float CameraLimit;

    
    private float XLimit;
    private float YLimit;

    public void setCameraLimit(float xLimit, float yLimit)
    {
        xLimit -= CameraLimit;
        yLimit -= CameraLimit;

        if(xLimit < 0)
        {
            xLimit = 0;
        }
        if(yLimit < 0)
        {
            yLimit = 0;
        }
        this.XLimit = xLimit;
        this.YLimit = yLimit;
    }

    private bool isInThisCell = false;
    private Vector3 followPointPos;
    private Vector3 followPointlocalPos;
    // Start is called before the first frame update
    void Start()
    {
        followCamera = PlayerManager.Instance.followCamera;
        cameraFollow = followCamera.GetComponent<CameraFollow>() as CameraFollow;
    }
    /// <summary>
    /// 绘制摄像机移动限制范围
    /// </summary>
    void DrawLimit()
    {
        Debug.DrawLine(new Vector3(-XLimit + transform.position.x, 0, YLimit + transform.position.z - YLimitOffset), new Vector3(XLimit + transform.position.x, 0, YLimit + transform.position.z - YLimitOffset), Color.red);
        Debug.DrawLine(new Vector3(XLimit + transform.position.x, 0, YLimit + transform.position.z - YLimitOffset), new Vector3(XLimit + transform.position.x, 0, -YLimit + transform.position.z - YLimitOffset), Color.red);
        Debug.DrawLine(new Vector3(XLimit + transform.position.x, 0, -YLimit + transform.position.z - YLimitOffset), new Vector3(-XLimit + transform.position.x, 0, -YLimit + transform.position.z - YLimitOffset), Color.red);
        Debug.DrawLine(new Vector3(-XLimit + transform.position.x, 0, -YLimit + transform.position.z - YLimitOffset), new Vector3(-XLimit + transform.position.x, 0, YLimit + transform.position.z - YLimitOffset), Color.red);
    }
    // Update is called once per frame
    void Update()
    {
        DrawLimit();

        // 检测到在当前cell
        if(isInThisCell)
        {
            // 更新跟随点位
            followPointPos = PlayerManager.Instance.playerList[PlayerManager.Instance.me].transform.position;
            followPoint.transform.position = followPointPos;


            // 更新本地位置（限制用）
            followPointlocalPos = followPoint.transform.localPosition;
            // x移动限制
            if(followPoint.transform.localPosition.x > XLimit)
            {
                 followPoint.transform.localPosition = new Vector3(XLimit, followPointlocalPos.y, followPointlocalPos.z);
            }
            else if(followPoint.transform.localPosition.x < -XLimit)
            {
                 followPoint.transform.localPosition = new Vector3(-XLimit, followPointlocalPos.y, followPointlocalPos.z);
            }
            followPointlocalPos = followPoint.transform.localPosition;
            // y移动限制
            if(followPoint.transform.localPosition.z > YLimit - YLimitOffset)
            {
                 followPoint.transform.localPosition = new Vector3(followPointlocalPos.x, followPointlocalPos.y, YLimit - YLimitOffset);
            }
            else if(followPoint.transform.localPosition.z < -YLimit - YLimitOffset)
            {
                 followPoint.transform.localPosition = new Vector3(followPointlocalPos.x, followPointlocalPos.y, -YLimit - YLimitOffset);

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player "+PlayerManager.Instance.me.ToString())
        {
            Debug.Log("in cell");
            cameraFollow.target = followPoint.transform;
            isInThisCell = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.name == "Player "+PlayerManager.Instance.me.ToString())
        {
            Debug.Log("out cell");
            cameraFollow.target = PlayerManager.Instance.playerList[PlayerManager.Instance.me].transform;
            isInThisCell = false;
        }
    }
}
