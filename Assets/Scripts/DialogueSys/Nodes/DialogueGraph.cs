using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

/// <summary>
/// Allows you to create a node graph  
/// </summary>

[CreateAssetMenu]
public class DialogueGraph : NodeGraph {

	public BaseNode current;


    //add a command to the context menu. Neat
    [ContextMenu("Say Hi!")]
    void SayHi()
    {
        Debug.Log(">hi!!");
    }
}