using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using static XNode.Node;

[Serializable]
public class DialogueChoicesNode : BaseNode
{
    //create the entry and exit points on the node in the graph editor
    [Input] public int entry;
    [Output(dynamicPortList = true)] public List<string> answers;

    public Sprite sprite;
    public string speakerName;
    //text area determines the gui area size for the text field (max, min)
    [TextArea(3, 10)] public string dialogueLine;

    public override string GetString()
    {
        return "DialogueChoicesNode/" + speakerName + "/" + dialogueLine + "/" + answers[0];
    }

    public override Sprite GetSprite()
    {
        return sprite;
    }
}
