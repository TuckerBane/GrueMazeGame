using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeASwitch : MonoBehaviour {

    public float mActivatedtime = 0.0f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        mActivatedtime -= Time.deltaTime;
	}

    public void OnCollision(GameObject obj)
    {
        if(obj.GetComponent<ControllerPlayerController>() && !obj.GetComponent<GrueLogic>())
            mActivatedtime = 2.0f;
    }
    private void OnCollisionEnter(Collision collision)
    {
        OnCollision(collision.collider.gameObject);
    }

    private void OnCollisionStay(Collision collision)
    {
        OnCollision(collision.collider.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        OnCollision(collision.collider.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnCollision(other.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        OnCollision(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        OnCollision(other.gameObject);
    }

}
