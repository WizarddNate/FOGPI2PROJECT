using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using XNode;
using static XNode.Node;

[NodeWidth(290)]
public class DialogueNode : BaseNode 
{

	//create the entry and exit points on the node in the graph editor
	[Input] public int entry;
	[Output] public int exit;

    public Sprite sprite;
    public string speakerName;
    //text area determines the gui area size for the text field (max, min)
    [TextArea(3,10)] public string dialogueLine;

    public override string GetString()
	{
		return "DialogueNode/" + speakerName + "/" + dialogueLine;
	}

    public override Sprite GetSprite()
    {
        return sprite;
    }
}


//add dialogue options
[System.Serializable]
public class DialogueOption
{
	public string choiceText;
    public string port;
    [Output] public DialogueOption dialogueOption;

}
