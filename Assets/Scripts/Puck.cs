/*
Author: Bharath Kumar S
Date: 03-10-2024
Description: Handles all puck logic
*/

using UnityEngine;

public class Puck : MonoBehaviour {
    [SerializeField] private float movementSpeed = 7f;
    [SerializeField] private float minSpeedThreshold = 1f; // Minimum speed to prevent getting stuck
    private Vector2 moveDirection;
    private new Rigidbody2D rigidbody2D;

    private void Awake() {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        // Generate a random direction for the puck to move in at the start
        moveDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

        // Move in the set random direction with the given speed
        rigidbody2D.velocity = moveDirection * movementSpeed;
    }

    private void FixedUpdate() {
        // Keep the puck moving at a constant speed in the current direction
        MaintainConstantSpeed();
    }

    private void MaintainConstantSpeed() {
        // Get the current velocity direction (ignoring the speed part)
        Vector2 currentDirection = rigidbody2D.velocity.normalized;

        // Apply the same movement speed to keep it constant
        rigidbody2D.velocity = currentDirection * movementSpeed;

        // Prevent the puck from getting stuck by applying a small nudge if velocity drops too low
        if (rigidbody2D.velocity.magnitude < minSpeedThreshold) {
            // Apply a small random nudge to get it out of stuck situations
            Vector2 nudgeDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
            rigidbody2D.velocity = nudgeDirection * movementSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Optionally handle puck collisions with players, walls, or other objects here
    }
}
