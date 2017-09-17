using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTimer : MonoBehaviour {

    public float lifespan = 1.5f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        lifespan -= Time.deltaTime;
        if (lifespan <= 0)
            Destroy(gameObject);
	}
}
