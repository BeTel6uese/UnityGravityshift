using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;                 
    public float speed = 6f;             
    public float jumpForce = 5f;         
    public Transform cameraTransform;    
    public float turnSmoothTime = 0.1f;  
    private float turnSmoothVelocity;
    public LayerMask groundMask;        
    public Transform groundCheck;        
    public float groundDistance = 0.4f;  

    private bool isGrounded;

    [SerializeField]
    private GameManager gameManager; 

    public Animator animator;

    public float fallThreshold = -2f; // threshold to detect falling down

    private void Start()
    {
        rb = GetComponent<Rigidbody>();  
        rb.freezeRotation = true;        

        if (animator == null)
        {
            animator = GetComponent<Animator>(); 
        }
    }

    // for collectibles trigger
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible") && gameManager.IsGameActive()) 
        {
            gameManager.CollectObject();
            Destroy(other.gameObject); 
        }
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        animator.SetBool("isGrounded", isGrounded);

        float verticalVelocity = rb.velocity.y;

        if (verticalVelocity < fallThreshold)
        {
            animator.SetBool("isFalling", true);  // play fall animation
        }
       
        else if (verticalVelocity >= 0 && isGrounded)
        {
            animator.SetBool("isFalling", false);  // stop fall animation
        }

        // get input WASD ------------
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");    
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
       
        float currentSpeed = direction.magnitude * speed;

        if (!animator.GetBool("isFalling"))
        {
            animator.SetFloat("Speed", currentSpeed);
        }
        else
        {          
            animator.SetFloat("Speed", 0);
        }

        // handle rotation and movenent
        if (currentSpeed >= 0.1f && !animator.GetBool("isFalling"))
        {
            // calculate target angle relative to the camera's forward direction
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

            // smooth the rotation for a fluid turn
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // move the player in the direction the camera is facing
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.MovePosition(rb.position + moveDirection.normalized * speed * Time.deltaTime);
        }

        // jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("Jump"); 
        }
    }
}
