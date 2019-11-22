using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSlideManager : MonoBehaviour
{
    public GameObject slidingBackgroundPrefab;
    public GameObject boundsRefObj;
    public float slidingVelocity = 0.01f;
    private float widthOnPositiveX;
    private float widthOnNegativeX;
    private bool isInstantiating;

    private void Start() {
        // GetComponent<MeshRenderer>().bounds.max.x objenin pozitif x ekseninde o anki dunya pozisyonunu doduruyor (lokal pozisyonu degil)
        // o anki dunya pozisyonundan o anki cismin pozisyonunun x'ini cikarirsam objenin gercek pozitif x genisligini bulurum
        // aynisi GetComponent<MeshRenderer>().bounds.min.x icin de gecerli
        widthOnPositiveX = boundsRefObj.GetComponent<MeshRenderer>().bounds.max.x - transform.position.x;
        widthOnNegativeX = Mathf.Abs(Mathf.Abs(boundsRefObj.GetComponent<MeshRenderer>().bounds.min.x) - transform.position.x);
        // Debug.Log(gameObject.name+": widthOnPositiveX="+widthOnPositiveX + " widthOnNegativeX=" + widthOnNegativeX);
        
    }

    private void Update() {
        transform.position += Vector3.left * slidingVelocity;
        //Debug.Log(transform.position.x);
        if (!isInstantiating) {
            if (transform.position.x < -13f) {
                isInstantiating=true;
                Instantiate (slidingBackgroundPrefab, transform.position + (transform.right*(widthOnPositiveX+widthOnNegativeX)), Quaternion.identity);

            }
        }
        
    }
}
