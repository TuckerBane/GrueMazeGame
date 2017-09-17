using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPlayerController : FFComponent {
    public float timeOn = 1.0f;
    public float timeOff = 0.5f;
    public float testGrueVis = 0.0f;
    public float accelerationSpeed = 10.0f;
    public float maxSpeed = 10.0f;
    FFAction.ActionSequence stuffImDoing;
    public AnimationCurve mLightCurve;
    public AnimationCurve mLightOffCurve;
    private Rigidbody myRidigBody;

    public float mStunTime = 0.0f;

    public bool mLightIsOn = false;
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
        stuffImDoing.Sync();
    }

    void CheckAButton()
    {
        mLightIsOn = false;
        if (Input.GetButtonUp("A" + mControllerNumber) && mStunTime <= 0.0f && !GetComponent<GrueLogic>())
        {
            ChangeLightOn();
            return;
        }
        stuffImDoing.Call(CheckAButton);
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
	
	// Update is called once per frame
	void Update () {

        if (mStunTime >= 0.0f)
            mStunTime -= Time.deltaTime;

        if (mStunTime > 0.0f)
        {
            mLightIsOn = false;
            transform.Find("Light").GetComponent<Light>().intensity = 0.0f;
            myRidigBody.velocity = Vector3.zero;
            return;
        }
            
        Vector3 movementVector = new Vector3();

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
