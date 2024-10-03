/*
Author: Bharath Kumar S
Date: 02-10-2024
Description: Handles all player logic using physics for movement with boundary restrictions
*/

using UnityEngine;

public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }
    [SerializeField] private float movementSpeed = 7f;
    [SerializeField] private Transform restrictions;

    private Vector2 targetPosition;
    private bool isDragging = false;
    private Rigidbody2D rigidbody2D;
    private Vector2 playerSize;
    private Boundary playerBoundary;

    private void Awake() {
        Instance = this;
        playerSize = GetComponent<SpriteRenderer>().bounds.extents;
    }

    private void Start() {
        // Initialize player boundaries based on restrictions
        playerBoundary = new Boundary(
            restrictions.GetChild(0).position.y, // up
            restrictions.GetChild(1).position.y, // down
            restrictions.GetChild(2).position.x, // right
            restrictions.GetChild(3).position.x  // left
        );

        rigidbody2D = GetComponent<Rigidbody2D>();
        GameInput.Instance.OnTouchingPerformed += GameInput_OnTouchingPerformed;
    }

    private void OnDestroy() {
        // Ensure event deregistration to avoid memory leaks
        GameInput.Instance.OnTouchingPerformed -= GameInput_OnTouchingPerformed;
    }

    private void GameInput_OnTouchingPerformed(object sender, GameInput.OnTouchingPerformedEventArgs e) {
        Vector2 touchPosition = e.currentPosition;
        // Start dragging no matter where the touch happens
        isDragging = true;
        targetPosition = touchPosition; // Update target position to touch location
    }

    private void FixedUpdate() {
        // Use FixedUpdate for physics-based movement
        if (isDragging) {
            MovePlayerTowardsTarget();
        }
        else {
            HandlePlayerMovement();
        }

        // Clamp the player's position to the boundaries
        ClampPlayerPosition();
    }

    private void MovePlayerTowardsTarget() {
        // Smooth movement towards target position using Rigidbody2D's velocity
        Vector2 moveDirection = (targetPosition - rigidbody2D.position).normalized;
        rigidbody2D.velocity = moveDirection * movementSpeed;

        // Optional: Stop dragging when player reaches target position (use a small threshold)
        if (Vector2.Distance(rigidbody2D.position, targetPosition) < 0.1f) {
            isDragging = false; // Stop dragging if very close to the target
            rigidbody2D.velocity = Vector2.zero; // Stop the player's movement
        }
    }

    private void HandlePlayerMovement() {
        // Handle keyboard movement by adjusting the player's velocity
        Vector2 moveDirection = GameInput.Instance.GetPlayerMovementNormalized();
        rigidbody2D.velocity = moveDirection * movementSpeed;

        // Stop the movement if no input is provided
        if (moveDirection == Vector2.zero) {
            rigidbody2D.velocity = Vector2.zero;
        }
    }

    private void ClampPlayerPosition() {
        // Get the current position of the player
        Vector2 clampedPosition = rigidbody2D.position;

        // Clamp the X position, accounting for the player's size
        clampedPosition.x = Mathf.Clamp(clampedPosition.x,
            playerBoundary.Left + playerSize.x,
            playerBoundary.Right - playerSize.x);

        // Clamp the Y position, accounting for the player's size
        clampedPosition.y = Mathf.Clamp(clampedPosition.y,
            playerBoundary.Down + playerSize.y,
            playerBoundary.Up - playerSize.y);

        // Apply the clamped position to the Rigidbody
        rigidbody2D.position = clampedPosition;
    }
}
