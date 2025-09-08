using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using XNode;

public class DialogueNode : BaseNode 
{

	//create the entry and exit points on the node in the graph editor
	[Input] public int entry;
	[Output] public int exit;

	public string speakerName;
	public string dialogueLine;
	public Sprite sprite;

	public bool dialogueOptions = false;

	public override string GetString()
	{
		return "DialogueNode/" + speakerName + "/" + dialogueLine;
	}

    public override Sprite GetSprite()
    {
        return sprite;
    }
}

[System.Serializable]
public class DialogueOption
{

}