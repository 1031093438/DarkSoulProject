using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour {
    private KeyboardInput pi;
    private Rigidbody rb;
    private Animator anim;
    public GameObject model;
    private CapsuleCollider col;
    public CameraController camcon;

    [Header("===== Friciton Settings =====")]
    public PhysicMaterial frictionOne;
    public PhysicMaterial frictionZero;

    public float moveSpeed = 1.0f;
    public float jumpForce = 1.0f;
    public float rollForce = 1.0f;
    public float jabFroce = 1.0f;
    public float targetLerp;


    public Vector3 planarVec;  //平面向量
    public Vector3 thrustVec;  //冲量向量
    private Vector3 deltaPos;


    public bool lockPlanar;
    private bool canAttack;
    
    

    void Awake () {
        pi = GetComponent<KeyboardInput> ();
        rb = GetComponent<Rigidbody> ();
        anim = model.GetComponent<Animator> ();
        col = GetComponent<CapsuleCollider> ();
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
        if (rb.velocity.magnitude > 9.0f || pi.roll)
        {
            anim.SetTrigger("jump");
        }

        //Attack Setting
        if (pi.attack && CheckState("Ground") && canAttack)
        {
            anim.SetTrigger("attack");
        }

        //Defense Setting
        if (pi.defense)
        {
            anim.SetBool("defense", true);
        }
        else
        {
            anim.SetBool("defense", false);
        }

        //LockOn Setting
        if (pi.lockOn)
        {
            camcon.LockUnlock();
        }
    }

    void FixedUpdate () {
        rb.position += deltaPos;
        rb.velocity = new Vector3(planarVec.x, rb.velocity.y, planarVec.z) + (thrustVec);
        thrustVec = Vector3.zero;
        deltaPos = Vector3.zero;
        
    }

    //On Enter Signal
    void OnJumpEnter()
    {
        LockUnLockInput(false, true);
        thrustVec = new Vector3(0, jumpForce, 0);
        canAttack = false;
    }
    void OnGroundEnter()
    {
        LockUnLockInput(true, false);
        canAttack = true;
        col.material = frictionOne;
    }

    void OnFallEnter()
    {
        LockUnLockInput(false, true);
    }
    void OnRollEnter()
    {
        LockUnLockInput(false, true);
        canAttack = false;
    }
    void OnJabEnter()
    {
        pi.inputEnabled = false;
    }
    void OnAttack1hAEnter()
    {
        targetLerp = 1;
        LockUnLockInput(false, false);
    }
    void OnAttackIdleEnter()
    {
        targetLerp = 0;
        LockUnLockInput(true, false);
        
    }
    //On Exit Signal
    void OnGroundExit()
    {
        col.material = frictionZero;
    }
    

    //On Update Signal
    void OnRollUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("rollVelocity") * rollForce;
    }
    void OnJabUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("jabVelocity") * jabFroce;
    }
    void OnAttack1hAUpdate()
    {
        anim.SetLayerWeight(anim.GetLayerIndex("Attack"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("Attack")), targetLerp, 0.1f));
        thrustVec = model.transform.forward * anim.GetFloat("attack1hAVelocity") * 0.5f;
    }
    void OnAttackIdleUpdate()
    {
        anim.SetLayerWeight(anim.GetLayerIndex("Attack"), Mathf.Lerp(anim.GetLayerWeight(anim.GetLayerIndex("Attack")), targetLerp, 0.1f));
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
    void LockUnLockInput(bool _inputEnabled, bool _lockPlanar)
    {
        pi.inputEnabled = _inputEnabled;
        lockPlanar = _lockPlanar;
    }

    private bool CheckState(string stateName, string layerName = "Base Layer")
    {
        int layerIndex = anim.GetLayerIndex(layerName);
        bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);
        return result;
    }

    public void OnUpdateRM(object _deltaPos)
    {
        if (CheckState("attack1hC", "Attack"))
        {
            deltaPos += (Vector3)_deltaPos;
        }
    }
}