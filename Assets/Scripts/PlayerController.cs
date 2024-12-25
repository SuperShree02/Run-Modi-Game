using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 10f;
    public float laneSwitchSpeed = 15f;
    public float jumpForce = 8f;
    public float slideDuration = 1f;
    public float gravityMultiplier = 2.5f;

    [Header("Lane Settings")]
    private int currentLane = 0;
    private float laneDistance = 3f;

    private Rigidbody rb;
    private bool isGrounded = true;
    private bool isSliding = false;

    private Vector3 startScale;

    [Header("Animation Settings")]
    public Transform gfx; // Reference to the GFX child object
    private Animator animator; // Animator component

    private Vector2 startMousePosition;
    private bool isSwiping = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startScale = transform.localScale;

        // Get Animator from the GFX child object
        animator = gfx.GetComponent<Animator>();

        rb.useGravity = false; // Custom gravity

        // Set target frame rate for smoother performance
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        HandleMouseInput();
        HandleJumping();
        HandleSliding();
        ApplyCustomGravity();

        // Update grounded state for animation
        animator.SetBool("isGrounded", isGrounded);
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0)) // Mouse click start
        {
            startMousePosition = Input.mousePosition;
            isSwiping = true;
        }

        if (Input.GetMouseButtonUp(0) && isSwiping) // Mouse release
        {
            Vector2 endMousePosition = Input.mousePosition;
            Vector2 swipeDelta = endMousePosition - startMousePosition;

            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y)) // Horizontal swipe
            {
                if (swipeDelta.x > 50 && currentLane < 1) // Swipe right
                {
                    currentLane++;
                }
                else if (swipeDelta.x < -50 && currentLane > -1) // Swipe left
                {
                    currentLane--;
                }
            }
            else if (swipeDelta.y > 50 && isGrounded && !isSliding) // Swipe up for jump
            {
                animator.SetTrigger("Jump");
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
                isGrounded = false;
            }
            else if (swipeDelta.y < -50 && isGrounded && !isSliding) // Swipe down for slide
            {
                StartCoroutine(Slide());
            }

            isSwiping = false;
        }

        Vector3 targetPosition = new Vector3(currentLane * laneDistance, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, laneSwitchSpeed * Time.deltaTime);
    }

    void HandleJumping()
    {
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGrounded && !isSliding)
        {
            animator.SetTrigger("Jump"); // Trigger jump animation
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            isGrounded = false;
        }
    }

    void HandleSliding()
    {
        if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && isGrounded && !isSliding)
        {
            StartCoroutine(Slide());
        }
    }

    private System.Collections.IEnumerator Slide()
    {
        isSliding = true;

        // Trigger slide animation
        animator.SetTrigger("Slide");

        // Reduce scale for sliding effect
        transform.localScale = new Vector3(startScale.x, startScale.y / 2, startScale.z);
        yield return new WaitForSeconds(slideDuration);

        // Reset scale
        transform.localScale = startScale;
        isSliding = false;
    }

    void ApplyCustomGravity()
    {
        if (!isGrounded)
        {
            rb.linearVelocity += Vector3.down * gravityMultiplier * Physics.gravity.y * Time.deltaTime;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.ResetTrigger("Jump"); // Reset jump trigger when grounded
        }
    }
}
