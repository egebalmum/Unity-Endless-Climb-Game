using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Blinker : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;
    bool ctrl;
    bool isBlink;
    Color savedColor;
    float startTime;
    [SerializeField] SpriteRenderer n1;
    [SerializeField] SpriteRenderer n2;
    [SerializeField] SpriteRenderer n3;
    [SerializeField] SpriteRenderer n4;
    [SerializeField] SpriteRenderer b1;
    [SerializeField] SpriteRenderer b2;
    [SerializeField] Light2D _light;
    BoxCollider2D _collider;
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        savedColor=_light.color;
        startTime = Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Platform._controller.freeze)
        {
            if (!ctrl)
            {
                if (Time.time-startTime>=1.5f)
                {
                    startTime=Time.time;
                    if (isBlink)
                    {
                        GoNormal();
                    }
                    else
                    {
                        GoBlink();
                    }
                    _audioSource.Play();
                }
            }
        }
        else
        {
            if (isBlink!=false)
            {
                GoNormal();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag=="Player")
        {
            EndBlinkCycle();
        }
    }
    public void EndBlinkCycle()
    {
        ctrl=true;
        GoNormal();
    }
    

    void GoBlink()
    {
        gameObject.layer=11;
        isBlink=true;
        n1.enabled=false;
        n2.enabled=false;
        n3.enabled=false;
        n3.enabled=false;
        n4.enabled=false;
        _collider.enabled=false;
        b1.enabled=true;
        b2.enabled=true;
        _light.color = new Color(204/255f,1/255f,3/255f,_light.color.a);
    }
    void GoNormal()
    {
        gameObject.layer=6;
        isBlink=false;
        n1.enabled=true;
        n2.enabled=true;
        n3.enabled=true;
        n3.enabled=true;
        n4.enabled=true;
        _collider.enabled=true;
        b1.enabled=false;
        b2.enabled=false;
        _light.color = savedColor;
    }
}
