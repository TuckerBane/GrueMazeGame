using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalLightChecker : MonoBehaviour {

    LightChecker[] mAllLights;

    public float GetTotalLightLevel(Transform otherObjecttransform)
    {
        float light = 0;
        foreach (LightChecker ligity in mAllLights)
        {
            light += ligity.GetLightLevel(otherObjecttransform);
        }
        return light;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        mAllLights = FindObjectsOfType<LightChecker>();
    }
}
