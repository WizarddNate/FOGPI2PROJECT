using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using XNode;


public class NodeParser : MonoBehaviour
{
    [Header("Dialogue Graph")]
    public DialogueGraph graph;
    private DialogueChoicesNode activeSegment;
    private string answer;
    Coroutine _parser;

    [Header("UI Elements")]
    public GameObject dialogueContainer;
    public TMP_Text speaker;
    public TMP_Text dialogue;
    public Image speakerImage;
    public GameObject buttonContainer;
    public Transform buttonParent;
    public GameObject buttonPrefab;

    [Header("Player Control")]
    public InputManager inputManager;
    public PlayerController playerController;
    public Interactor interactor;

    public void StartDialogue(DialogueGraph currentGraph)
    {
        graph = currentGraph;

        //open dialogue UI 
        Debug.Log("open dialogue");
        dialogueContainer.gameObject.SetActive(true);

        //Stop player from walking away!
        playerController.enabled = false;
        //TEMPORARY way to stop the interaction system from firing while talking
        interactor.castDistance = 0;

        //loop through nodes in graph to find start node
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

    public void AnswerClicked(int clickedIndex)
    {
        buttonContainer.SetActive(false);
        BaseNode b = graph.current;
        var port = activeSegment.GetPort("answers " + clickedIndex);

        if (port.IsConnected)
        {
            graph.current = port.Connection.node as BaseNode;
            _parser = StartCoroutine(ParseNode());
        }
        else
        {
            Debug.LogError("ERROR: ChoiceDialogue port is not connected");
        }
    }

    //create choice buttons
    private void UpdateDialogue(DialogueChoicesNode newSegment)
    {
        activeSegment = newSegment;
        dialogue.text = newSegment.dialogueLine;
        speaker.text = newSegment.speakerName;
        int answerIndex = 0;
        
        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var answer in newSegment.answers)
        {
            var button = Instantiate(buttonPrefab, buttonParent); //create button prefab
            button.GetComponentInChildren<TextMeshProUGUI>().text = answer;

            var index = answerIndex;
            //trigger AnswerClicked on button click
            button.GetComponentInChildren<Button>().onClick.AddListener((() => { AnswerClicked(index); }));
            answerIndex++;
        }

    }

    IEnumerator ParseNode()
    {
        //split string up into a string list
        BaseNode b = graph.current;
        string data = b.GetString();
        string[] dataParts = data.Split('/');

        //reset textbox
        speaker.text = "";
        dialogue.text = "";
        speakerImage.sprite = null;

        foreach (Transform child in buttonParent) //destroys buttons before going to next node
        { 
            Destroy(child.gameObject);
        }

        //make this a switch later. Im just lazy and dont wanna do that right now
        //if start node, just keep going
        if (dataParts[0] == "Start")
        {
            //next node
            NextNode("exit");
        }
        if (dataParts[0] == "DialogueChoicesNode")
        {
            //wait a split second before continuing. Prevents skipping through multiple nodes at once.
            yield return new WaitForSeconds(0.4f);

            buttonContainer.SetActive(true);

            speaker.text = dataParts[1];
            dialogue.text = dataParts[2];
            speakerImage.sprite = b.GetSprite();

            UpdateDialogue(b as DialogueChoicesNode); //instantiates the buttons
        }
        if (dataParts[0] == "DialogueNode")
        {
            //wait a split second before continuing. Prevents skipping through multiple nodes at once.
            yield return new WaitForSeconds(0.4f);

            //assign data from string list to ui parts
            speaker.text = dataParts[1];
            dialogue.text = dataParts[2];
            speakerImage.sprite = b.GetSprite();

            //wait until mouse click to continue to next node
            Debug.Log("waiting for mouse click...");
            yield return new WaitUntil(() => inputManager.InteractAction.IsPressed());
            
            //continue to next node.

            //This assumes there will only be one exit node and goes to that
            //instead, we need a way to select multiple choices and go to the node it connect to instead

            Debug.Log("Click!");
            NextNode("exit"); 
        }
        //if this is the end node, end dialogue
        if (dataParts[0] == "End")
        {
            CloseDialogue();
        }
    }

    //i dont.. really understand what we're doing here...
    public void NextNode(string fieldName)
    {
        //stop the coroutine
        if (_parser != null)
        {
            StopCoroutine(_parser);
            _parser = null;
        }

        foreach (NodePort p in graph.current.Ports)
        {
            //Check if this exit port is the one we're looking for
            if (p.fieldName == fieldName)
            {
                graph.current = p.Connection.node as BaseNode;
                break;
            }
        }
        _parser = StartCoroutine(ParseNode());
    }

    private void CloseDialogue()
    {
        Debug.Log("close dialogue");

        //hide dialogue
        dialogueContainer.gameObject.SetActive(false);

        //Let player move again
        playerController.enabled = true;

        //let the character interact with things again 
        interactor.castDistance = 15f;
    }
}
