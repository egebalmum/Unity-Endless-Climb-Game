using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;
using MPUIKIT;
public class ScoreAbility : CharacterAbility
{

   int combo = 0;
   bool controlNext;
   float comboStartedTime;
   bool restartDone;
   [SerializeField] float maxComboWaitTime;
   [SerializeField] int ComboCountForPowerUp;
   MPImage _image;
   MPImage _image2;
   Platform nextPlatform;
   bool comboActive;
   int _score;
   protected override void Initialization()
   {
        base.Initialization();
        _score=-1;
        _image = MapManager._mapManager._image;
        _image2 = MapManager._mapManager._image2;
   }

   void Update()
   {
     if (Time.time-comboStartedTime >= maxComboWaitTime)
          {
               comboActive=false;
               combo=0;
               _character.uiSource.pitch= 1;
          }
     if (comboActive==false &&restartDone==false)
     {
          _image.fillAmount = Mathf.Lerp(_image.fillAmount,0,6*Time.deltaTime);
          _image2.fillAmount = Mathf.Lerp(_image2.fillAmount,0,10*Time.deltaTime);
          if (_image.fillAmount==0f)
          {
               restartDone=true;
          }
     }
     if (comboActive && restartDone==false)
     {
          _image.fillAmount = Mathf.Lerp(_image.fillAmount,combo/6f,6*Time.deltaTime);
          float value = 1 - (Time.time-comboStartedTime)/maxComboWaitTime;
          _image2.fillAmount = Mathf.Lerp(_image2.fillAmount,value,25*Time.deltaTime);
     }
   }

   void EndCombo()
   {
          comboActive=false;
          combo=0;
          _character.uiSource.pitch= 1;
          controlNext=false;
          
   }

     public override void Process()
     {

          if (comboActive)
          {
               if (_controller.boostControl)
               {
                    
                    controlNext=true;
                    if (_character.transform.position.y>= nextPlatform.transform.position.y)
                    {
                         if(nextPlatform.GetComponent<Blinker>() !=null)
                         {
                              nextPlatform.GetComponent<Blinker>().EndBlinkCycle();
                         }
                         nextPlatform.tag="CONTACTED";
                         _controller.lastContactedObject = nextPlatform.gameObject;
                         _score+=1;
                         _character.uiSource.PlayOneShot(_character.score);
                         nextPlatform._light.enabled=true;
                         var particle = Instantiate(nextPlatform._particle,nextPlatform.transform.position,nextPlatform.transform.rotation);
                         Destroy(particle,3f);
                         MapManager._mapManager.UpdateScore(_score);
                         nextPlatform = MapManager.Node.TotalPlatforms[(MapManager.Node.TotalPlatforms.IndexOf(nextPlatform)+1)];
                    }
               }
               if (!_controller.boostControl && controlNext)
               {
                    EndCombo();
               }
          }
     }
   public override void CollisionEnterBlock(Collision2D other)
   {
        if(_controller.contacted.gameObject.layer ==6 && _controller.sticky==true && _controller.contacted.gameObject.tag=="WAITING")
        {
          

          if (!comboActive)
          {
               comboStartedTime = Time.time;
               comboActive=true;
               restartDone=false;
               combo +=1;
               _character.uiSource.pitch+= 0.333f;
          }
          else
          {
               if (Time.time-comboStartedTime <= maxComboWaitTime && combo<ComboCountForPowerUp)
               {
                    combo +=1;
                    comboStartedTime=Time.time;
                    _character.uiSource.pitch+= 0.333f;
               }
          }




          _controller.contacted.gameObject.tag="CONTACTED";
          _score+=1;
          _character.uiSource.PlayOneShot(_character.score);
          Platform platform = other.gameObject.GetComponent<Platform>();
          platform._light.enabled=true;
          var particle = Instantiate(platform._particle,platform.transform.position,platform.transform.rotation);
          Destroy(particle,3f);
          MapManager._mapManager.UpdateScore(_score);


          
          if (combo==ComboCountForPowerUp)
          {
               nextPlatform = MapManager.Node.TotalPlatforms[(MapManager.Node.TotalPlatforms.IndexOf(other.gameObject.GetComponent<Platform>())+1)];
               _controller.boostBool=true;
          }

        }
   }
  
}
