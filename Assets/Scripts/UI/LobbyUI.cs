/*
Author: Bharath Kumar S
Date: 03-10-2024
Description: LobbyUI
*/
using System;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour {
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quickJoinButton;
    [SerializeField] private Button joinByCodeButton;
    [SerializeField] private Button creatLobbyButton;
    [SerializeField] private TMP_InputField lobbyCode;
    [SerializeField] private CreateLobbyUI createLobbyUI;
    [SerializeField] private Transform lobbyListContainer;
    [SerializeField] private Transform lobbyTemplate;
    private void Awake() {
        mainMenuButton.onClick.AddListener(() => {
            GameLobbyManager.Instance.LeaveLobby();
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        quickJoinButton.onClick.AddListener(() => {
            GameLobbyManager.Instance.QuickJoin();
        });
        joinByCodeButton.onClick.AddListener(() => {
            GameLobbyManager.Instance.JoinWithCode(lobbyCode.text);
        });
        creatLobbyButton.onClick.AddListener(() => {
            createLobbyUI.Show();
        });
    }
    private void Start() {
        GameLobbyManager.Instance.OnLobbyListChanged += GameLobbyManager_OnLobbyListChanged;
        UpdateLobbyList(new List<Lobby>());
    }

    private void GameLobbyManager_OnLobbyListChanged(object sender, GameLobbyManager.OnLobbyListChnagedEventArgs e) {
        UpdateLobbyList(e.lobbyList);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList) {
        //clean up 
        foreach (Transform child in lobbyListContainer) {
            if (child == lobbyTemplate) continue;
            Destroy(child.gameObject);
        }
        foreach (Lobby lobby in lobbyList) {
            Transform lobbyTransform = Instantiate(lobbyTemplate, lobbyListContainer);
            lobbyTransform.gameObject.SetActive(true);

            lobbyTransform.GetComponent<LobbyListSingleUI>().SetLobby(lobby);
        }
    }
    private void OnDestroy() {
        GameLobbyManager.Instance.OnLobbyListChanged -= GameLobbyManager_OnLobbyListChanged;
    }
}

