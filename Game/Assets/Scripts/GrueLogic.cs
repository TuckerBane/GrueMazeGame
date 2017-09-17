using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GrueLogic : FFComponent
{

    public float accelerationSpeed = 100.0f;
    public float maxSpeed = 200.0f;

    public float mMaxStunTime = 5.0f;
    public float mStunTime = 0.0f;
    public float mStunRange = 1.0f;

    Rigidbody myRidigBody;
    ControllerPlayerController[] mPlayers;
    LightChecker[] mAllLights;
    GameObject mTarget;
    FFAction.ActionSequence mConvertMaybe;

    GameObject GetTarget()
    {
        GameObject obj = null;

        while(!obj || obj.GetComponent<GrueLogic>())
        {
            obj = mPlayers[Random.Range((int)0, (int)3)].gameObject;
        }
        return obj;
    }

    // Use this for initialization
    void Start () {
        mConvertMaybe = action.Sequence();
        myRidigBody = GetComponent<Rigidbody>();
        mPlayers = FindObjectsOfType<ControllerPlayerController>();
        mAllLights = FindObjectsOfType<LightChecker>();
        mTarget = GetTarget();
	}
	
    float TotalLight()
    {
        float light = 0;

        foreach(LightChecker ligity in mAllLights){
            light += ligity.GetLightLevel(transform);
        }

        return light;
    }

    void StunFinish()
    {
        int convertedCount = 0;
        for(int i = 0; i < 3; ++i)
        {
            if (mPlayers[i].GetComponent<GrueLogic>())
                ++convertedCount;
        }
        if (convertedCount >= 2)
        {
            SceneManager.LoadScene("YouLose");
            return;
        }

        if (!mTarget.GetComponent<GrueLogic>())
        {
            //grueize
            mTarget.AddComponent<GrueLogic>();
            mTarget.GetComponent<Collider>().isTrigger = true;
            //change model to grueling
        }
        mTarget = GetTarget();
    }

	// Update is called once per frame
	void Update () {

        if (FindObjectOfType<GlobalLightChecker>().GetTotalLightLevel(transform) >= 0.2)
        {
            if (!GetComponent<ControllerPlayerController>())
                Destroy(gameObject);
            else
            {
                //un grueize
                mTarget.GetComponent<Collider>().isTrigger = false;
                mConvertMaybe.ClearSequence();
                // change model back to normal
                Destroy(this);
            }
        }

        if (mStunTime >= 0.0f)
            mStunTime -= Time.deltaTime;

        if (mStunTime > 0.0f)
        {
            myRidigBody.velocity = Vector3.zero;
            return;
        }

        if(GameMath.DistanceBetween(this,mTarget) < mStunRange)
        {
            mConvertMaybe.Delay(mMaxStunTime);
            mConvertMaybe.Sync();
            mConvertMaybe.Call(StunFinish);
            mConvertMaybe.Sync();

            this.mStunTime = mMaxStunTime;
            mTarget.GetComponent<ControllerPlayerController>().mStunTime = mMaxStunTime;
        }

        Vector3 movementVector = GameMath.UnitVectorBetween(this, mTarget);
        movementVector *= accelerationSpeed * Time.deltaTime;
        myRidigBody.AddForce(movementVector);
        if (myRidigBody.velocity.magnitude >= maxSpeed)
        {
            myRidigBody.velocity = myRidigBody.velocity.normalized * maxSpeed;
        }

        transform.rotation = Quaternion.AngleAxis(
            Mathf.Atan2(myRidigBody.velocity.x * -1, myRidigBody.velocity.z) * 180 / Mathf.PI
            , Vector3.down);
    }
}
