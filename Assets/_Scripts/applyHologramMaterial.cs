using UnityEngine;

public class applyHologramMaterial : MonoBehaviour
{
    [SerializeField]
    private Material hologramMaterial;

    void Start()
    {
        // get everything in the character and its children
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        // apply the hologram material to all
        foreach (Renderer rend in renderers)
        {
            rend.material = hologramMaterial;
        }
    }
}