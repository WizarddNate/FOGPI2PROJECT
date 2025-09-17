using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using XNode;
using XNodeEditor;
using static XNodeEditor.NodeEditor;

[CustomNodeEditor(typeof(DialogueChoicesNode))]
public class DialogueChoicesNodeEditor : NodeEditor
{
    /// <summary>
    /// Choices node. Theres a built in hotkey that selects all nodes when you press 'A' and recenters all nodes when you press 'F'.
    /// This is built into xNode, but hopefully I can find a way to override it into 'ctrl+A' and 'ctrl+F'
    /// </summary>


    private DialogueChoicesNode dialogueChoicesNode;

    //override dialogue node gui
    public override void OnBodyGUI()
    {
        if (dialogueChoicesNode == null)
        {
            dialogueChoicesNode = target as DialogueChoicesNode;
        }

        serializedObject.Update();

        var segment = serializedObject.targetObject as DialogueChoicesNode;
 
        NodeEditorGUILayout.PortField(segment.GetPort("input"));


        //entry node
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("entry"));

        //sprite
        EditorGUIUtility.labelWidth = 150;
        dialogueChoicesNode.sprite = (Sprite)EditorGUILayout.ObjectField("Character Sprite", dialogueChoicesNode.sprite, typeof(Sprite), false);

        //name
        dialogueChoicesNode.speakerName = EditorGUILayout.TextField("Speaker", dialogueChoicesNode.speakerName);

        //dialogue line
        NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("dialogueLine"));

        //
        NodeEditorGUILayout.DynamicPortList(
            "answers", //field name
            typeof(string), //field type
            serializedObject, //make serializable
            NodePort.IO.Input, //new port
            Node.ConnectionType.Override, //port connection type
            Node.TypeConstraint.None,
            OnCreateReorderableList);

        foreach (XNode.NodePort dynamicPort in target.DynamicPorts)
        {
            if (NodeEditorGUILayout.IsDynamicPortListPort(dynamicPort)) continue;
            NodeEditorGUILayout.PortField(dynamicPort);
        }

        //update xnode graph
        serializedObject.ApplyModifiedProperties();
    }

    void OnCreateReorderableList(ReorderableList list)
    {
        list.elementHeightCallback = (int index) => { return 60; };

        // Override drawHeaderCallback to display node's name instead
        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var segment = serializedObject.targetObject as DialogueChoicesNode;

            NodePort port = segment.GetPort("answers " + index);

            segment.answers[index] = GUI.TextArea(rect, segment.answers[index]);


            if (port != null)
            {
                Vector2 pos = rect.position + (port.IsOutput ? new Vector2(rect.width + 6, 0) : new Vector2(-36, 0));
                NodeEditorGUILayout.PortField(pos, port);
            }
        };
    }
}
