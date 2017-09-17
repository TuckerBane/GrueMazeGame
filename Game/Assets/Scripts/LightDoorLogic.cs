using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDoorLogic : MonoBehaviour {

    public Material mNormalMaterial;
    public Material mGhostMaterial;
    public bool mNeedsRedLight = false;
    LightChecker RedPlayerLight;
    public bool mNeedsBlueLight = false;
    LightChecker BluePlayerLight;
    public bool mNeedsGreenLight = false;
    LightChecker GreenPlayerLight;

    public float mMaxOpentime = 5.0f;
    public float mOpenTime = 0.0f;

    // Use this for initialization
    void Start () {
        RedPlayerLight = GameObject.Find("RedCharacter").GetComponent<LightChecker>();
        BluePlayerLight = GameObject.Find("BlueCharacter").GetComponent<LightChecker>();
        GreenPlayerLight = GameObject.Find("GreenCharacter").GetComponent<LightChecker>();

    }
	
	// Update is called once per frame
	void Update () {
        mOpenTime -= Time.deltaTime;
        if (mOpenTime > 0)
        {
            GetComponent<Collider>().isTrigger =  true;
            GetComponent<MeshRenderer>().material = mGhostMaterial;
        }
        else
        {
            GetComponent<Collider>().isTrigger = false ;
            GetComponent<MeshRenderer>().material = mNormalMaterial;
        }

        if((!mNeedsRedLight || RedPlayerLight.GetLightLevel(transform) > 0 )
            && (!mNeedsBlueLight || BluePlayerLight.GetLightLevel(transform) > 0)
            && (!mNeedsGreenLight || BluePlayerLight.GetLightLevel(transform) > 0) )
        {
            mOpenTime = mMaxOpentime;
        }


    }
}
