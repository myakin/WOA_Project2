using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader sceneLoader;

    private void Awake() {
        if (SceneLoader.sceneLoader==null) {
            SceneLoader.sceneLoader = this;
        } else {
            if (this != SceneLoader.sceneLoader) {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(SceneLoader.sceneLoader.gameObject);
    }


    public void LoadSceneByLevelNo(int levelNo) {
        GameController.gameController.backgoundsPool.Clear();
        SceneManager.LoadScene(levelNo);
        SceneManager.UnloadScene(0);
    }
    public void LoadMenu() {
        SceneManager.LoadScene(0, LoadSceneMode.Additive);
    }

}
