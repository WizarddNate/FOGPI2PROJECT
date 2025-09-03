using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;
using XNode;
using TMPro;
using UnityEngine.InputSystem;


public class NodeParser : MonoBehaviour
{
    public DialogueGraph graph;
    Coroutine _parser;

    public TMP_Text speaker;
    public TMP_Text dialogue;
    public Image speakerImage;

    private void Start()
    {
      foreach (BaseNode b in graph.nodes)
        {
            if (b.GetString() == "Start")
            {
                //make this node starting point
                graph.current = b;
                break;
            }
        }
        _parser = StartCoroutine(ParseNode());
    }
    
    IEnumerator ParseNode()
    {
        BaseNode b = graph.current;
        string data = b.GetString();
        string[] dataParts = data.Split('/');

        //make sure the first node is the start node
        if (dataParts[0] == "Start")
        {
            NextNode("exit");
        }
        //make this a switch later. Im just lazy and dont wanna do that right now
        //check that first index in the string array equals DialogueNode
        if (dataParts[0] == "DialogueNode")
        {
            //Run dialogue processing
            speaker.text = dataParts[1];
            dialogue.text = dataParts[2];
            speakerImage.sprite = b.GetSprite();
            yield return null;
            
        }
    }

    public void NextNode(string fieldName)
    {
        //find the post with this name;
        if (_parser != null)
        {
            StopCoroutine(_parser);
            _parser = null;
        }
        foreach (NodePort p in graph.current.Ports)
        {
            //Check if this port is the one we're looking for
            if (p.fieldName == fieldName)
            {
                graph.current = p.Connection.node as BaseNode;
                break;
            }
        }
        _parser = StartCoroutine(ParseNode());
    }

    //next node doesnt read until the player clicks
    private void OnMouseDown()
    {
        NextNode("exit");
    }
}
