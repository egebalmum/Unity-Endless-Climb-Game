using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Character : MonoBehaviour
{
    [SerializeField] public ParticleSystem dropParticle;
    [SerializeField] public bool inGame;
    public AudioSource audioSource;
    public AudioClip deathSound;
    public AudioClip jumpWall;
    public AudioClip jumpBlock;
    public AudioSource uiSource;
    public AudioClip score;
    public Controller _controller;
    public GameObject _model;
    [SerializeField] ParticleSystem _particle;

    public enum ConditionalState {normal, dead, paused, controlledMovement}
    public enum MovementState {stuck, ready ,midAir}

    public ConditionalState _conditionalState;
    public MovementState _movementState;
    public MovementState _lastFrameState;

    private CharacterAbility[] _characterAbilities;
    
    // Start is called before the first frame update

    void Start(){
        Initialization();
    }

    void Initialization(){
        _controller = GetComponent<Controller>();
        CacheAbilities();
        _conditionalState = ConditionalState.normal;
        SetMovementState(MovementState.midAir);
    }

    public virtual void CacheAbilities()
    {
		_characterAbilities = this.gameObject.GetComponents<CharacterAbility>();
    }
    public void SetMovementState(MovementState state){
        _movementState = state;
    }
    public MovementState GetMovementState(){
        return _movementState;
    }
    public void SetConditionalState(ConditionalState state){
        _conditionalState = state;
    }
    public ConditionalState GetConditionalState(){
        return _conditionalState;
    }
    void OnCollisionEnter2D(Collision2D other) {
    if (GetMovementState() != Character.MovementState.midAir){return;}
      _controller.CollisionEnterBlock(other);
        if (!_controller.ignoreCollision)
        {
            foreach (CharacterAbility ability in _characterAbilities)
			    {
				    if (ability.enabled && ability.AbilityInitialized)
				    {
					ability.CollisionEnterBlock(other);
				    }
			    }
        }

    }
    // Update is called once per frame
    void Update()
    {
        EveryFrame();
    }


    void EveryFrame(){
        EarlyProcessAbilities();
		ProcessAbilities();
		LateProcessAbilities();
        _lastFrameState = _movementState;
    }

    protected virtual void EarlyProcessAbilities()
		{
			foreach (CharacterAbility ability in _characterAbilities)
			{
				if (ability.enabled && ability.AbilityInitialized)
				{
					ability.EarlyProcess();
				}
			}
		}

		/// <summary>
		/// Calls all registered abilities' Process methods
		/// </summary>
		protected virtual void ProcessAbilities()
		{
			foreach (CharacterAbility ability in _characterAbilities)
			{
				if (ability.enabled && ability.AbilityInitialized)
				{
					ability.Process();
				}
			}
		}

		/// <summary>
		/// Calls all registered abilities' Late Process methods
		/// </summary>
		protected virtual void LateProcessAbilities()
		{
			foreach (CharacterAbility ability in _characterAbilities)
			{
				if (ability.enabled && ability.AbilityInitialized)
				{
					ability.LateProcess();
				}
			}
		}

        public void ExplosionEffect()
        {
            if (_conditionalState == ConditionalState.dead)
            {
                return;
            }
            Instantiate(_particle,transform.position,Quaternion.identity);
            audioSource.PlayOneShot(deathSound);
            _conditionalState = ConditionalState.dead;
            _model.SetActive(false);
            _controller._collider.enabled=false;
        }

       
       
        public void OnDeath(){
        
            
        }
        public void OnLevelComplete(){
            //win effects
            LevelManager._levelManager.SceneNext();
        }

}
