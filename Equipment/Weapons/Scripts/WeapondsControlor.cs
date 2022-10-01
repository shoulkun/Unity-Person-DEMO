using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
    [RequireComponent(typeof(PlayerInput))]
#endif

    public class WeapondsControlor : MonoBehaviour
    {
    #if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
        private PlayerInput _playerInput;
    #endif
        private StarterAssetsInputs _input;

        [Tooltip("对象池")]
        public BulletPool BulletPoolManager;
        [Tooltip("武器位置")]
        public Transform weapondsPos;   //武器位置
        [Header("攻击属性")]
        public float cd;
        public float damage;
        public float range;
        public Collider fire_collider;
        public List<string> ownedWeapons;
        
        private GameObject bullet;
        private bool isFired;
        private float realityIntervalFiredTime; // 当前冷却时间
        private float intervalFiredTime;    // 冷却


        // Start is called before the first frame update
        void Start()
        {
            BulletPoolManager = BulletPool.Instance;
            _input = GetComponent<StarterAssetsInputs>();
    #if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
            _playerInput = GetComponent<PlayerInput>();
    #else
            Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
    #endif

            intervalFiredTime = cd;
            realityIntervalFiredTime = intervalFiredTime;
        }

        // Update is called once per frame
        void Update()
        {
            Fire();
            //Debug.Log(realityIntervalFiredTime);
        }
        void FixedUpdate() 
        {
            FireCoolingDown();
        }
        void FireCoolingDown()
        {
            if(realityIntervalFiredTime < cd)
            {
                realityIntervalFiredTime+=1;
            }
        }
        void Fire()
        {
            if(_input.attack != new Vector2(0, 0) && realityIntervalFiredTime >= cd)
            {
                BulletControlor bullet = BulletPoolManager.GetBullet().GetComponent<BulletControlor>();
                //Rigidbody rig = bullet.GetComponent<Rigidbody>();
                //bullet.transform.position = weapondsPos.position;
                //bullet.SetActive(true);
                //rig.constraints = ~RigidbodyConstraints.FreezeAll;
                //rig.AddForce(new Vector3(_input.attack.x, 0, _input.attack.y) * 10, ForceMode.Impulse);

                bullet.UnfreezeBullet();
                bullet.SetFireState
                (
                    gameObject.name,
                    weapondsPos.position,
                    new Vector3(_input.attack.x, 0, _input.attack.y),
                    10
                );
                bullet.Fire();

                realityIntervalFiredTime = 0;
            }
        }
    }
}

