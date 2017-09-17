using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWallLogic : MonoBehaviour {

    public BeASwitch[] mSwitches;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
        for(int i = 0; i < mSwitches.Length; ++i)
        {
            if(mSwitches[i].mActivatedtime <= 0.0f)
            {
                return;
            }
        }
        Destroy(gameObject);
	}
}
