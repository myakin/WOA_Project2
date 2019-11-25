using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashManager : MonoBehaviour
{

    bool isPostCrashProcedureOn;
    GameObject player;

    
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            GameController.gameController.isGamePlaying=false;
            player = other.gameObject;
            player.GetComponent<PlayerMovement>().crashEffect.transform.position = player.transform.position;
            player.GetComponent<PlayerMovement>().crashEffect.Play();
            player.GetComponent<PlayerMovement>().isInputBlocked=true;
            Destroy(player.GetComponent<Rigidbody2D>());
            isPostCrashProcedureOn=true;
        }
    }

    private void Update() {
        if (isPostCrashProcedureOn) {
            if (!player.GetComponent<PlayerMovement>().crashEffect.isPlaying) {
                isPostCrashProcedureOn=false;
                Time.timeScale = 0;
                SceneLoader.sceneLoader.LoadMenu();
            }
            
        }
    }
}
