using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class ImpactReceiver : MonoBehaviour
{
    public float mass = 3.0F; // defines the character mass
    private Vector3 impact = Vector3.zero;
    private CharacterController character;
    private ThirdPersonController thirdPersonController;

    // Use this for initialization
    void Start () {
        character = GetComponent<CharacterController>();
        thirdPersonController = GetComponent<ThirdPersonController>();
    }
    
    // Update is called once per frame
    void Update () 
    {
        // apply the impact force:
        if (impact.magnitude > 0.2F) character.Move(impact * Time.deltaTime);
            // consumes the impact energy each cycle:
            impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
    }
    // call this function to add an impact force:
    public void AddImpact(Vector3 dir, float force)
    {
        dir.Normalize();
        if (dir.y < 0) dir.y = -dir.y; // reflect down force on the ground
        impact += dir.normalized * force / mass;
    }


    private GameObject hitGameObject;
    private float hitAngle;
    private Quaternion q;
    private Vector3 exclude;
    private Vector3 dir = new Vector3(0,0,1);

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag != "Enemies")
        {
            return;
        }

        hitGameObject = hit.gameObject;
        Debug.Log(Time.time + ":进入该触发器的对象是：" + hitGameObject.name);

        hitAngle = hitGameObject.GetComponent<Enemies>().hitDamage * 0.1f;
        if(hitAngle < 30) hitAngle = 0;
        if(hitAngle > 45) hitAngle = 45;

        q = Quaternion.AngleAxis(hitAngle, dir);// 旋转
        exclude = hit.transform.position - transform.position;
        exclude.y = 0;
        thirdPersonController.OnAttack();
        AddImpact(q * -exclude.normalized, 15f);
    }
    
}
