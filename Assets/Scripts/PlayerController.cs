using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 5.0f; // The speed at which the player moves
    public float turnSpeed; // The speed at which the player turns
    public GameObject SpellCastingScreen; // The screen for spell casting
    public Camera Camera; // The camera object
    CameraController cameraController; // The camera controller component
    CharacterController characterController; // The character controller component

    public GameObject GameManager;

    public float groundCheckRadius = 0.2f; // The radius of the ground check sphere
    public Vector3 groundCheckOffset; // The offset of the ground check sphere
    public LayerMask groundCheckLayerMask; // The layer mask for ground checking

    public bool isGrounded; // Flag indicating if the player is grounded

    private bool iscasting = false; // Flag indicating if the player is casting a spell
    public GameObject projectiletofire; // The projectile to fire

    [SerializeField]
    float ySpeed; // The vertical speed of the player

    private void Awake()
    {
        cameraController = Camera.GetComponent<CameraController>(); // Get the camera controller component
        characterController = GetComponent<CharacterController>(); // Get the character controller component
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("PlayerController Started"); // Log a message indicating that the PlayerController has started
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (iscasting) return;
            SpellCastingScreen.SetActive(true); // Toggle the visibility of the spell casting screen
            Cursor.visible = true; // Toggle the visibility of the cursor
            Cursor.lockState = CursorLockMode.None; // Unlock the cursor if it is currently locked
            File.Delete(Directory.GetCurrentDirectory() + "/Assets/temp/temp.png"); // Delete a temporary file
            cameraController.POV = 1; // Set the camera POV to 1
            cameraController.FreezeCamera = true; // Toggle the freeze camera flag in the camera controller

        }
        if (Input.GetMouseButtonDown(0) && iscasting)
        {
            GameObject projectile = Instantiate(projectiletofire, gameObject.transform.position + (Camera.transform.rotation * Vector3.forward * 2), Camera.transform.rotation);
            Debug.Log("click. you are using the " + projectile.gameObject.name + " at the angle " + Camera.transform.rotation.eulerAngles);
            GameManager.
                GetComponent<ProjectileManager>().
                Fire(
                    projectile, 
                    Camera.transform.rotation
                    ); // Fire the projectile
            isCasting = false; // Toggle the is casting flag
            SpellCastingScreen.SetActive(false); // Toggle the visibility of the spell casting screen
            Cursor.visible = false; // Toggle the visibility of the cursor
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor
            cameraController.POV = 3; // Set the camera POV to 3
        }

        float h = Input.GetAxis("Horizontal"); // Get the horizontal input
        float v = Input.GetAxis("Vertical"); // Get the vertical input

        var moveInput = (new Vector3(h, 0, v)).normalized; // Normalize the move input vector

        GroundCheck(); // Perform ground check

        Quaternion targetRotation = transform.rotation; // Set the target rotation to the current rotation

        if (isGrounded)
        {
            ySpeed = 0f; // Set the vertical speed to a negative value to counteract gravity
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime; // Apply gravity to the vertical speed
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(isGrounded); // Log a message indicating that the space key has been pressed
            if (isGrounded)
            {
                ySpeed = 2.7f; // Set the vertical speed to a positive value to counteract  gravity
            }
        }
        var velocity = cameraController.PlanarRotation * moveInput * movementSpeed; // Calculate the velocity based on the camera rotation, move input, and movement speed
        velocity.y = ySpeed; // Set the vertical velocity

        characterController.Move(velocity * Time.deltaTime); // Move the character controller based on the velocity

        if ((Mathf.Abs(h) + Mathf.Abs(v)) > 0)
        {
            targetRotation = Quaternion.LookRotation(cameraController.PlanarRotation * moveInput); // Calculate the target rotation based on the camera rotation and move input
        }
        Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime); // Rotate towards the target rotation

    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundCheckLayerMask); // Perform a sphere cast to check if the player is grounded
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f); // Set the color of the gizmos
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius); // Draw a sphere gizmo for ground checking
    }

    public bool isCasting { get => iscasting; set => iscasting = value; }
    public GameObject ProjectileToFire { get => projectiletofire; set => projectiletofire = value; }
}
