using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MoveAbility : CharacterAbility
{
    [SerializeField] SpriteRenderer circle1;
    [SerializeField] SpriteRenderer circle2;
    [SerializeField] SpriteRenderer circle3;
    [SerializeField] float boostForce;
    public Transform _rotatable;
    float maximumDistance = 0.5f;
    public float minDrag;
    public float maxDrag;
    public float power;
    public float currentDrag; //going to delete
    Camera cam;
    Vector3 startPoint;
    Vector3 endPoint;
    bool startedAbility;
    bool circlesActive;
    Light2D circle1Light;
    Light2D circle2Light;
    Light2D circle3Light;
  
    public Quaternion catchUpRotation;
    protected override void Initialization()
    {
        base.Initialization();
        cam = Camera.main;
        circle1Light = circle1.transform.GetChild(0).GetComponent<Light2D>();
        circle2Light = circle2.transform.GetChild(0).GetComponent<Light2D>();
        circle3Light = circle3.transform.GetChild(0).GetComponent<Light2D>();
    }
    protected override void HandleInput()
    {
        base.HandleInput();
        if (!AbilityAuthorized||_character.GetConditionalState() !=Character.ConditionalState.normal)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartDrop();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) && startedAbility)
        {
            EndDrop();
        }
    }

    public override void ResetAbility()
    {
        base.ResetAbility();
        startPoint=Vector3.zero;
        endPoint=Vector3.zero;
        currentDrag=0f;
        startedAbility=false;
    }


    void Shoot(Vector3 dragVelocity)
    {
        if (dragVelocity.magnitude>maxDrag)
        {
            dragVelocity = dragVelocity.normalized*maxDrag;
        }
        transform.parent=null;
        _controller.totalVelocityVector=dragVelocity*power;
        _character._movementState = Character.MovementState.midAir;
        _character.audioSource.PlayOneShot(_character.jumpBlock);
        ResetAbility();
        ResetCircles();
    }
    public void PublicShoot(Vector3 dragVelocity)
    {
         transform.parent=null;
        _controller.totalVelocityVector=dragVelocity*power;
        _character._movementState = Character.MovementState.midAir;
        _character.audioSource.PlayOneShot(_character.jumpBlock);
        ResetAbility();
        ResetCircles();
    }

  
    void ResetCircles()
    {
        circle1.transform.parent.transform.rotation = Quaternion.Euler(0,0,0);
        circle2.transform.parent.transform.rotation = Quaternion.Euler(0,0,0);
        circle2.transform.parent.transform.rotation = Quaternion.Euler(0,0,0);
        circle1.color = new Color(circle1.color.r,circle1.color.g,circle1.color.b,0);
        circle2.color = new Color(circle2.color.r,circle2.color.g,circle2.color.b,0);
        circle3.color = new Color(circle3.color.r,circle3.color.g,circle3.color.b,0);
        circle1Light.enabled=false;
        circle2Light.enabled=false;
        circle3Light.enabled=false;
    }
    
    void StartDrop(){
        var vectorDif = cam.ScreenToWorldPoint(Input.mousePosition) - _character.transform.position;
        vectorDif.z = 0;
        var distanceBetween = vectorDif.magnitude;
        if (distanceBetween > maximumDistance)
        {
            return;
        }
        startPoint= _character.transform.position;
        startPoint.z = 0;
        startedAbility=true;
        circle1Light.enabled=true;
        circle2Light.enabled=true;
        circle3Light.enabled=true;
    }
    void EndDrop(){
        endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        endPoint.z=0;
        var dragVelocity = (startPoint-endPoint);
        if(dragVelocity.magnitude <minDrag || dragVelocity.y <= 0.05f)
        {
            ResetAbility();
        }
        else
        {
            Shoot(dragVelocity);
        }
    }

    public override void Process()
    {
        if (_character._conditionalState == Character.ConditionalState.dead)
        {
            ResetAbility();
            ResetCircles();
        }
        if (_controller.boostBool && _character._movementState == Character.MovementState.ready)
        {
            Physics2D.IgnoreLayerCollision(12,6,true);
            PublicShoot(Vector3.up*boostForce);
            _controller.boostBool=false;
            _controller.boostControl=true;
        }
        if (_controller.boostControl && _controller.totalVelocityVector.y<=0f)
        {
            Physics2D.IgnoreLayerCollision(12,6,false);
            _controller.totalVelocityVector = (_controller.lastContactedObject.transform.position+Vector3.up*0.05f-_character.transform.position).normalized*8;
            _controller.boostControl=false;
            _controller.UpdateGravityState(false);
        }
        //startPoint = _character.transform.position; ?????
        //startPoint.z=0;
         if (startedAbility)
        {
        var lookVector = startPoint-cam.ScreenToWorldPoint(Input.mousePosition);
        lookVector.z=0;
        if (lookVector.y <= 0.05f)
        {
            circle1Light.color = Color.Lerp(circle1Light.color, new Color(224/255f,42/255f,25/255f,circle1Light.color.a),Time.deltaTime*15f);
            circle2Light.color = Color.Lerp(circle2Light.color, new Color(224/255f,42/255f,25/255f,circle2Light.color.a),Time.deltaTime*15f);
            circle3Light.color = Color.Lerp(circle3Light.color, new Color(224/255f,42/255f,25/255f,circle3Light.color.a),Time.deltaTime*15f);

            circle1.color = Color.Lerp(circle1.color, new Color(224/255f,42/255f,25/255f,circle1.color.a),Time.deltaTime*15f);
            circle2.color = Color.Lerp(circle2.color, new Color(224/255f,42/255f,25/255f,circle2.color.a),Time.deltaTime*15f);
            circle3.color = Color.Lerp(circle3.color, new Color(224/255f,42/255f,25/255f,circle3.color.a),Time.deltaTime*15f);
        }
        else
        {
            circle1Light.color = Color.Lerp(circle1Light.color, new Color(250/255f,255/255f,73/255f,circle1Light.color.a),Time.deltaTime*15f);
            circle2Light.color = Color.Lerp(circle2Light.color, new Color(250/255f,255/255f,73/255f,circle2Light.color.a),Time.deltaTime*15f);
            circle3Light.color = Color.Lerp(circle3Light.color, new Color(250/255f,255/255f,73/255f,circle3Light.color.a),Time.deltaTime*15f);

            circle1.color = Color.Lerp(circle1.color, new Color(250/255f,255/255f,73/255f,circle1.color.a),Time.deltaTime*15f);
            circle2.color = Color.Lerp(circle2.color, new Color(250/255f,255/255f,73/255f,circle2.color.a),Time.deltaTime*15f);
            circle3.color = Color.Lerp(circle3.color, new Color(250/255f,255/255f,73/255f,circle3.color.a),Time.deltaTime*15f);

           
        }
        currentDrag=lookVector.magnitude; //going to delete
        var targetRotation = Quaternion.LookRotation(Vector3.forward,lookVector);
        _rotatable.rotation = Quaternion.Lerp(_rotatable.rotation, targetRotation, Time.deltaTime * 20f);
        if (currentDrag>maxDrag)
        {
            currentDrag=maxDrag;
        }
        var dragScaleValue = currentDrag/(maxDrag-minDrag);
        var multiplier = 0.65f;
       
        circle1.color = Color.Lerp(circle1.color, new Color(circle1.color.r,circle1.color.g,circle1.color.b,0.75f),Time.deltaTime*5f);
        circle2.color = Color.Lerp(circle2.color, new Color(circle2.color.r,circle2.color.g,circle2.color.b,0.75f),Time.deltaTime*5f);
        circle3.color = Color.Lerp(circle3.color, new Color(circle3.color.r,circle3.color.g,circle3.color.b,0.75f),Time.deltaTime*5f);
        
        circle1Light.color = Color.Lerp(circle1Light.color, new Color(circle1Light.color.r,circle1Light.color.g,circle1Light.color.b,1),Time.deltaTime*5f);
        circle2Light.color = Color.Lerp(circle2Light.color, new Color(circle2Light.color.r,circle2Light.color.g,circle2Light.color.b,1),Time.deltaTime*5f);
        circle3Light.color = Color.Lerp(circle3Light.color, new Color(circle3Light.color.r,circle3Light.color.g,circle3Light.color.b,1),Time.deltaTime*5f);

      
        circle1.transform.localPosition = Vector3.Lerp(circle1.transform.localPosition, new Vector3(0,0.85f+dragScaleValue*multiplier,0),Time.deltaTime*5);
        circle2.transform.localPosition = Vector3.Lerp(circle2.transform.localPosition, new Vector3(0,1.25f+dragScaleValue*multiplier*2,0),Time.deltaTime*10);
        circle3.transform.localPosition = Vector3.Lerp(circle3.transform.localPosition, new Vector3(0,1.55f+dragScaleValue*multiplier*3,0),Time.deltaTime*15);
        circle1.transform.parent.transform.rotation = Quaternion.Lerp(circle1.transform.parent.transform.rotation, targetRotation, Time.deltaTime * 15);
        circle2.transform.parent.transform.rotation = Quaternion.Lerp(circle2.transform.parent.transform.rotation, targetRotation, Time.deltaTime * 15*9/10);
        circle3.transform.parent.transform.rotation = Quaternion.Lerp(circle3.transform.parent.transform.rotation, targetRotation, Time.deltaTime * 15*(9*9)/(10*10));
        }
        else
        {
            if (_character.GetMovementState() == Character.MovementState.ready)
            {
            var targetRotation = Quaternion.LookRotation(Vector3.forward,-_controller.savedContactNormal);
            _rotatable.rotation = Quaternion.Lerp(_rotatable.rotation, targetRotation, Time.deltaTime * 10f);

            circle1.color = Color.Lerp(circle1.color, new Color(circle1.color.r,circle1.color.g,circle1.color.b,0),Time.deltaTime*15f);
            circle2.color = Color.Lerp(circle2.color, new Color(circle2.color.r,circle2.color.g,circle2.color.b,0),Time.deltaTime*15f);
            circle3.color = Color.Lerp(circle3.color, new Color(circle3.color.r,circle3.color.g,circle3.color.b,0),Time.deltaTime*15f);

            circle1Light.color = Color.Lerp(circle1Light.color, new Color(circle1Light.color.r,circle1Light.color.g,circle1Light.color.b,0),Time.deltaTime*15f);
            circle2Light.color = Color.Lerp(circle2Light.color, new Color(circle2Light.color.r,circle2Light.color.g,circle2Light.color.b,0),Time.deltaTime*15f);
            circle3Light.color = Color.Lerp(circle3Light.color, new Color(circle3Light.color.r,circle3Light.color.g,circle3Light.color.b,0),Time.deltaTime*15f);
            }
        }
    }
    
}
