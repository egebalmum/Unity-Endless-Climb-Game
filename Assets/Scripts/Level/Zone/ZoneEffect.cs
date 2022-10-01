using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneEffect : MonoBehaviour
{
    public float timeScale = 1;
    public Vector2 gravityDirection;
    public float gravityAcceleration = 0;
    Collider2D _collider;
    Controller _controller;
    Character _character;
    // Start is called before the first frame update
    void Start(){
        Initialization();
    }
    void Initialization(){
        _controller=FindObjectOfType<Controller>();
        _character=FindObjectOfType<Character>();
        _collider= GetComponent<Collider2D>();
    }

    void Update()
    {   
        if (_character!=null)
        {
            if (_collider.OverlapPoint(_controller.transform.position))
            {
                if (gravityAcceleration==0)
                {
                    _controller.UpdateGravityState(false);
                }
                else
                {
                    _controller.UpdateGravityState(true);
                }
            _controller.UpdateGravity(gravityDirection,gravityAcceleration);
            }
        }
    }
}
