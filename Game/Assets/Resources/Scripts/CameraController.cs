using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : FFComponent {


    public Transform []targets;
    
    Vector3 targetPosition;
    float targetSize;

    FFAction.ActionSequence movementSeq;
    FFAction.ActionSequence sizeSeq;


    public Vector3 offset;
    public float SizeLeash = 2.5f;
    public float PositionLeash = 2.5f;
    public float MinCameraSize = 10.0f;
    public float MaxCameraSize = 25.0f;
    public float AdjustmentTime = 0.6f;


    private float saveHeight;

    public Transform Camera;
    public Transform Bounds;

    FFRef<float> RefCameraSize()
    {
        return new FFRef<float>(() => Camera.GetComponent<Camera>().orthographicSize, (v) => { Camera.GetComponent<Camera>().orthographicSize = v; });
    }
    FFRef<Vector3> RefboundScale()
    {
        return new FFRef<Vector3>(() => Bounds.localScale, (v) => { Bounds.localScale = v; });
    }
    // Use this for initialization
    void Start ()
    {
        movementSeq = action.Sequence();
        sizeSeq = action.Sequence();
    }

	// Update is called once per frame
	void Update ()
    {
        Vector3 newAveragePos = new Vector3();
        Vector3 topRight = new Vector3(float.NegativeInfinity, float.NegativeInfinity);
        Vector3 botLeft  = new Vector3(float.PositiveInfinity, float.PositiveInfinity);

        { // get Average Position
            for(int i = 0; i < targets.Length; ++i)
            {
                newAveragePos += new Vector3(
                    targets[i].position.x,
                    0,
                    targets[i].position.z);


                topRight.x = Mathf.Max(topRight.x, targets[i].position.x);
                topRight.y = Mathf.Max(topRight.y, targets[i].position.y);

                botLeft.x = Mathf.Min(botLeft.x, targets[i].position.x);
                botLeft.y = Mathf.Min(botLeft.y, targets[i].position.y);
            }
            newAveragePos /= 3.0f;
            newAveragePos.y += 10.0f;
            newAveragePos += offset;
        }
        
        // Adjust Camera Position
        if((newAveragePos-targetPosition).magnitude > PositionLeash)
        { // Update movementSequence
            targetPosition = newAveragePos;
            movementSeq.ClearSequence();
            movementSeq.Property(ffposition, targetPosition, FFEase.E_SmoothEnd, AdjustmentTime);
        }
        
        float cameraSize = Mathf.Clamp(
            Mathf.Max(topRight.x - botLeft.x, topRight.y - botLeft.y),
            MinCameraSize,
            MaxCameraSize);

        if (Mathf.Abs(cameraSize - targetSize) > SizeLeash)
        { // Update SizeSequence
            targetSize = cameraSize;
            Vector3 scaleSize = new Vector3(
                1.0f + ((targetSize / MinCameraSize)-1)*2.0f,
                1.0f + ((targetSize / MinCameraSize)-1)*2.0f,
                1.0f + ((targetSize / MinCameraSize)-1)*2.0f);

            sizeSeq.ClearSequence();
            sizeSeq.Property(RefCameraSize(), targetSize, FFEase.E_SmoothEnd, AdjustmentTime);
            sizeSeq.Property(ffscale, scaleSize, FFEase.E_SmoothEnd, AdjustmentTime);
        }

	}
}
