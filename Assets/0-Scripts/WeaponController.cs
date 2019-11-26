using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public ParticleSystem laser;

    private void Update() {
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (!laser.isPlaying) {
                Fire();
            }
        }
        if (Input.GetKeyUp(KeyCode.Space)) {
            StopFire();
        }
        
    }

    public void Fire() {
        laser.Play();
    }
    public void StopFire() {
        laser.Stop();
    }
}
