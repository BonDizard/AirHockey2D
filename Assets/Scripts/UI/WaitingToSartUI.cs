/*
Author: Bharath Kumar S
Date: 03-10-2024
Description: Waiting to play UI
*/
using System;
using UnityEngine;

public class WaitingToSartUI : MonoBehaviour {
    private void Start() {
        Show();
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(object sender, EventArgs e) {
        if (GameManager.Instance.IsWaitingToStart()) {
            Show();
        }
        else {
            Hide();
        }
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
    private void Show() {
        gameObject.SetActive(true);
    }
}
