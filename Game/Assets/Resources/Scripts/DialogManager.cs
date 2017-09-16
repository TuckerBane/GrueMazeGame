using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogManager : FFComponent
{
    List<CharacterDialog.Dialog> gameDialog = new List<CharacterDialog.Dialog>();

    public enum OratorNames
    {
        // @TODO ADD CHARACTER NAMES HERE

        None,

        

        Sammy,
        Bob,
        JackyChan,



    }
    [Serializable]
    public struct NameToTrans
    {
        public OratorNames name;
        public Transform trans;
    }
    public NameToTrans[] OratorMapping;

    Dictionary<OratorNames, Transform> Orators = new Dictionary<OratorNames, Transform>();

    FFAction.ActionSequence updateDialogSeq;


    // Use this for initialization
    void Start ()
    {
        updateDialogSeq = action.Sequence();
        
        // Add mapping to dictionary
        foreach(var mapping in OratorMapping)
        {
            if(mapping.name != OratorNames.None && mapping.trans != null)
            Orators.Add(mapping.name, mapping.trans);
        }

        // Add Dialogs
        {
            foreach(Transform child in transform)
            {
                var cd = child.GetComponent<CharacterDialog>();
                AddCharacterDialog(cd);

                if (cd != null)
                {
                    AddCharacterDialog(cd);
                }
            }
        }

        // Listen to events
        FFMessage<EnterParty>.Connect(OnEnterParty);
        FFMessage<LeaveParty>.Connect(OnLeaveParty);
        FFMessage<EnterArea>.Connect(OnEnterArea);
        FFMessage<LeaveArea>.Connect(OnLeaveArea);

        // Start update of dialogs

    }

    private void OnLeaveArea(LeaveArea e)
    {
        throw new NotImplementedException();
    }
    private void OnEnterArea(EnterArea e)
    {
        throw new NotImplementedException();
    }
    private void OnLeaveParty(LeaveParty e)
    {
        throw new NotImplementedException();
    }
    private void OnEnterParty(EnterParty e)
    {
        throw new NotImplementedException();
    }



    private void OnDestroy()
    {
        FFMessage<EnterParty>.Disconnect(OnEnterParty);
        FFMessage<LeaveParty>.Disconnect(OnLeaveParty);
        FFMessage<EnterArea>.Disconnect(OnEnterArea);
        FFMessage<LeaveArea>.Disconnect(OnLeaveArea);
    }

    void AddCharacterDialog(CharacterDialog characterDialog)
    {
        if (characterDialog == null)
            return;

        foreach (var dialog in characterDialog.dialogSets)
        {
            AddDialog(dialog);
        }
    }



    void AddDialog(CharacterDialog.Dialog dialog)
    {
        gameDialog.Add(dialog);
    }
	

    void UpdateDialog()
    {
        foreach(var dialog in gameDialog)
        {



        }

        updateDialogSeq.Delay(0.25f);
        updateDialogSeq.Sync();
        updateDialogSeq.Call(UpdateDialog);
    }

    // @ TODO. Make conditions work!
    // @ TODO. make Enter/Leave events tracked

    bool DialogConditionsTrue(CharacterDialog.Dialog dialog)
    {
        bool result = true;

        for(int i = 0; i < dialog.condition.Length; ++i)
        {
            switch (dialog.condition[i].type)
            {
                case CharacterDialog.Dialog.Condition.Type.InParty:

                    break;
                case CharacterDialog.Dialog.Condition.Type.Area:
                    break;
                case CharacterDialog.Dialog.Condition.Type.Custom:
                    break;
            }
        }

        return result;
    }
}
