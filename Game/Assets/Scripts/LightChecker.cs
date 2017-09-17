using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightChecker : MonoBehaviour {

    public GameObject testTargetGrue;
    public float testHitNum = -1337.0f;

    public float mMaxRange = 10.0f;
    public float mFullyEffectiveRange = 5.0f;
    public bool mIsCone = false;
    public float mConeAngleDegree = 30.0f;

    public float angle = 0.0f;




	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        testHitNum = GetLightLevel(testTargetGrue.transform);
	}

    float GetLightLevel(Transform grueTrans )
    {
        float distance = (grueTrans.position - transform.position).magnitude;
        if(distance > mMaxRange)
        {
            return 0.0f;
        }
        RaycastHit hit = new RaycastHit();
        Vector3 start = transform.position;
        Vector3 direction = (grueTrans.position - transform.position).normalized;
        if (!Physics.Raycast(start, direction, out hit, mMaxRange) )
        {
            return 0.0f;
        }
        // we hit something before the Grue
        if(hit.collider.gameObject != grueTrans.gameObject)
        {
            return 0.0f;
        }

        //shape check
        if (mIsCone)
        {
            Vector3 toGrue = GameMath.UnitVectorBetween(transform, grueTrans);
            toGrue.y = 0;
            Vector3 forward = transform.forward;
            forward.y = 0;

            float angleBetweenDegree = Mathf.Acos(Vector3.Dot(forward, toGrue)) * 180 / Mathf.PI;
            angle = angleBetweenDegree;
            if (angleBetweenDegree > (mConeAngleDegree / 2))
            {
                return 0.0f;
            }
        }

        if (distance <= mFullyEffectiveRange)
        {
            return 1.0f;
        }

        return (mMaxRange - distance) / (mMaxRange - mFullyEffectiveRange);
    }

}
