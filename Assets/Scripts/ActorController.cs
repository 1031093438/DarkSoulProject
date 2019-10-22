using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {
    private KeyboardInput pi;
    private Rigidbody rb;
    private Animator anim;
    public GameObject model;

    public float moveSpeed = 1.0f;
    public float jumpForce = 1.0f;
    public float rollForce = 1.0f;
    public float jabFroce = 1.0f;

    public Vector3 planarVec;  //平面向量
    public Vector3 thrustVec;  //冲量向量


    public bool lockPlanar;
    

    void Awake () {
        pi = GetComponent<KeyboardInput> ();
        rb = GetComponent<Rigidbody> ();
        anim = model.GetComponent<Animator> ();
    }

    void Update () {
        
        //Setting Model Turn and Forward
        anim.SetFloat ("forward", Mathf.Lerp(anim.GetFloat("forward"), pi.Dmagnitude * (pi.run ? 2.0f : 1.0f), 0.3f));
        if (pi.Dmagnitude > 0.1f){
            model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.5f);
        }

        //Setting Action Lock
        if(lockPlanar == false)
        {
            planarVec = pi.Dmagnitude * model.transform.forward * moveSpeed * (pi.run ? 2.0f : 1.0f);
        }


        //Jump Setting
        if (pi.jump)
        {
            anim.SetTrigger("jump");
            
        }

        //Roll Setting
        if (rb.velocity.magnitude > 5.0f || pi.roll)
        {
            anim.SetTrigger("jump");
        }


    }

    void FixedUpdate () {
        rb.velocity = new Vector3(planarVec.x, rb.velocity.y, planarVec.z) + (thrustVec);
        thrustVec = Vector3.zero;
    }

    //On Enter Signal
    void OnJumpEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
        thrustVec = new Vector3(0, jumpForce, 0);
        
    }
    void OnGroundEnter()
    {
        pi.inputEnabled = true;
        lockPlanar = false;
    }

    void OnFallEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;
    }
    void OnRollEnter()
    {
        pi.inputEnabled = false;
        lockPlanar = true;   
    }
    void OnJabEnter()
    {
        pi.inputEnabled = false;
    }
    //On Exit Signal

    //On Update Signal
    void OnRollUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("rollVelocity") * rollForce;
    }
    void OnJabUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("jabVelocity") * jabFroce;
    }

    //Other Signal
    void IsGround()
    {
        
        anim.SetBool("isGround", true);
    }

    void IsNotGround()
    {
        
        anim.SetBool("isGround", false);
    }

}