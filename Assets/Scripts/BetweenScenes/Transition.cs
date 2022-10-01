using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x >=0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale,Vector3.zero,Time.deltaTime*7f);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
}
