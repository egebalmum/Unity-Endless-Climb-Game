using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    //Cache Components
    public bool freeze;
    [SerializeField] public GameObject[] splats;
    Collider2D lastCollision;
    Collider2D[] hits = new Collider2D[2];
    [System.NonSerialized] public Rigidbody2D _rigidbody;
    Character _character;
    public LayerMask platformMask;
    public LayerMask wallMask;
    public LayerMask touchableMask;
    public LayerMask deathMask;
    public bool boostBool;
    public bool boostControl;
    //Gravity Attributes
    bool isGravityOn;
    Vector3 gravityDirection;
    float gravityAcceleration;
    public float characterMaxSpeed=12f;
    //Character Move Attributes
    Vector3 gravityVelocityVector;
    Vector3 inputVelocityVector;
    public Vector3 totalVelocityVector;
    public Vector3 savedTotalVelocityVector;
     public Collider2D _collider;
    public Vector3 savedContactNormal;
    public bool sticky;
    public Collision2D contacted;
    public GameObject lastContactedObject;
    public bool ignoreCollision;
    void Awake()
    {
        Initialiation();
    }

    void Initialiation(){
        _rigidbody = GetComponent<Rigidbody2D>();
        _character = GetComponent<Character>();
        _collider = GetComponent<Collider2D>();
    }
    void Start()
    {
        _character._lastFrameState = _character._movementState;
    }
    
    void FixedUpdate()
    {
        AddGravity();
        MoveObject();
        if (!(!boostBool && boostControl))
        {
            CastCircle();
        }
        CastWall();
    }

    private void CastCircle()
    {
        int platformCount = Physics2D.OverlapCircleNonAlloc(transform.position,0.3f,hits,platformMask);
        if (platformCount==1)
        {
            if (lastCollision!=null)
            {
                if (lastCollision != hits[0])
                {
                    ReloadCollider(hits[0]);
                    lastCollision = hits[0];
                }
            }
            else
            {
                ReloadCollider(hits[0]);
                lastCollision = hits[0];
            }
            
        }
        else if (platformCount==0)
        {
            lastCollision = null;
        }
    
    }

    private void CastWall()
    {
        int platformCount = Physics2D.OverlapCircleNonAlloc(transform.position,3f,hits,wallMask);
        if (platformCount==1)
        {
            if (lastCollision!=null)
            {
                if (lastCollision != hits[0])
                {
                    ReloadCollider(hits[0]);
                    lastCollision = hits[0];
                }
            }
            else
            {
                ReloadCollider(hits[0]);
                lastCollision = hits[0];
            }
            
        }
        else if (platformCount==0)
        {
            lastCollision = null;
        }
        
    }


    void ReloadCollider(Collider2D other)
    {
        other.enabled=false;
        other.enabled=true;
    }

    public void CollisionEnterBlock(Collision2D other)   //WHOLE COLLISION EFFECT
    {
        contacted = other;
        transform.SetParent(other.transform);
        if (totalVelocityVector.magnitude   >= characterMaxSpeed)
        {
            savedTotalVelocityVector = totalVelocityVector.normalized*characterMaxSpeed;
        }
        else
        {
            savedTotalVelocityVector = totalVelocityVector;
        }
        totalVelocityVector = Vector3.zero;
        gravityVelocityVector = Vector3.zero;
        inputVelocityVector = Vector3.zero;
        savedContactNormal = contacted.GetContact(0).normal;
        var theAngle = Vector3.Angle(Vector3.up,savedContactNormal); //neww
        theAngle = Mathf.Abs(theAngle); //new
        if (theAngle<=45)   //savedContactNormal.normalized ==Vector3.up
        {
            sticky=true;
        }
        else
        {
            sticky=false;
        }
        _character.SetMovementState(Character.MovementState.stuck);
    }


    public void ExitContact()
    {
        if (deathMask==(deathMask | (1<<contacted.gameObject.layer)))
        {
            _character.OnDeath();
        }
        else if(touchableMask==(touchableMask | (1<<contacted.gameObject.layer)))
        {   //stuck:6 , reflector:7
            if (contacted.gameObject.layer ==6 && sticky==true)
            {
                GetReady();
                
            }
            else
            {   
                JumpBack();
                
            }
        }
    }
    private void GetReady(){
        _character.SetMovementState(Character.MovementState.ready);
    }
    private void JumpBack(){
        transform.parent=null;
        totalVelocityVector = Vector3.Reflect(savedTotalVelocityVector, savedContactNormal);
        _character.SetMovementState(Character.MovementState.midAir);
    }
   
    void MoveObject()
    {
        totalVelocityVector += inputVelocityVector + gravityVelocityVector;
        if (totalVelocityVector.magnitude>=characterMaxSpeed && !boostControl)
        {
            totalVelocityVector = totalVelocityVector.normalized*characterMaxSpeed;
        }
        _rigidbody.transform.position += totalVelocityVector*Time.fixedDeltaTime;
    }

    public void UpdateGravityState(bool state){
       isGravityOn =state;
    }
    void AddGravity()
    {   if (_character.GetMovementState() == Character.MovementState.midAir){
        if (isGravityOn)
        {
            gravityVelocityVector = gravityDirection*gravityAcceleration*Time.fixedDeltaTime;
        }
        else
        {
            gravityVelocityVector=Vector3.zero;
        }
        }
    }
    public void UpdateGravity(Vector3 direction, float acceleration)
    {
        gravityDirection = direction.normalized;
        gravityAcceleration = acceleration;
    }
}
