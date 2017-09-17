using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlareLogic : FFComponent
{
    public bool mLightIsOn = false;
    int mControllerNumber = 2;
    FFAction.ActionSequence stuffImDoing;
    public float timeOn = 1.0f;
    public float timeOff = 0.5f;
    public AnimationCurve mLightCurve;
    public AnimationCurve mLightOffCurve;
    void CheckAButton()
    {
        mLightIsOn = false;
        ChangeLightOn();
        stuffImDoing.Sync();
    }

    void ChangeLightOn()
    {
        mLightIsOn = true;
        stuffImDoing.Property(LightValue(), 100, mLightCurve, timeOn);
        stuffImDoing.Sync();
        stuffImDoing.Call(ChangeLightOff);
        stuffImDoing.Sync();
    }
    void ChangeLightOff()
    {
        stuffImDoing.Property(LightValue(), 0, mLightOffCurve, timeOff);
        stuffImDoing.Sync();
        stuffImDoing.Call(CheckAButton);
        stuffImDoing.Sync();
        mLightIsOn = false;
    }

    FFRef<float> LightValue()
    {
        return new FFRef<float>(() => transform.Find("Light").GetComponent<Light>().intensity, (v) => { transform.Find("Light").GetComponent<Light>().intensity = v; });
    }

    // Use this for initialization
    void Start () {
        stuffImDoing = action.Sequence();
        stuffImDoing.Call(CheckAButton);
        stuffImDoing.Sync();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
