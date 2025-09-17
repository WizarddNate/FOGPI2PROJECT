using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [Tooltip("The distance of the raycast")]
    [SerializeField]
    public float castDistance = 15f;

    [Tooltip("The raycasts offset from the player")]
    [SerializeField]
    private Vector3 raycastOffset = new Vector3(0.5f, 0.5f, 0);

    public GameObject interactDisplay;

    InputAction interactAction;

    private void Start()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
        interactDisplay.SetActive(false);
    }

    private void Update()
    {
        if (InteractionTest(out IInteractable interactable))
        {
            if (interactable.CanInteract())
            {
                //overhead ui shows when a player can interact with something
                interactDisplay.SetActive(true);

                if (interactAction.WasPressedThisFrame())
                {
                    interactable.Interact(this);

                    interactDisplay.SetActive(false);
                }
            }
        }
        else
        {
            //the overhead ui is hidden when not able to interact with an object
            interactDisplay.SetActive(false);
        }
    }

    //determines if you are able to interact with an object by sending out a raycast
    private bool InteractionTest(out IInteractable _interactable)
    {
        _interactable = null;

        Ray ray = new Ray(transform.position + raycastOffset, transform.forward);

        //shows the size of the ray - debug tool
        Debug.DrawRay(ray.origin, ray.direction, Color.green, 3f);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, castDistance))
        {
            _interactable = hitInfo.collider.GetComponent<IInteractable>();

            if (_interactable != null)
            {
                return true;
            }

            return false;
        }

        return false;
    }
}
