/*
Author: Bharath Kumar S
Date: 03-10-2024
Description: Countdown Ui
*/
using System;
using TMPro;
using UnityEngine;

public class CountdownUI : MonoBehaviour {
    [SerializeField] TextMeshProUGUI timmerText;
    private void Start() {
        Hide();
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    }
    private void Update() {
        timmerText.text = GameManager.Instance.GetCountDownTimer().ToString();
    }
    private void GameManager_OnGameStateChanged(object sender, EventArgs e) {
        if (GameManager.Instance.IsCountDownToStart()) {
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
