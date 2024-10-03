/*
Author: Bharath Kumar S
Date: 03-10-2024
Description: pause ui
*/

using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour {
    [SerializeField] private Button menuButton;
    [SerializeField] private Button restartButton;

    private void Awake() {
        menuButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        restartButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.GameScene);
        });
    }
    private void Start() {
        Hide();
    }
    private void Hide() {
        gameObject.SetActive(false);
    }
    private void Show() {
        gameObject.SetActive(false);
    }
}
