using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    /// <summary>
    /// This simple script makes the player overhead UI always face the camera
    /// </summary>

    void Update()
    {
        // Make sure we've got a camera
        if (Camera.main != null)
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }
    }
}
