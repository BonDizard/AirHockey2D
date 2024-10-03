/*
Author: Bharath Kumar S
Date: 03-10-2024
Description: Game manager
*/
using System;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    public enum State {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        GameOver,
    }
    public event EventHandler OnGameStateChanged;
    public event EventHandler OnGameLost;
    public event EventHandler OnGameWon;
    public event EventHandler<OnPlayerScoredEventArgs> OnPlayerBlueScored;
    public event EventHandler<OnPlayerScoredEventArgs> OnPlayerRedScored;
    public class OnPlayerScoredEventArgs : EventArgs {
        public int score;
    }
    [SerializeField] private int maxScore = 5;
    [SerializeField] private Transform playerPrefab;
    [SerializeField] private Transform enemyPrefab;
    [SerializeField] private Transform playerRestrictionsPrefab;
    [SerializeField] private Transform enemyRestrictionsPrefab;

    private int playerBlueScore = 0;
    private int playerRedScore = 0;
    bool isSpawnedOnce = false;
    private State state;
    private float countDownTime = 3f;

    private void Awake() {
        Instance = this;
        state = State.WaitingToStart;
        OnGameStateChanged?.Invoke(this, EventArgs.Empty);
        Time.timeScale = 0f;
    }

    private void Start() {
        GameInput.Instance.OnTouchingPerformed += GameInput_OnTouchingPerformed;
        Puck.Instance.OnGoalScored += Puck_OnGoalScored;  // Subscribe to puck events
    }

    private void Update() {
        switch (state) {
            case State.WaitingToStart:
                break;

            case State.CountDownToStart:
                countDownTime -= Time.deltaTime;
                if (countDownTime < 0) {
                    state = State.GamePlaying;
                    OnGameStateChanged?.Invoke(this, EventArgs.Empty);  // Notify that the game started
                }
                break;

            case State.GamePlaying:
                if (!isSpawnedOnce) {

                    Instantiate(playerPrefab);
                    // Instantiate(enemyPrefab);
                    Instantiate(playerRestrictionsPrefab);
                    Instantiate(enemyRestrictionsPrefab);
                    //Instantiate(puckPrefab);
                    isSpawnedOnce = true;
                }
                break;

            case State.GameOver:
                // Handle game over state
                break;

            default:
                break;
        }
    }

    private void GameInput_OnTouchingPerformed(object sender, GameInput.OnTouchingPerformedEventArgs e) {
        if (state == State.WaitingToStart) {
            state = State.CountDownToStart;
            Time.timeScale = 1f;
            OnGameStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Puck_OnGoalScored(bool didBlueScore) {
        if (state == State.GamePlaying) {
            if (didBlueScore) {
                playerBlueScore++;
                OnPlayerBlueScored.Invoke(this, new OnPlayerScoredEventArgs {
                    score = playerBlueScore,
                });
            }
            else {
                playerRedScore++;
                OnPlayerRedScored.Invoke(this, new OnPlayerScoredEventArgs {
                    score = playerRedScore,
                });
            }

            // Check if someone won
            CheckForGameEnd();
        }
    }

    private void CheckForGameEnd() {
        if (playerBlueScore >= maxScore) {
            state = State.GameOver;
            OnGameWon?.Invoke(this, EventArgs.Empty);
            OnGameStateChanged?.Invoke(this, EventArgs.Empty);  // Blue player wins
        }
        else if (playerRedScore >= maxScore) {
            state = State.GameOver;
            OnGameLost?.Invoke(this, EventArgs.Empty);
            OnGameStateChanged?.Invoke(this, EventArgs.Empty);  // Red player wins
        }
    }

    public bool IsWaitingToStart() {
        return state == State.WaitingToStart;
    }
    public bool IsCountDownToStart() {
        return state == State.CountDownToStart;
    }
    public bool IsGamePlaying() {
        return state == State.GamePlaying;
    }

    public bool IsGameOver() {
        return state == State.GameOver;
    }
    public int GetCountDownTimer() {
        return Mathf.CeilToInt(countDownTime);
    }
}
