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

        Sam,
        Alex,
        Max,
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
    FFAction.ActionSequence dialogSequence;


    // Use this for initialization
    void Start ()
    {
        updateDialogSeq = action.Sequence();
        dialogSequence = action.Sequence();

        // Add mapping to dictionary
        foreach (var mapping in OratorMapping)
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
        UpdateDialog();
    }

    Dictionary<string, bool> PartyStatus = new Dictionary<string, bool>();
    Dictionary<string, bool> AreaStatus = new Dictionary<string, bool>();
    Dictionary<string, bool> CustomStatus = new Dictionary<string, bool>();

    private void OnLeaveArea(LeaveArea e)
    {
        if(AreaStatus.ContainsKey(e.area.name))
        {
            AreaStatus[e.area.name] = false;
        }
    }
    private void OnEnterArea(EnterArea e)
    {
        if (AreaStatus.ContainsKey(e.area.name))
        {
            AreaStatus[e.area.name] = true;
        }
        else
        {
            AreaStatus.Add(e.area.name, true);
        }
    }

    private void OnLeaveParty(LeaveParty e)
    {
        if (PartyStatus.ContainsKey(e.character.name))
        {
            PartyStatus[e.character.name] = false;
        }
    }
    private void OnEnterParty(EnterParty e)
    {
        if (PartyStatus.ContainsKey(e.character.name))
        {
            PartyStatus[e.character.name] = true;
        }
        else
        {
            PartyStatus.Add(e.character.name, true);
        }
    }
    
    private void OnCustomDialogOff(CustomDialogOff e)
    {
        if (CustomStatus.ContainsKey(e.tag))
        {
            CustomStatus[e.tag] = false;
        }
    }
    private void OnCustomDialogOn(CustomDialogOn e)
    {
        if (CustomStatus.ContainsKey(e.tag))
        {
            CustomStatus[e.tag] = true;
        }
        else
        {
            CustomStatus.Add(e.tag, true);
        }
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

        foreach(var cond in dialog.condition)
        {
            if(cond.type == CharacterDialog.Dialog.Condition.Type.Custom)
            {
                FFMessageBoard<CustomDialogOn> .Box(cond.identifier).Connect(OnCustomDialogOn);
                FFMessageBoard<CustomDialogOff>.Box(cond.identifier).Connect(OnCustomDialogOff);
            }
        }
    }
    
    void UpdateDialog()
    {
        foreach(var dialog in gameDialog)
        {
            if(DialogConditionsTrue(dialog))
            {
                QueueDialog(dialog);
                QueueSideEffects(dialog);
            }


        }

        updateDialogSeq.Delay(0.25f);
        updateDialogSeq.Sync();
        updateDialogSeq.Call(UpdateDialog);
    }

    // @ TODO. Make conditions work!
    // @ TODO. make Enter/Leave events tracked

    bool DialogConditionsTrue(CharacterDialog.Dialog dialog)
    {
        for(int i = 0; i < dialog.condition.Length; ++i)
        {
            bool conditionMet = false;
            switch (dialog.condition[i].type)
            {
                case CharacterDialog.Dialog.Condition.Type.InParty:
                    conditionMet = PartyStatus.ContainsKey(dialog.condition[i].identifier);
                    break;
                case CharacterDialog.Dialog.Condition.Type.Area:
                    conditionMet = AreaStatus.ContainsKey(dialog.condition[i].identifier);
                    break;
                case CharacterDialog.Dialog.Condition.Type.Custom:
                    conditionMet = CustomStatus.ContainsKey(dialog.condition[i].identifier);
                    break;
            }

            if (dialog.condition[i].invertCondition)
                conditionMet = !conditionMet;

            if(!conditionMet)
                return false;
        }

        return true;
    }

    public float averageLengthPerWord = 5.0f;
    public float displayTimePerWord = 95.0f / 60.0f;
    public float minDisplayTime = 1.45f;

    void QueueDialog(CharacterDialog.Dialog dialog)
    {
        for(int i = 0; i < dialog.conversation.Length; ++i)
        {
            var echoDisplayTime = Mathf.Max(minDisplayTime, ((float)dialog.conversation[i].text.Length / averageLengthPerWord) * displayTimePerWord);
            var orator = dialog.conversation[i].orator;
            var text = dialog.conversation[i].text;


            var speechRoot = Orators[orator].GetComponent<Character>().GetSpeachRoot();
            var speechController = speechRoot.GetComponent<SpeechController>();

            speechController.SayThing(dialogSequence, text, echoDisplayTime);
        }
    }
    void QueueSideEffects(CharacterDialog.Dialog dialog)
    {
        for (int i = 0; i < dialog.sideEffects.Length; ++i)
        {
            dialogSequence.Call(SendCustomDialogEvent, dialog.sideEffects[i]);
        }
    }

    void SendCustomDialogEvent(object text)
    {
        var box = FFMessageBoard<CustomDialogOn>.Box((string)text);
        CustomDialogOn cdo;
        cdo.tag = (string)text;
        box.SendToLocal(cdo);
    }
}
