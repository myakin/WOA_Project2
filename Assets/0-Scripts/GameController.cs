using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController gameController;

    public List<GameObject> backgoundsPool = new List<GameObject>();
    public bool isGamePlaying = true;

    private void Awake() {
        if (GameController.gameController == null) {
            GameController.gameController = this;
        } else {
            if (this!=GameController.gameController) {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(GameController.gameController.gameObject);
    }

    public void AddToBackgroundPool(GameObject aBackgroundGameObject) {
        backgoundsPool.Add(aBackgroundGameObject);
    }
    public GameObject GetFromBackroundPool() {
        GameObject returnValue = backgoundsPool[0];
        backgoundsPool.RemoveAt(0);
        return returnValue;
    }
    public int CountBackgroundPool() {
        return backgoundsPool.Count;
    }
}
