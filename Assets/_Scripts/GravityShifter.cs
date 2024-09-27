using System.Collections;
using UnityEngine;

public class GravityShifter : MonoBehaviour
{
    public Transform playerTransform;
    public Transform worldTransform; 
    public float maxRaycastDistance = 10f;
    public float rotationSpeed = 1f; 
    public LayerMask wallMask; 
    public float rotationAngle = 90f; 

    public GameObject[] holograms; 
    private int selectedHologramIndex = -1; 

    private bool isRotating = false; 
    private Vector3 targetNormal; // the normal of future ground

    void Update()
    {
        if (!isRotating) 
        {
            
            if (Input.GetKeyDown(KeyCode.F)) // Front hologram
            {
                ToggleHologram(0);
            }
            if (Input.GetKeyDown(KeyCode.E)) // Left hologram
            {
                ToggleHologram(1);
            }
            if (Input.GetKeyDown(KeyCode.R)) // Back hologram
            {
                ToggleHologram(2);
            }
            if (Input.GetKeyDown(KeyCode.Q)) // Right hologram
            {
                ToggleHologram(3);
            }

            // confirm using C
            if (Input.GetKeyDown(KeyCode.C) && selectedHologramIndex != -1)
            {
                Vector3 direction = GetDirectionFromHologramIndex(selectedHologramIndex);
                SelectWall(direction);
            }
        }
    }

    void ToggleHologram(int index)
    {      
        if (selectedHologramIndex == index && holograms[index].activeSelf)
        {
            holograms[index].SetActive(false);
            selectedHologramIndex = -1; 
        }
        else
        {          
            for (int i = 0; i < holograms.Length; i++)
            {
                holograms[i].SetActive(i == index); 
            }
            selectedHologramIndex = index;
        }
    }

    Vector3 GetDirectionFromHologramIndex(int index)
    {
        // return the direction based on the selected hologram
        switch (index)
        {
            case 0: return playerTransform.forward;    // Front wall
            case 1: return -playerTransform.right;     // Left wall
            case 2: return -playerTransform.forward;   // Back wall
            case 3: return playerTransform.right;      // Right wall
            default: return Vector3.zero;              // default -no movement-
        }
    }

    void SelectWall(Vector3 relativeDirection)
    {
        // using ray cast
        Ray ray = new Ray(playerTransform.position, relativeDirection);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxRaycastDistance, wallMask))
        {        
            targetNormal = hit.normal; 
            RotateWorldAroundPlayer(); 
        }
    }

    void RotateWorldAroundPlayer()
    {
        if (!isRotating)
        {
            isRotating = true; 

            // calculating the axis of rotation
            Vector3 rotationAxis = Vector3.Cross(playerTransform.up, targetNormal).normalized;

            // start the smooth rotation 
            StartCoroutine(SmoothRotateWorld(rotationAxis));
        }
    }

    IEnumerator SmoothRotateWorld(Vector3 rotationAxis)
    {
        float t = 0f; // for interpolation
        float initialRotation = 0f; 
        float targetRotation = rotationAngle; 

        // rotate the world around the player using Mathf.Lerp 
        while (t < 1f)
        {
            t += Time.deltaTime * rotationSpeed; 
            float currentAngle = Mathf.Lerp(initialRotation, targetRotation, t);
            worldTransform.RotateAround(playerTransform.position, rotationAxis, currentAngle - initialRotation); //
            initialRotation = currentAngle; 

            yield return null; 
        }

        worldTransform.RotateAround(playerTransform.position, rotationAxis, targetRotation - initialRotation);
       
        deactivateAllHolograms();

        isRotating = false; 
    }

    void deactivateAllHolograms()
    {
        
        for (int i = 0; i < holograms.Length; i++)
        {
            holograms[i].SetActive(false);
        }
        selectedHologramIndex = -1; 
    }
}
