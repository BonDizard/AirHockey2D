/*
Author: Bharath Kumar S
Date: 03-10-2024
Description: scoreUI
*/
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI bluePlayerScore;
    [SerializeField] private TextMeshProUGUI redPlayerScore;
    private void Start() {
        GameManager.Instance.OnPlayerBlueScored += Puck_OnPlayerBlueScored;
        GameManager.Instance.OnPlayerRedScored += Puck_OnPlayerRedScored;
    }

    private void Puck_OnPlayerRedScored(object sender, GameManager.OnPlayerScoredEventArgs e) {
        redPlayerScore.text = e.score.ToString();
    }

    private void Puck_OnPlayerBlueScored(object sender, GameManager.OnPlayerScoredEventArgs e) {
        bluePlayerScore.text = e.score.ToString();
    }
}
