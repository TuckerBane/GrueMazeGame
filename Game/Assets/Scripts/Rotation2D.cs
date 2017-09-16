using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation2D : MonoBehaviour {

    public float angle = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Quaternion goalDirection = Quaternion.AngleAxis(angle, Vector3.forward);
        gameObject.transform.rotation = goalDirection;

        angle += 180.0f * Time.deltaTime;
	}
}
