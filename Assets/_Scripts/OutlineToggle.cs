using UnityEngine;

public class OutlineToggle : MonoBehaviour
{
    public Material normalMaterial;   
    public Material outlineMaterial; 
    public LayerMask ignoreLayers;    

    private Renderer objRenderer;
    private Transform cameraTransform;

    void Start()
    {
        objRenderer = GetComponent<Renderer>();
        cameraTransform = Camera.main.transform; 
    }

    void Update()
    {    
        Ray ray = new Ray(cameraTransform.position, transform.position - cameraTransform.position);
        RaycastHit hit;

 
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~ignoreLayers))
        {
            if (hit.collider.gameObject == gameObject)
            {
                objRenderer.material = normalMaterial; 
            }
            else
            {
                objRenderer.material = outlineMaterial; 
            }
        }
    }
}
