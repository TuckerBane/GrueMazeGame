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

    public float mMaxGoSpeedX = 20;
    public float mMaxGoSpeedY = 20;

    public GameObject mCryingPlace;
    public bool mCrying = false;

    FFAction.ActionSequence mMyActionList;

	// Use this for initialization
	void Start () {
        mMyActionList = action.Sequence();
        mMyActionList.Call(SpawnFunction);
    }
	
    void SpawnFunction()
    {
        mMyActionList.Delay(Random.Range(mMinSpawnPeriod, mMaxSpawnPeriod));
        mMyActionList.Sync();
        

        for(int unitsToSpawn = Random.Range(mMinWaveNumber, mMaxWaveNumber); unitsToSpawn > 0; --unitsToSpawn)
        {
            GameObject newLing = Instantiate(mGruelingPrefab);
            newLing.GetComponent<GrueLogic>().mStunTime = 10.0f;
            newLing.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-mMaxGoSpeedX, mMaxGoSpeedX), 0, Random.Range(-mMaxGoSpeedY, mMaxGoSpeedY));
        }

        mMyActionList.Call(SpawnFunction);
        mMyActionList.Sync();
    }

    void GoCry()
    {
        GetComponent<ExPathFollower>().enabled = false;
    }

	// Update is called once per frame
	void Update () {

        float lightLevel = GetComponent<GlobalLightChecker>().GetTotalLightLevel(transform);
        if(!mCrying && lightLevel >= 1.5f)
        {
            mCrying = true;
            mMyActionList.ClearSequence();
        }

	}
}
