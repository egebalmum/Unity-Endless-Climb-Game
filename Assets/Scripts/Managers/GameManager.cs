using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioSource _audioSource;
    public AudioSource music;
    static public GameManager _gameManager;
    
    void Awake()
    {
        DontDestroyOnLoad (transform.gameObject);
        music.Play();
        _gameManager = this;
    }
    public int targetFrameRate=144;
    void Start(){
        Application.targetFrameRate=targetFrameRate;
    }
}
