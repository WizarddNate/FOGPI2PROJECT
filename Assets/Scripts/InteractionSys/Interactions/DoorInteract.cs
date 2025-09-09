using UnityEngine;

public class DoorInteract : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Vector3 targetRotation = new Vector3(0, -100f, 0);

    [SerializeField]
    private float rotationSpeed = 3f;

    private bool isOpen = false;

    public bool CanInteract()
    {
        return true;
    }

    public bool Interact(Interactor _interactor)
    {
        if (isOpen)
        {
            transform.Rotate(-targetRotation, rotationSpeed, Space.World);
        }
        else
        {
            transform.Rotate(targetRotation, rotationSpeed, Space.World);
        }

        isOpen = !isOpen;
        return true;
    }

}
