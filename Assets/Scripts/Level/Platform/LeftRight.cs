using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRight : MonoBehaviour
{
    float minX = -2;
    float maxX = 2;
    Vector3 direction;
    public float speed=3f;
    float maxSpeed;
    Rigidbody2D _rigidbody;
    bool stopMoving=false;

    void Start()
    {
        _rigidbody= GetComponent<Rigidbody2D>();
        if (transform.position.x <maxX)
        {
            direction= Vector3.right;
        }
        else
        {
            direction = Vector3.left;
        }
    }

    void FixedUpdate()
    {
        if (!Platform._controller.freeze)
        {
            Move();
        }
    }
    
    void Move()
    {
        if (transform.position.x>=maxX)
        {
            direction=Vector3.left;
        }
        else if(transform.position.x <= minX)
        {
            direction=Vector3.right;
        }
        if (!stopMoving)
        {
            _rigidbody.transform.position += (direction*speed*Time.fixedDeltaTime);
        }
        
    }

    

}
