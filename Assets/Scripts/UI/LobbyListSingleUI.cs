/*
 * Author: Bharath Kumar S
 * Date: 2-10-2024
 * Description: Lobby List single template
 */

using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyListSingleUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    private Lobby lobby;
    private void Awake() {
        GetComponent<Button>().onClick.AddListener(() => {
            GameLobbyManager.Instance.JoinWithId(lobby.Id);
        });
    }

    public void SetLobby(Lobby lobby) {
        this.lobby = lobby;
        lobbyNameText.text = lobby.Name;
    }
}
