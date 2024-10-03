using System;
using UnityEngine;
using System.Collections;  // For Coroutine

public class Puck : MonoBehaviour {
    private const string BLUE_PLAYER_SCORE_TAG = "BlueScore";
    private const string RED_PLAYER_SCORE_TAG = "RedScore";

    public static Puck Instance { get; private set; }
    [SerializeField] private float movementSpeed = 7f;
    [SerializeField] private float maxSpeed = 10f;  // Limit maximum speed
    public event EventHandler<OnPlayerScoredEventArgs> OnPlayerRedScored;
    public event EventHandler<OnPlayerScoredEventArgs> OnPlayerBlueScored;
    public event EventHandler OnGameWon;
    public event EventHandler OnGameLost;

    public class OnPlayerScoredEventArgs : EventArgs {
        public int score;
    }

    private Rigidbody2D rigidbody2D;
    private int playerBlueScore = 0;
    private int playerRedScore = 0;
    private int maxScore = 5;
    private bool wasGoal = false;  // Similar to your friend's 'WasGoal'

    private void Awake() {
        Instance = this;
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        // Initial movement, no delay
        StartGame();
    }

    private void FixedUpdate() {
        // Maintain constant speed and limit maximum velocity for smoother control
        rigidbody2D.velocity = Vector2.ClampMagnitude(rigidbody2D.velocity.normalized * movementSpeed, maxSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!wasGoal) {  // Check to avoid multiple triggers
            if (other.CompareTag(RED_PLAYER_SCORE_TAG)) {
                playerRedScore++;
                wasGoal = true;
                OnPlayerRedScored?.Invoke(this, new OnPlayerScoredEventArgs { score = playerRedScore });
                StartCoroutine(ResetPuckWithDelay(false));  // Reset puck after Red player scores
                CheckForGameEnd();
            }
            else if (other.CompareTag(BLUE_PLAYER_SCORE_TAG)) {
                playerBlueScore++;
                wasGoal = true;
                OnPlayerBlueScored?.Invoke(this, new OnPlayerScoredEventArgs { score = playerBlueScore });
                StartCoroutine(ResetPuckWithDelay(true));  // Reset puck after Blue player scores
                CheckForGameEnd();
            }
        }
    }

    private IEnumerator ResetPuckWithDelay(bool didEnemyScore) {
        // Wait for 1 second before resetting the puck
        yield return new WaitForSecondsRealtime(1);
        wasGoal = false;

        // Reset puck to center position
        rigidbody2D.velocity = Vector2.zero;  // Stop the puck

        if (didEnemyScore) {
            rigidbody2D.position = new Vector2(0, -1);  // Set enemy's position
        }
        else {
            rigidbody2D.position = new Vector2(0, 1);   // Set player's position
        }

        // Start movement again without waiting for an additional delay
        StartGame();
    }

    private void StartGame() {
        // Assign a new random direction and start moving the puck
        Vector2 newDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        rigidbody2D.velocity = newDirection * movementSpeed;
    }

    private void CheckForGameEnd() {
        // Check if either player has won the game
        if (playerBlueScore >= maxScore) {
            OnGameWon?.Invoke(this, EventArgs.Empty);  // Game won by Blue player
        }
        else if (playerRedScore >= maxScore) {
            OnGameLost?.Invoke(this, EventArgs.Empty);  // Game lost (won by Red player)
        }
    }
}
