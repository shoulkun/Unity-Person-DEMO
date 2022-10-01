using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControlor : MonoBehaviour
{

    private Transform _bulletSelfTransform;
    private Rigidbody _bulletSelfRigidbody;

    private Vector3 _firePosition;
    private Vector3 _fireDirection;
    private float _bulletSpeed;
    private string _playerName;

    private Vector3 _poolPosition;
    private TimerManager timerManager;
    // Start is called before the first frame update
    void Start()
    {
        _poolPosition = Vector3.zero;
        _poolPosition.y = -100;
        _bulletSelfTransform = GetComponent<Transform>();
        _bulletSelfRigidbody = GetComponent<Rigidbody>();
        timerManager = TimerManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFireState(string name,Vector3 pos, Vector3 dir, float speed)
    {
        _playerName = name;
        _firePosition = pos;
        _fireDirection = dir;
        _bulletSpeed = speed;
    }

    public void Fire()
    {
        _bulletSelfTransform.position = _firePosition;
        _bulletSelfRigidbody.velocity = _fireDirection * _bulletSpeed;
        timerManager.AddTimer
        (
            gameObject.name,
            new Timer
            (
                1,
                null,
                ()=>
                {
                    _bulletSelfTransform.position = _poolPosition;
                    FreezeBullet();
                    timerManager.RemoveTimer(gameObject.name);
                },
                null
            )
        );
    }

    public void FreezeBullet()
    {
        _bulletSelfRigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void UnfreezeBullet()
    {
        _bulletSelfRigidbody.constraints = ~RigidbodyConstraints.FreezeAll;
    }


    
    
    // OnTriggerEnter 不带碰撞的 勾选is Trigger
    // OnCollisionEnterd 带碰撞的，不勾选 is Trigger
    void OnTriggerEnter(Collider other) {
        
        // 输出被碰撞到该 Collider 的所在的 GameObject名字
        Debug.Log("hit :"+other.gameObject.name);
        if(other.tag == "MapCell"){return;}
        if(other.tag == "Enemies")
        {
            CharactorProperties player = PlayerManager.Instance.charactorPropertiesMap[_playerName];
            Enemies enemies = other.gameObject.GetComponent<Enemies>();
            enemies.OnAttack
            (
                ((other.transform.position - transform.position).normalized + _fireDirection.normalized * 4).normalized, 
                player.damage, 
                player.hitPush
            );   
        }

        _bulletSelfTransform.position = _poolPosition;
        FreezeBullet();
    }
}
