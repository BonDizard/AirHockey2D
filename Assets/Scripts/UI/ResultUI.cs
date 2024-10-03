/*
Author: Bharath Kumar S
Date: 03-10-2024
Description: Handles result ui
*/
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultUI : MonoBehaviour {
    [SerializeField] private Button playAgainButton;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private Button menuButton;
    private void Awake() {
        playAgainButton.onClick.AddListener(() => {
            GameManager.Instance.PlayAgain();
            Hide();
        });
        menuButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
    }
    private void Start() {
        Hide();
        GameManager.Instance.OnGameLost += GameManager_OnGameLost;
        GameManager.Instance.OnGameWon += GameManager_OnGameWon;
    }

    private void GameManager_OnGameLost(object sender, EventArgs e) {
        Show();
        Time.timeScale = 0f;
        resultText.text = "YOU LOST!";
    }

    private void GameManager_OnGameWon(object sender, EventArgs e) {
        Show();
        resultText.text = "YOU WON!";
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
    private void Show() {
        gameObject.SetActive(true);
    }
}
