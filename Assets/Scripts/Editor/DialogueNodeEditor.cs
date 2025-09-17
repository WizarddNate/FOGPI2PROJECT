using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;
using System.Linq;


/// <summary>
/// The GUI overrides on the dialogue node. GUI will not be visable unless included in the override
/// </summary>

[CustomNodeEditor(typeof(DialogueNode))]
public class DialogueNodeEditor : NodeEditor
{
    private DialogueNode dialogueNode;

    //override dialogue node gui
    public override void OnBodyGUI()
    {
        if (dialogueNode == null)
        {
            dialogueNode = target as DialogueNode;
        }

        //create button
        /*
        if (GUILayout.Button("Toggle Dialogue Choice"))
        {
            
        }
        */

        //show data from dialogue node
        //entry node 
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("entry"));

        //sprite
        EditorGUIUtility.labelWidth = 150;
        dialogueNode.sprite = (Sprite)EditorGUILayout.ObjectField("Character Sprite", dialogueNode.sprite, typeof(Sprite), false);

        //name
        dialogueNode.speakerName = EditorGUILayout.TextField("Speaker", dialogueNode.speakerName);

        //dialogue line
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("dialogueLine"));

        //exit node
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("exit"));

        //update xnode graph
        serializedObject.Update();
    }
}
