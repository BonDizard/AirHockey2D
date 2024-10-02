using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private float movementSpeed = 7f;
    private Vector2 targetPosition;
    private bool isDragging = false;

    private void Start() {
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

    private void Update() {
        if (isDragging) {
            MovePlayerTowardsTarget();
        }
        else {
            HandlePlayerMovement();
        }
    }

    private void MovePlayerTowardsTarget() {
        // Smooth movement towards target position using MoveTowards for more responsive movement
        Vector3 targetPos3D = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPos3D, Time.deltaTime * movementSpeed);

        // Optional: Stop dragging when player reaches target position (use a small threshold)
        if (Vector3.Distance(transform.position, targetPos3D) < 0.1f) {
            isDragging = false;  // Stop dragging if very close to the target
        }
    }

    private void HandlePlayerMovement() {
        // Handle keyboard movement if player is not being dragged
        Vector2 moveDirection = GameInput.Instance.GetPlayerMovementNormalized();
        float moveDistance = Time.deltaTime * movementSpeed;

        Vector3 move = new Vector3(moveDirection.x, moveDirection.y, 0).normalized;
        transform.position += move * moveDistance;
    }
}
