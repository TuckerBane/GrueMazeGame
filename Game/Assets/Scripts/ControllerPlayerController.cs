using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerPlayerController : MonoBehaviour {

    public float accelerationSpeed = 10.0f;
    public float maxSpeed = 10.0f;
    private Rigidbody myRidigBody;

    public int mControllerNumber = 0;
    public float controllerX;
    public float controllerY;

    public int joy = 0;
    public int axis = 0;
    public float testOut = 0.0f;

	// Use this for initialization
	void Start () {
        myRidigBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 movementVector = new Vector3();

        switch (mControllerNumber)
        {
            case 0:
                movementVector.x = Input.GetAxis("joy_0_axis_0");
                movementVector.y = -Input.GetAxis("joy_0_axis_1");
            break;
            case 1:
                movementVector.x = Input.GetAxis("joy_1_axis_0");
                movementVector.y = -Input.GetAxis("joy_1_axis_1");
                break;
            case 2:
                movementVector.x = Input.GetAxis("joy_2_axis_0");
                movementVector.y = -Input.GetAxis("joy_2_axis_1");
                break;
        }

        testOut = Input.GetAxis("joy_" + joy + "_axis_" + axis);

        controllerX = Input.GetAxis("joy_0_axis_0");
        controllerY = Input.GetAxis("joy_0_axis_1");
        movementVector.Normalize();
        movementVector *= accelerationSpeed * Time.deltaTime;
        myRidigBody.AddForce(movementVector);
        myRidigBody.velocity = new Vector3(0, 0, 0);
        if (myRidigBody.velocity.magnitude >= maxSpeed)
        {
            myRidigBody.velocity = myRidigBody.velocity.normalized * maxSpeed;
        }
        GameMath.PointObjAt2D(gameObject, gameObject.transform.position + myRidigBody.velocity);
        transform.rotation = Quaternion.AngleAxis(
            Mathf.Atan2(myRidigBody.velocity.x, myRidigBody.velocity.z)
            , Vector3.down);

    }
}
