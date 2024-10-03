using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameMultiplayerManager : MonoBehaviour {
    public const int MAX_PLAYERS_ALLOWED = 2;

    public static GameMultiplayerManager Instance { get; private set; }
    private void Awake() {

        Instance = this;
    }
    public void StartClient() {
        NetworkManager.Singleton.StartClient();
    }


    public void StartHost() {
        NetworkManager.Singleton.StartHost();
    }
}
