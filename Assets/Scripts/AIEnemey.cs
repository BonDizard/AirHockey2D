/*
Author: Bharath Kumar S
Date: 03-10-2024
Description: Ai enemy logic
*/

using UnityEngine;

public class AIEnemy : MonoBehaviour {

    public float MaxMovementSpeed;
    private Rigidbody2D rb;
    private Vector2 startingPosition;

    public Rigidbody2D Puck;

    public Transform PlayerBoundaryHolder;
    private Boundary playerBoundary;
    private Vector2 targetPosition;

    private bool isFirstTimeInOpponentsHalf = true;
    private float offsetXFromTarget;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        startingPosition = rb.position;

        // Define the player boundary from the boundary holder
        playerBoundary = new Boundary(PlayerBoundaryHolder.GetChild(0).position.y,
                              PlayerBoundaryHolder.GetChild(1).position.y,
                              PlayerBoundaryHolder.GetChild(2).position.x,
                              PlayerBoundaryHolder.GetChild(3).position.x);
    }

    private void FixedUpdate() {
        float movementSpeed;

        // If the puck is outside the AI's boundary (on the player's side)
        if (Puck.position.x < playerBoundary.Left) {
            if (isFirstTimeInOpponentsHalf) {
                isFirstTimeInOpponentsHalf = false;
                offsetXFromTarget = Random.Range(-1f, 1f); // Offset for variation
            }

            // Slow down the AI when the puck is on the player's side
            movementSpeed = MaxMovementSpeed * Random.Range(0.1f, 0.3f);

            // Follow the puck's Y position with a slight offset
            targetPosition = new Vector2(startingPosition.x, // Keep AI in its starting X position
                              Mathf.Clamp(Puck.position.y + offsetXFromTarget, // Follow puck Y
                                        playerBoundary.Down + 0.5f, // Clamp Y within boundaries
                                        playerBoundary.Up - 0.5f));
        }
        // If the puck is within AI's boundary (on AI's side)
        else {
            isFirstTimeInOpponentsHalf = true;

            // Increase speed when puck is on AI's side
            movementSpeed = Random.Range(MaxMovementSpeed * 0.4f, MaxMovementSpeed);

            // Follow puck's position within the AI's allowed area
            targetPosition = new Vector2(Mathf.Clamp(Puck.position.x, playerBoundary.Left, playerBoundary.Right),
                                         Mathf.Clamp(Puck.position.y, playerBoundary.Down, playerBoundary.Up));
        }

        // Move the AI towards the target position smoothly
        rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, movementSpeed * Time.fixedDeltaTime));
    }
}
