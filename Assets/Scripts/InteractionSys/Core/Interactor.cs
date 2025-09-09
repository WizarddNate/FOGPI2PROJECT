using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField]
    private float castDistance = 5f;

    [SerializeField]
    private Vector3 raycastOffset = new Vector3(0, 1f, 0);

    InputAction interactAction;

    private void Start()
    {
        interactAction = InputSystem.actions.FindAction("Interact");
    }

    private void Update()
    {
        if (interactAction.WasPressedThisFrame())
        {
            if (InteractionTest(out IInteractable interactable))
            {
                if (interactable.CanInteract())
                {
                    //set up popup that tells player interaction is possible
                    interactable.Interact(this);
                }
            }
        }
    }

    private bool InteractionTest(out IInteractable _interactable)
    {
        _interactable = null;

        Ray ray = new Ray(transform.position + raycastOffset, transform.forward);

        //shows the size of the array
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
