using Unity.VisualScripting;
using UnityEngine;

public class CharacterInteract : MonoBehaviour
{
    public NodeParser NodeParser;
    public bool hasInteracted;

    private void Start()
    {
        hasInteracted = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player")) && (hasInteracted == false))
        {
            NodeParser.StartDialogue();
            hasInteracted = true;
        }
    }
/*
public NodeParser NodeParser;
    public bool hasSpoken;

    public bool CanInteract()
{
    return true;
}

public bool Interact(Interactor _interactor)
{
    if (hasSpoken)
    {
        return false;
    }
    else
    {
        NodeParser.StartDialogue();
        return true;
    }
}
*/
}


