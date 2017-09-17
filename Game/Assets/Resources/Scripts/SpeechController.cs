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

    public void SayThing(FFAction.ActionSequence dialogSeq, string text, float time)
    {
        transform.Find("Text").GetComponent<TextMesh>().text = text;

        var textColor = TextColor();
        var bubbleColor = BubbleColor();

        var newTextColor = textColor.Val;
        var newBubbleColor = bubbleColor.Val;
        newTextColor.a = 1.0f;
        newBubbleColor.a = 1.0f;

        // Set starting color
        dialogSeq.Property(bubbleColor, newBubbleColor, SpeechBubbleTransition, time);
        dialogSeq.Property(textColor, newTextColor, SpeechBubbleTransition, time);

        dialogSeq.Sync();
    }


}
