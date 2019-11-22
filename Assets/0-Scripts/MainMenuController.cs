using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button saveButton, loadButton, newGameButton, quitButton;

    private void Start() {
        saveButton.onClick.AddListener(SaveGame);
        loadButton.onClick.AddListener(LoadGame);
        newGameButton.onClick.AddListener(StartNewGame);
        quitButton.onClick.AddListener(QuitApp);
    }

    public void SaveGame() {

    }
    public void LoadGame() {

    }

    public void StartNewGame() {
        SceneLoader.sceneLoader.LoadSceneByLevelNo(1);
    }

    public void QuitApp() {

    }



}
