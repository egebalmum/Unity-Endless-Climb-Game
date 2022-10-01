using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothAbility : CharacterAbility
{


    [SerializeField] float timeOnSticky;
    [SerializeField] float timeOnReflector;
    public Transform _rotatable;
    public Transform _scalable;

    public float StretchMultiplier = 0.005f;
    public float SquashMultiplier = 0.06f;
    public float DelayMultiplier = 0.2f;
    public float ScaleChangeRate = 20f;

    private Quaternion _targetRotation;
    private Quaternion _currentRotation;

    private float _currentScale = 1f;
    private float _targetScale = 1f;
    private Vector3 _savedVelocity;
    private Vector3 _savedContactNormal;
    private bool _inverted;
    public override void LateProcess()
    {
        base.LateProcess();
        if (_character.GetMovementState() == Character.MovementState.midAir)
        {
            _targetRotation = Quaternion.LookRotation(Vector3.forward, _controller.totalVelocityVector);
            float velocity = _controller.totalVelocityVector.magnitude;
            _targetScale = 1f + velocity * velocity * StretchMultiplier;
            _targetScale = Mathf.Clamp(_targetScale, 1f, 2f);
        }

        _currentScale = Mathf.Lerp(_currentScale, _targetScale, Time.deltaTime * ScaleChangeRate);
        
        SquashScale(_currentScale);

        if (!_inverted && _currentScale >= 1f) {
            _inverted = true;
            _rotatable.rotation = _targetRotation = _currentRotation = Quaternion.LookRotation(Vector3.forward, _savedContactNormal);
            
        }
        
        if (_character.GetMovementState() != Character.MovementState.ready)
        {
         _currentRotation = Quaternion.Lerp(_currentRotation, _targetRotation, Time.deltaTime * 10f);
        _rotatable.rotation = _currentRotation;
        }
    }

    void SquashScale(float value) {
        if (value == 0f) return;
        _scalable.localScale = new Vector3(1/value, value, 1);
    }


    public override void CollisionEnterBlock(Collision2D other)
    {
        base.CollisionEnterBlock(other);
        _savedVelocity = _controller.savedTotalVelocityVector;
        _savedContactNormal = _controller.savedContactNormal;
       // _savedContactNormal = other.contacts[0].normal;
        _controller._rigidbody.isKinematic = true;

        _targetRotation = Quaternion.LookRotation(Vector3.forward, -_controller.contacted.GetContact(0).normal);

        _targetScale = Mathf.Lerp(1f, 0.3f, _savedVelocity.magnitude * SquashMultiplier);

        float velocityProjectionMagnitude = Vector3.Project(_savedVelocity, -_savedContactNormal).magnitude;
        float jumpMultiplier;
        if (other.gameObject.layer==6 && _controller.sticky==true)
        {
            jumpMultiplier=timeOnSticky;
            _character.audioSource.PlayOneShot(_character.jumpBlock);
        }
        else
        {
            jumpMultiplier=timeOnReflector;
            _character.audioSource.PlayOneShot(_character.jumpWall);
            if (other.gameObject.layer == 7)
            {
                if (_character.inGame)
                {
                    var splat = Instantiate(_controller.splats[Random.Range(0,2)],transform.position,Quaternion.Euler(0,0,Random.Range(0f,350f)));
                    MapManager._mapManager.splats.Add(splat.transform);
                    Instantiate(_character.dropParticle,other.GetContact(0).point,Quaternion.LookRotation(other.GetContact(0).normal));
                }
            }   
        }
        float groundedTime = velocityProjectionMagnitude * DelayMultiplier*jumpMultiplier;
        groundedTime = Mathf.Clamp(groundedTime, 0f, 0.15f);

        transform.position = _controller.contacted.GetContact(0).point + _controller.contacted.GetContact(0).normal * 0.3f;

        Invoke("StartToStretch", groundedTime);
        
        Invoke("DisableIsKinematic", groundedTime * 1.5f);
        
        
    }

    void StartToStretch() {
        _targetScale = Mathf.Lerp(0.5f, 1f, 1f + _savedVelocity.magnitude * StretchMultiplier);
        _inverted = false;
    }

    void DisableIsKinematic() {
        _controller._rigidbody.isKinematic=false;
        Invoke("ExitSaveMode", 0.02f);
    }

    void ExitSaveMode() {

        _controller.ExitContact();
    }
}
