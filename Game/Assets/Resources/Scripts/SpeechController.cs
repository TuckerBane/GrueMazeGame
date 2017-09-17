using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechController : FFComponent
{
    #region FFRef

    SpriteRenderer BubbleSprite()
    {
        return transform.Find("Bubble").GetComponent<SpriteRenderer>();
    }
    FFRef<Color> BubbleColor()
    {
        return new FFRef<Color>(() => BubbleSprite().color, (v) => { transform.Find("Bubble").GetComponent<SpriteRenderer>().color = v; });
    }
    FFRef<Color> TextColor()
    {
        return new FFRef<Color>(() => transform.Find("Text").GetComponent<TextMesh>().color, (v) => { transform.Find("Text").GetComponent<TextMesh>().color = v; });
    }

    #endregion

    public AnimationCurve SpeechBubbleTransition;

    
    class TextItem
    {
        public FFAction.ActionSequence dialogSeq;
        public string text;
        public float time;
    }


    public void SayThing(FFAction.ActionSequence dialogSeq, string text, float time)
    {
        Debug.Log("Say Thing");

        TextItem item = new TextItem();
        item.dialogSeq = dialogSeq;
        item.text = text;
        item.time = time;

        dialogSeq.Sync();
        dialogSeq.Call(Say, item);
    }

    void Say(object textObj)
    {
        TextItem item = (TextItem)textObj;

        // Get text Item to set text
        transform.Find("Text").GetComponent<TextMesh>().text = item.text;

        var textColor = TextColor();
        var bubbleColor = BubbleColor();

        var newTextColor = textColor.Val;
        var newBubbleColor = bubbleColor.Val;
        newTextColor.a = 1.0f;
        newBubbleColor.a = 1.0f;

        // Set starting color
        item.dialogSeq.Property(bubbleColor, newBubbleColor, SpeechBubbleTransition, item.time);
        item.dialogSeq.Property(textColor, newTextColor, SpeechBubbleTransition, item.time);
        item.dialogSeq.Sync();
    }


}
