/*
Author: Bharath Kumar S
Date: 03-10-2024
Description: main menu ui
*/
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {
    [SerializeField] private Button playOfflineButton;
    [SerializeField] private Button playOnline;

    private void Awake() {
        playOfflineButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.GameScene);
        });
        playOnline.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.LobbyScene);
        });
    }
}
