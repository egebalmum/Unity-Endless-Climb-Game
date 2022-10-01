using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class EffectOnBoost : CharacterAbility
{
    Volume _volume;
    Bloom _bloom;
    bool controller;
    protected override void Initialization()
    {
        base.Initialization();
        _volume = MapManager._mapManager._volume;
        Bloom tmp;
        if (_volume.profile.TryGet<Bloom>(out tmp))
        {
            _bloom = tmp;
        }
    }
    public override void Process()
    {
        if (_controller.boostControl && !controller)
        {
            //white-black
            _controller.freeze=true;
            controller=true;
        }
        if (_controller.freeze)
        {
            _bloom.intensity.value = Mathf.Lerp(_bloom.intensity.value,3.5f,20*Time.deltaTime);
        }
        else
        {
            _bloom.intensity.value = Mathf.Lerp(_bloom.intensity.value,0.1f,20*Time.deltaTime);
        }
        



    }
    public override void CollisionEnterBlock(Collision2D other)
        {
            if (other.gameObject.layer==6 && controller)
            {
                MapManager._mapManager.controlforAbra=false;
                controller=false;
                //normal screen
                _controller.freeze=false;
                _controller.UpdateGravityState(true);
            }
        }


}
