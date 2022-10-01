using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionEnding : MonoBehaviour
{
    public bool controller;
    [SerializeField] GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void PlayButton()
    {
        GameManager._gameManager._audioSource.PlayOneShot(LevelManager._levelManager.button1);
        controller=true;
    }
    // Update is called once per frame
    void Update()
    {
       Starting();
       Ending();
    }

    void Starting()
    {
        if (!controller)
        {
            transform.localScale = Vector3.Lerp(transform.localScale,Vector3.zero,Time.deltaTime*7f);
            if (transform.localScale.x <= 0.1 && !canvas.activeSelf)
            {
                canvas.SetActive(true);
            }
        }
        
    }
    void Ending()
    {
         if (controller)
        {
            if (canvas.gameObject.activeSelf)
            {
                canvas.SetActive(false);
            }
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(20f,20f,20f),Time.deltaTime*7f);
            if (transform.localScale.x >=19.5f)
            {
                LevelManager._levelManager.SceneNext();
            }
        }
    }

}
