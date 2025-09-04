using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //Interact with objects, continue dialogue
    public InputAction InteractAction;
    public bool canClick;
    public bool isActivated;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InteractAction = InputSystem.actions.FindAction("Interact");

        canClick = true;
        isActivated = false;
    }

    private void Update()
    {
        if (InteractAction.IsPressed())
        {
            if (canClick)
            {
                isActivated = true;
                canClick = false;
                StartCoroutine(ClickCooldown());
            }
        }
    }

    //stop click from triggering multiple times
    IEnumerator ClickCooldown()
    {
        isActivated = false;
        yield return new WaitForSeconds(10);
        canClick = true;
    }
}
