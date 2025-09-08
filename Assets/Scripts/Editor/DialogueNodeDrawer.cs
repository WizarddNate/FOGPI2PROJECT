using UnityEngine;
using UnityEditor;
using XNode;
using XNodeEditor;
using System.Linq;

//this is for potentionally customizing the dialogue node boxes in the future
//not needed right now though


[CustomNodeEditor(typeof(DialogueNode))]
public class DialogueNodeDrawer : NodeEditor
{
    /*
    private DialogueNode dialogueNode;
    public override void OnBodyGUI()
    {
        if (dialogueNode == null)
        {
            dialogueNode = target as DialogueNode;
        }

        serializedObject.Update();
    }
    */
}
