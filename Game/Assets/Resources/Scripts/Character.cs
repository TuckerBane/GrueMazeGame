using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


#region Events
public class QueryLight
{
    public QueryLight(Vector3 inPoint)
    {
        in_point = inPoint;
    }

    public Vector3 in_point;
    public float out_intensity = 0.0f;
}
public class QueryParty
{
    public QueryParty(string inCharacterName)
    {
        in_characterName = inCharacterName;
        out_character = null;
    }
    public string in_characterName;
    public Character out_character;
}



public struct Transformation
{
    public enum Type
    {
        Human,
        Grue,
    }
    public Type transformationType;
    public Character character;
}

public struct EnterParty
{
    public Character character;
}
public struct LeaveParty
{
    public Character character;
}
public struct EnterArea
{
    public Transform area;
}
public struct LeaveArea
{
    public Transform area;
}



struct CustomDialogOn
{
    public string tag;
}
struct CustomDialogOff
{
    public string tag;
}
#endregion


public class Character : FFComponent
{
    public enum Type
    {
        Player,
        NPC,
        Grue,
    }
    public enum Personality
    {
        Aggressive,
        Passive
    }

    [Serializable]
    public struct Details
    {
        public string Name;
        public Personality personality;
        public float personalityStrength;
    }
    public Details details;
    
    float QueryLight()
    {
        QueryLight lightVal = new QueryLight(transform.position);
        FFMessage<QueryLight>.SendToLocal(lightVal);

        return lightVal.out_intensity;
    }

    public Transform GetSpeachRoot()
    {
        return transform.Find("SpeechRoot");
    }
    public SpeechController GetSpeechController()
    {
        return transform.Find("SpeechRoot").GetComponent<SpeechController>();
    }

    //@ TODO uncomment the player controller
    public void TransformCharacter(Type type)
    {
        //if (GetComponent<PlayerController>()) Destroy(GetComponent<PlayerController>());
        if (GetComponent<NPCController>()) Destroy(GetComponent<NPCController>());
        if (GetComponent<GrueController>()) Destroy(GetComponent<GrueController>());


        if (type == Type.Player)
        {
            //var pc = gameObject.AddComponent<PlayerController>();
        }
        else if(type == Type.NPC)
        {
            var npcc = gameObject.AddComponent<NPCController>();
        }
        else
        {
            var gc = gameObject.AddComponent<GrueController>();
        }
    }



    // Update is called once per frame
    void Update () {
		
	}
}
