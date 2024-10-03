/*
Author: Bharath Kumar S
Date: 03-10-2024
Description: CreateLobbyUI
*/
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateLobbyUI : MonoBehaviour {

    [SerializeField] private Button createPublicLobbyButton;
    [SerializeField] private Button createPrivateLobbyButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TMP_InputField lobbyName;

    private void Awake() {
        createPublicLobbyButton.onClick.AddListener(() => {
            GameLobbyManager.Instance.CreatLobby(lobbyName.text, false);
        });
        createPrivateLobbyButton.onClick.AddListener(() => {
            GameLobbyManager.Instance.CreatLobby(lobbyName.text, true);
        });
        closeButton.onClick.AddListener(() => {
            Hide();
        });
    }
    private void Hide() {
        gameObject.SetActive(false);
    }
    public void Show() {
        gameObject.SetActive(true);
    }
}
