using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float antiGravityForceMagnitude = 10f;
    public float fastFlyForceMagnitude = 5f;
    public bool isInputBlocked;
    private Vector2 screenDimensions;
    private bool pauseGame;

    private void Start() {
        screenDimensions = new Vector2(Screen.width, Screen.height);
        Time.timeScale = 1;
        //rb.AddForce(Vector2.right * fastFlyForceMagnitude, ForceMode2D.Impulse);
    }

    private void Update() {
        Touch[] touches = Input.touches;
        if (!isInputBlocked) {
            if (touches.Length>0) {
                isInputBlocked = true;
                if (touches.Length==1) {
                    rb.AddForce(Vector2.up * antiGravityForceMagnitude, ForceMode2D.Impulse);
                }
                if (touches.Length==2) {
                    rb.AddForce(Vector2.right * fastFlyForceMagnitude, ForceMode2D.Impulse);
                }
                StartCoroutine(RemoveInputBlock());
            }
        }
        
    }

    private IEnumerator RemoveInputBlock() {
        yield return new WaitForSeconds(0.1f);
        isInputBlocked = false;
    }
}
