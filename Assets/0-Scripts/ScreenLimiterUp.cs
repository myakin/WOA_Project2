using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenLimiterUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            Time.timeScale = 0;
            SceneLoader.sceneLoader.LoadMenu();
        }
    }
}
