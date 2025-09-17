using Unity.VisualScripting;
using UnityEngine;

public class CharacterInteract : MonoBehaviour, IInteractable
{   
    public NodeParser NodeParser;

    [Tooltip("Dialogue graph that will open upon interaction")]
    public DialogueGraph currentGraph;

    //currently, once you have finished speaking to a character, you cannot speak with them again.
    //I will change this to where the character goes through an list of dialogue trees, and once you reach the last one 
    //it will continue to play the very last tree in the list.
    private bool hasSpoken;
    public bool loopDialogue;

    public bool CanInteract()
    {
        if (hasSpoken && !loopDialogue)
        {
            return false;
        }
        return true;
    }

    public bool Interact(Interactor _interactor)
    {
        Debug.Log("Dialogue starting! Starting graph ", currentGraph);
        NodeParser.StartDialogue(currentGraph);
        hasSpoken = true;
        return true;

    }

    public void Start()
    {
        hasSpoken = false;
    }
}


