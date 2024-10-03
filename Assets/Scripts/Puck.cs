using System;
using UnityEngine;
using System.Collections;  // For Coroutine

public class Puck : MonoBehaviour {
    public static Puck Instance { get; private set; }

    [SerializeField] private float movementSpeed = 7f;
    [SerializeField] private float maxSpeed = 10f;  // Limit maximum speed

    private bool canMove = false;
    private new Rigidbody2D rigidbody2D;
    private bool wasGoal = false;  // Prevents multiple triggers for a goal

    public event Action<bool> OnGoalScored;  // Notify GameManager of a goal (true for Blue, false for Red)

    private void Awake() {
        Instance = this;
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        // Initial movement will start when the game is in the playing state
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;  // Subscribe to state changes
    }

    private void FixedUpdate() {
        // Ensure movement is only allowed when the game is in the GamePlaying state
        if (canMove) {
            // Maintain constant speed and limit maximum velocity for smoother control
            rigidbody2D.velocity = Vector2.ClampMagnitude(rigidbody2D.velocity.normalized * movementSpeed, maxSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!wasGoal) {  // Check to avoid multiple triggers
            if (other.CompareTag("RedScore")) {
                wasGoal = true;
                OnGoalScored?.Invoke(false);  // Notify GameManager of Red player goal
                StartCoroutine(ResetPuckWithDelay(false));
            }
            else if (other.CompareTag("BlueScore")) {
                wasGoal = true;
                OnGoalScored?.Invoke(true);  // Notify GameManager of Blue player goal
                StartCoroutine(ResetPuckWithDelay(true));
            }
        }
    }

    private IEnumerator ResetPuckWithDelay(bool didEnemyScore) {
        canMove = false;  // Disable movement while resetting
        rigidbody2D.velocity = Vector2.zero;  // Stop the puck

        // Wait for 1 second before resetting the puck
        yield return new WaitForSecondsRealtime(1);

        wasGoal = false;

        // Reset puck to center position
        rigidbody2D.position = didEnemyScore ? new Vector2(0, -1) : new Vector2(0, 1);

        // If the game is still playing, start movement again
        if (GameManager.Instance.IsGamePlaying()) {
            StartGame();
        }
    }

    private void StartGame() {
        // Enable movement when starting
        canMove = true;

        // Assign a new random direction and start moving the puck
        Vector2 newDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        rigidbody2D.velocity = newDirection * movementSpeed;
    }

    // This method will be called when the game state changes
    private void GameManager_OnGameStateChanged(object sender, EventArgs e) {
        if (GameManager.Instance.IsGamePlaying()) {
            // If the game is now playing, start the puck movement
            StartGame();
        }
        else {
            // If the game is paused or over, stop the puck
            canMove = false;
            rigidbody2D.velocity = Vector2.zero;  // Stop the puck immediately
        }
    }
}
