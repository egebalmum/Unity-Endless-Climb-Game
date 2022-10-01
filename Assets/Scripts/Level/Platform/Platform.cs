using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
public class Platform : MonoBehaviour
{
    public static Controller _controller;
    public ParticleSystem _particle;
    bool controller;
    public int platformID;
    public Light2D _light;
    public static int platformCounter;
    // Start is called before the first frame update
    void Awake()
    {
        platformCounter+=1;
        platformID=platformCounter;
        if (_controller ==null)
        {
            _controller = FindObjectOfType<Controller>();
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(this.tag=="CONTACTED" && !controller)
        {
            controller=true;
            //_spawner.deadZone.transform.Translate(Vector3.up*3);
            if (this.platformID>=4)
            {
                MapManager._mapManager.DeletePlatform();
            }
        }
        
    }
}
