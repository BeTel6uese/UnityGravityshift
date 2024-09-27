using UnityEngine;

public class freeLookCamera : MonoBehaviour
{
    public Transform player;             
    public float mouseSensitivity = 100f;
    public float distanceFromPlayer = 5f;  
    public float minDistanceFromPlayer = 1f; 
    public float verticalRotationLimit = 80f;
    public LayerMask collisionLayers;    

    private float pitch = 0f;
    private float yaw = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  // lock the cursor to the center of the screen
    }

    private void LateUpdate()
    {
        // get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // update yaw and pitch
        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -verticalRotationLimit, verticalRotationLimit);  // clamp vertical rotation to prevent full flips

        // set camera rotation based on yaw and pitch
        transform.eulerAngles = new Vector3(pitch, yaw, 0f);

        Vector3 desiredCameraPosition = player.position - transform.forward * distanceFromPlayer;

        // perform a raycast to detect collision between the player and the camera
        RaycastHit hit;
        if (Physics.Raycast(player.position, -transform.forward, out hit, distanceFromPlayer, collisionLayers))
        {
            // adjust the camera position to be in front of the object it hit
            float hitDistance = Mathf.Clamp(hit.distance, minDistanceFromPlayer, distanceFromPlayer);
            transform.position = player.position - transform.forward * hitDistance;
        }
        else
        {
            // if no collision, set the camera to its desired position
            transform.position = desiredCameraPosition;
        }
    }
}
