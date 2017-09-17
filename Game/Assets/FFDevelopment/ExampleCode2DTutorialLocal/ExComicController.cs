using UnityEngine;
using System.Collections;
using System;
/*
 * This is an example script which shows how
 * you might make a comic reader script to move
 * around the camera. This layout uses the FFAction
 * to make a state machine of sorts.
 */

public class ExComicController : FFComponent {

    FFAction.ActionSequence movementSeq;
    FFAction.ActionSequence audioSeq;

    float distAlongPath = 0;

    [Range(0.1f,100.0f)]
    public float ComicCameraSpeed;
    
    int currentPointNumber = 0;
    public FFPath PathToFollow;

    [Serializable]
   public struct AudioCue
    {
        public float delay;
        public AudioClip clip;
    }


    public AudioCue[] pathPointAudioClip;

	// Use this for initialization
	void Start () {
        if(PathToFollow != null && PathToFollow.points.Length > 1)
        {
            movementSeq = action.Sequence();
            audioSeq = action.Sequence();

            transform.position = PathToFollow.PointAlongPath(PathToFollow.LengthAlongPathToPoint(currentPointNumber));
            movementSeq.Call(WaitForInput);
        }
        else
        {
            if (PathToFollow != null)
                Debug.Log("ExComicController has a path which has " + PathToFollow.points.Length + " points which is invalid.");
            else
                Debug.Log("EXComicController has no path to follow");
        }
	}

    // Move forward state
    void MoveForward()
    {
        float lengthToNextPoint = PathToFollow.LengthAlongPathToPoint(currentPointNumber);
        if (distAlongPath >= lengthToNextPoint) // reached next point
        {
            transform.position = PathToFollow.PointAlongPath(lengthToNextPoint); // goto point

            movementSeq.Call(WaitForInput); // waitForInput
            return;
        }
        else // keep moving along path
        {
            transform.position = PathToFollow.PointAlongPath(distAlongPath);
        }

        distAlongPath += Time.deltaTime * ComicCameraSpeed;
        movementSeq.Sync();
        movementSeq.Call(MoveForward);
    }

    // Move Backward state
    void MoveBackward()
    {
        float lengthToPrevPoint = PathToFollow.LengthAlongPathToPoint(currentPointNumber);
        if (distAlongPath <= lengthToPrevPoint) // reached begining
        {
            transform.position = PathToFollow.PointAlongPath(lengthToPrevPoint); // goto point
            movementSeq.Call(WaitForInput); // waitForInput
            return;
        }
        else // keep moving along path
        {
            transform.position = PathToFollow.PointAlongPath(distAlongPath);
        }

        distAlongPath -= Time.deltaTime * ComicCameraSpeed;
        movementSeq.Sync();
        movementSeq.Call(MoveBackward);
    }

    // Wait For Input State
    void WaitForInput()
    {
        var goForard =
            Input.GetButtonUp("A" + 0) ||
            Input.GetButtonUp("A" + 1) ||
            Input.GetButtonUp("A" + 2) ||
            Input.GetKeyDown(KeyCode.RightArrow);

        if (goForard)  // go forwards
        {
            if (currentPointNumber < PathToFollow.points.Length)
            {
                ++currentPointNumber;
                movementSeq.Call(MoveForward);

                // Play Audio Clip
                if(currentPointNumber < pathPointAudioClip.Length &&
                   pathPointAudioClip[currentPointNumber].clip != null)
                {
                    if(pathPointAudioClip[currentPointNumber].delay != 0.0f)
                    {
                        audioSeq.Delay(pathPointAudioClip[currentPointNumber].delay);
                        audioSeq.Sync();
                    }
                    audioSeq.Call(PlayAudioClipAtPointIndex);
                }
                return;
            }
            else // finished Path, Goto Next level
            {
                TriggerFade tf;
                FFMessage<TriggerFade>.SendToLocal(tf);
            }
        }


        var gobackward=
            //Input.GetButtonUp("B" + 0) ||
            //Input.GetButtonUp("B" + 1) ||
            //Input.GetButtonUp("B" + 2) ||
            Input.GetKeyDown(KeyCode.LeftArrow);

        if (gobackward)  // go backwards
        {
            if (currentPointNumber > 0)
            {
                --currentPointNumber;
                movementSeq.Call(MoveBackward);
                return;
            }
        }

        movementSeq.Sync();
        movementSeq.Call(WaitForInput);
    }

    void PlayAudioClipAtPointIndex()
    {
        var audioSrc = GetComponent<AudioSource>();
        if (currentPointNumber < pathPointAudioClip.Length &&
           pathPointAudioClip[currentPointNumber].clip != null)
        {
            audioSrc.PlayOneShot(pathPointAudioClip[currentPointNumber].clip);
        }
    }
	
}
