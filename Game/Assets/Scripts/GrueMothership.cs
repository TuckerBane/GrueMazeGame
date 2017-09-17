using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrueMothership : FFComponent
{

    public GameObject mGruelingPrefab;
    public float mMinSpawnPeriod = 5.0f;
    public float mMaxSpawnPeriod = 10.0f;
    public int mMinWaveNumber = 2;
    public int mMaxWaveNumber = 4;

    public float mMaxFlingDistanceX = 20;
    public float mMaxFlingDistanceY = 20;

    public float mGruelingFlingSpeed = 5.0f;

    public GameObject mCryingPlace;
    public bool mCrying = false;

    public bool mGoCryHackSwitch = false;
    public bool mBeCuredHackSwitch = false;
    FFAction.ActionSequence mMyActionList;

	// Use this for initialization
	void Start () {
        mMyActionList = action.Sequence();
        mMyActionList.Call(SpawnFunction);
    }
	
    IEnumerator SendOutGrueling(GameObject grueling)
    {
        Vector3 endPos = grueling.transform.position + new Vector3( Random.Range(-mMaxFlingDistanceX, mMaxFlingDistanceX), 0, Random.Range(-mMaxFlingDistanceY, mMaxFlingDistanceY) );

        while (GameMath.DistanceBetween(grueling, endPos) > 2.0f) {
            Vector3 goalDur = GameMath.UnitVectorBetween(grueling, endPos);

            grueling.transform.position += goalDur * Time.deltaTime * mGruelingFlingSpeed;
            yield return null;
        }
        yield return null;
    }

    IEnumerator GoCry()
    {
        Vector3 endPos = mCryingPlace.transform.position;

        while (GameMath.DistanceBetween(this, endPos) > 2.0f)
        {
            Vector3 goalDur = GameMath.UnitVectorBetween(this, endPos);

            transform.position += goalDur * Time.deltaTime * mGruelingFlingSpeed;
            yield return null;
        }
        yield return null;
    }


    void SpawnFunction()
    {
        mMyActionList.Delay(Random.Range(mMinSpawnPeriod, mMaxSpawnPeriod));
        mMyActionList.Sync();
        

        for(int unitsToSpawn = Random.Range(mMinWaveNumber, mMaxWaveNumber); unitsToSpawn > 0; --unitsToSpawn)
        {
            GameObject newLing = Instantiate(mGruelingPrefab,transform.position,transform.rotation);
            newLing.GetComponent<GrueLogic>().mStunTime = 10.0f;
            StartCoroutine("SendOutGrueling", newLing);
            //newLing.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-mMaxGoSpeedX, mMaxGoSpeedX), 0, Random.Range(-mMaxGoSpeedY, mMaxGoSpeedY));
        }

        mMyActionList.Call(SpawnFunction);
        mMyActionList.Sync();
    }


	// Update is called once per frame
	void Update () {

        float lightLevel = FindObjectOfType<GlobalLightChecker>().GetTotalLightLevel(transform);
        if( (!mCrying && lightLevel >= 1.5f) || (!mCrying && mGoCryHackSwitch) )
        {
            CustomDialogOn cd;
            cd.tag = "ScareAway";
            var box = FFMessageBoard<CustomDialogOn>.Box("ScareAway");
            box.SendToLocal(cd);

            mCrying = true;
            mMyActionList.ClearSequence();
            GetComponent<ExPathFollower>().enabled = false;
            StartCoroutine("GoCry");
        }
        else if( (mCrying && GameMath.DistanceBetween(this,mCryingPlace) <= 3.0f && lightLevel >= 2.5f ) || mBeCuredHackSwitch)
        {
            var box = FFMessageBoard<CustomDialogOn>.Box("Rescue");
            box.SendToLocal(new CustomDialogOn());

            // Become human?
            Destroy(this);
        }

	}
}
