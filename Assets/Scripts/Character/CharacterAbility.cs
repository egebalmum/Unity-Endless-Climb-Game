using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAbility : MonoBehaviour
{
    public bool AbilityPermitted = true;

    public Character.ConditionalState[] BlockingConditionalStates;
    public Character.MovementState[] BlockingMovementStates;
    public virtual bool AbilityAuthorized
    {   
        get
            {
            if (_character != null)
            {
                if ((BlockingMovementStates!=null) && (BlockingMovementStates.Length>0))
                {
                    for (int i = 0; i<BlockingMovementStates.Length; i++)
                    {
                        if (BlockingMovementStates[i] == (_character.GetMovementState()))
                        {
                            return false;
                        }
                    }
                }

                if ((BlockingConditionalStates != null) && (BlockingConditionalStates.Length >0))
                {
                    for (int i=0; i<BlockingConditionalStates.Length;i++)
                    {
                        if (BlockingConditionalStates[i] == (_character._conditionalState))
                        {
                            return false;
                        }
                    }    
                    
                }
            }
            return AbilityPermitted;
            }
    }

    public bool AbilityInitialized {get {return _abilityInitialized;}}

    protected Character _character;
    protected Controller _controller;
    protected GameObject _model;
    protected bool _abilityInitialized;



    protected virtual void Awake(){
        PreInitialization();
    }
    protected virtual void Start(){
        Initialization();
    }

    protected virtual void PreInitialization()
    {   
        _character = GetComponent<Character>();
    }
    protected virtual void Initialization()
    {
        _controller = GetComponent<Controller>();
        _model = _character._model;
        _abilityInitialized =true;
    }

    public virtual void CollisionEnterBlock(Collision2D other){

    }
    protected virtual void HandleInput(){

    }


    public virtual void EarlyProcess(){
        HandleInput();
    }


    public virtual void Process(){

    }
    public virtual void LateProcess(){

    }
    public virtual void PermitAbility(bool abilityPermitted)
    {
        AbilityPermitted = abilityPermitted;
    }
    public virtual void ResetAbility(){

    }


    public virtual void OnDeath()
    {

    }
}
