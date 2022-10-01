using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
   
    public AudioClip button1;
    public AudioClip button2;
    [SerializeField] Transform _transition;
    static public LevelManager _levelManager;
    void Awake()
    {
        _levelManager = this;
    }

    public void SceneNext()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
    
    public void SceneLevelSelection()
    {
        SceneManager.LoadScene("LevelSelection");
    }

    public void SceneReload()
    {
        GameManager._gameManager._audioSource.PlayOneShot(button1);
        Platform.platformCounter =0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void SceneMenu()
    {
        GameManager._gameManager._audioSource.PlayOneShot(button2);
        Platform.platformCounter =0;
        SceneManager.LoadScene("Menu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }


    public void TransitionActivated()
    {
        StartCoroutine("Transition");
    }

    IEnumerator Transition()
    {
        while (_transition.localScale.x <= 19.5f)
        {
            _transition.localScale = Vector3.Lerp(_transition.localScale,new Vector3(20f,20f,20f),Time.deltaTime*7f);
            yield return new WaitForEndOfFrame();
        }
        SceneNext();
    }


}
