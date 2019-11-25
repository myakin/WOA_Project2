using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleColumnManager : MonoBehaviour
{
    public GameObject lower, upper;
    public float aperture;
    public float apertureBoundsMin;
    public float apertureBoundsMax;

    private void Start() {
        aperture = DefineAperture();
        if (aperture>0) {
            upper.transform.position = lower.transform.position + (lower.transform.up * aperture);
        }
    }

    private float DefineAperture() {
        return Random.Range(apertureBoundsMin, apertureBoundsMax);
    }


}
