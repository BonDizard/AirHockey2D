/*
Author: Bharath Kumar S
Date: 02-10-2024
Description: Handels all the game input from mouse, keyboard, touch and gamepad 
*/

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {
    public static GameInput Instance { get; private set; }
    public event EventHandler OnPauseActionPerformed;
    public event EventHandler<OnTouchingPerformedEventArgs> OnTouchingPerformed;

    public class OnTouchingPerformedEventArgs : EventArgs {
        public Vector2 currentPosition;
    }

    private PlayerInputAction playerInputAction;
    private bool onHolding = false;
    private Vector2 currentPosition = Vector2.zero;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject); // Ensure there's only one instance
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Keep this object alive across scenes

        playerInputAction = new PlayerInputAction();
        playerInputAction.Player.Enable();

        // Register input events
        playerInputAction.Player.Pause.performed += PlayerInputAction_Pause_Performed;
        playerInputAction.Player.PrimaryContact.started += PlayerInputAction_Touch_Started;
        playerInputAction.Player.PrimaryContact.canceled += PlayerInputAction_Touch_Canceled;
    }

    private void OnDestroy() {
        // Unregister events to prevent memory leaks
        playerInputAction.Player.Pause.performed -= PlayerInputAction_Pause_Performed;
        playerInputAction.Player.PrimaryContact.started -= PlayerInputAction_Touch_Started;
        playerInputAction.Player.PrimaryContact.canceled -= PlayerInputAction_Touch_Canceled;
    }

    private void Update() {
        if (onHolding) {
            UpdateCurrentTouchPosition();
        }
    }

    private void PlayerInputAction_Touch_Started(InputAction.CallbackContext context) {
        onHolding = true;
        UpdateCurrentTouchPosition(); // Capture initial position
    }

    private void UpdateCurrentTouchPosition() {
        // Get the current position of the touch/mouse
        Vector2 screenPosition = playerInputAction.Player.PrimaryPosition.ReadValue<Vector2>();
        currentPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        // Invoke event if subscribers exist
        OnTouchingPerformed?.Invoke(this, new OnTouchingPerformedEventArgs {
            currentPosition = currentPosition,
        });
    }

    private void PlayerInputAction_Touch_Canceled(InputAction.CallbackContext context) {
        onHolding = false;
        currentPosition = Vector2.zero;
    }

    private void PlayerInputAction_Pause_Performed(InputAction.CallbackContext context) {
        OnPauseActionPerformed?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetPlayerMovementNormalized() {
        return playerInputAction.Player.Movement.ReadValue<Vector2>().normalized;
    }
}
