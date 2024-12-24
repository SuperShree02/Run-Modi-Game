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

    private Vector2 startTouchPosition;
    private bool isTouching = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startScale = transform.localScale;

        // Get Animator from the GFX child object
        animator = gfx.GetComponent<Animator>();

        rb.useGravity = false; // Custom gravity
    }

    void Update()
    {
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        if (IsMobilePlatform())
        {
            HandleTouchInput();
        }
        else
        {
            HandleKeyboardInput();
        }

        HandleJumping();
        HandleSliding();
        ApplyCustomGravity();

        // Update grounded state for animation
        animator.SetBool("isGrounded", isGrounded);
    }

    // Checks if the game is running on a mobile platform
    bool IsMobilePlatform()
    {
        return (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer);
    }

    void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);  // Get the first touch

            // Handle swipe direction (left-right)
            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position; // Store the starting touch position
                isTouching = true;
            }

            if (touch.phase == TouchPhase.Moved && isTouching)
            {
                float swipeDistance = touch.position.x - startTouchPosition.x;

                if (Mathf.Abs(swipeDistance) > 50) // Swipe threshold for movement
                {
                    if (swipeDistance > 0 && currentLane < 1)  // Swipe right
                    {
                        currentLane++;
                    }
                    else if (swipeDistance < 0 && currentLane > -1)  // Swipe left
                    {
                        currentLane--;
                    }

                    Vector3 targetPosition = new Vector3(currentLane * laneDistance, transform.position.y, transform.position.z);
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * laneSwitchSpeed);
                }
            }

            // Handle slide (downward swipe)
            if (touch.phase == TouchPhase.Ended && isTouching)
            {
                Vector2 swipeDelta = touch.position - startTouchPosition;

                if (swipeDelta.y < -100 && isGrounded && !isSliding)  // Downward swipe for sliding
                {
                    StartCoroutine(Slide());
                }

                isTouching = false;
            }
        }
    }

    void HandleKeyboardInput()
    {
        // Keyboard controls for PC version
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (currentLane > -1) currentLane--;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (currentLane < 1) currentLane++;
        }

        Vector3 targetPosition = new Vector3(currentLane * laneDistance, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * laneSwitchSpeed);

        // Slide and Jump controls for PC version
        if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && isGrounded && !isSliding)
        {
            StartCoroutine(Slide());
        }

        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGrounded && !isSliding)
        {
            animator.SetTrigger("Jump");
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            isGrounded = false;
        }
    }

    void HandleJumping()
    {
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGrounded && !isSliding)
        {
            Debug.Log("Jump Triggered!");
            animator.SetTrigger("Jump"); // Trigger jump animation
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            isGrounded = false;
        }
    }

    void HandleSliding()
    {
        if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) && isGrounded && !isSliding)
        {
            Debug.Log("Slide Triggered!");
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
            rb.velocity += Vector3.down * gravityMultiplier * Physics.gravity.y * Time.deltaTime;
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
