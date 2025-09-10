using Unity.VisualScripting;
using UnityEngine;

public class CharacterInteract : MonoBehaviour, IInteractable
{   
    public NodeParser NodeParser;

    //currently, once you have finished speaking to a character, you cannot speak with them again.
    //I will change this to where the character goes through an list of dialogue trees, and once you reach the last one 
    //it will continue to play the very last tree in the list.
    public bool hasSpoken;

    public bool CanInteract()
    {
        if (hasSpoken)
        {
            return false;
        }
        return true;
    }

    public bool Interact(Interactor _interactor)
    {
        NodeParser.StartDialogue();
        hasSpoken = true;
        return true;

    }

    public void Start()
    {
        hasSpoken = false;
    }
}


