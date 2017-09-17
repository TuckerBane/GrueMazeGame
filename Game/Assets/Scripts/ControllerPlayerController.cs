using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPlayerController : FFComponent {
    public float testGrueVis = 0.0f;
    public float accelerationSpeed = 10.0f;
    public float maxSpeed = 10.0f;
    FFAction.ActionSequence stuffImDoing;
    public AnimationCurve mLightCurve;
    public AnimationCurve mLightOffCurve;
    private Rigidbody myRidigBody;

    public int mControllerNumber = 0;
    //public Color mColor;
    public float controllerX;
    public float controllerY;

    public int joy = 0;
    public int axis = 0;
    
    public bool testButton = false;
	// Use this for initialization
	void Start () {
        myRidigBody = GetComponent<Rigidbody>();
        transform.Find("Light").GetComponent<Light>().intensity = 50 * 0.25f;
        stuffImDoing = action.Sequence();
        stuffImDoing.Call(CheckAButton);
    }

    void CheckAButton()
    {
        mLightIsOn = false;
        if (Input.GetButtonUp("A" + mControllerNumber))
        {
            ChangeLightOn();
            return;
        }
        stuffImDoing.Call(CheckAButton);
        stuffImDoing.Sync();
    }

    bool mLightIsOn = false;
    void ChangeLightOn()
    {
        mLightIsOn = true;
        stuffImDoing.Property(LightValue(), 100, mLightCurve, 4.0f);
        stuffImDoing.Sync();
        stuffImDoing.Call(ChangeLightOff);
    }
    void ChangeLightOff()
    {
        stuffImDoing.Property(LightValue(), 0, mLightOffCurve, 4.0f);
        stuffImDoing.Sync();
        stuffImDoing.Call(CheckAButton);
    }

       FFRef<float> LightValue()
    {
        return new FFRef<float>(() => transform.Find("Light").GetComponent<Light>().intensity, (v) => { transform.Find("Light").GetComponent<Light>().intensity = v; });
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 movementVector = new Vector3();

        /*
        stuffImDoing.Call(checkAButton);

            
        stuffImDoing.Call(LightBeOn);
        stuffImDoing.Delay(LightOnTime);
        stuffImDoing.Call(LightBeOff);
        stuffImDoing.Delay(cooldownTime);
        stuffImDoing.Call(TrueizemCanButton);
        stuffImDoing.Sync();
        stuffImDoing.Delay(cooldownTime);
        stuffImDoing.Sync();
        */

        switch (mControllerNumber)
        {
            case 0:
                movementVector.x = Input.GetAxis("joy_0_axis_0");
                movementVector.z = -Input.GetAxis("joy_0_axis_1");
            break;
            case 1:
                movementVector.x = Input.GetAxis("joy_1_axis_0");
                movementVector.z = -Input.GetAxis("joy_1_axis_1");
                break;
            case 2:
                movementVector.x = Input.GetAxis("joy_2_axis_0");
                movementVector.z = -Input.GetAxis("joy_2_axis_1");
                break;
        }

        //controllerX = Input.GetAxis("joy_0_axis_0");
        //controllerY = Input.GetAxis("joy_0_axis_1");
        movementVector.Normalize();
        movementVector *= accelerationSpeed * Time.deltaTime;
        myRidigBody.AddForce(movementVector);
        if (myRidigBody.velocity.magnitude >= maxSpeed)
        {
            myRidigBody.velocity = myRidigBody.velocity.normalized * maxSpeed;
        }

        transform.rotation = Quaternion.AngleAxis(
            Mathf.Atan2(myRidigBody. velocity.x * -1, myRidigBody.velocity.z) * 180 / Mathf.PI
            , Vector3.down);
            

    }
}
