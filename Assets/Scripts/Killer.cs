using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killer : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float speed = 2f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.transform.Translate(Vector3.up*speed*Time.deltaTime,Space.World);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag=="Player")
        {
            MapManager._mapManager.AlienDeath();
        }
    }
}
