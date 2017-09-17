using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterDialog : MonoBehaviour
{   
    [Serializable]
    public class Dialog
    {
        [Serializable]
        public struct Condition
        {
            public enum Type
            {
                InParty,
                Area,
                Custom,
            }
            public Type type;
            public bool invert;
            public string identifier;
        }

        public bool recurring;
        public bool blocking;
        public bool interupting;
        public Condition[] condition;
        public string[] echo;
        public string[] sideEffects;
    }

    public Dialog[] dialogSets;
}
