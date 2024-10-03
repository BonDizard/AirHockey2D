/*
 * Author: Bharath Kumar S
 * Date: 03-10-2024
 * Description: Handels Scene Loading
 */
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader {
    public enum Scene {
        MainMenuScene,
        GameScene,
        LobbyScene,
    }
    private static Scene targetScene;
    public static void Load(Scene scene) {
        targetScene = scene;
        SceneManager.LoadScene(targetScene.ToString());
    }

}
