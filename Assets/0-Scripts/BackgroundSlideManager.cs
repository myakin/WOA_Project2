using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSlideManager : MonoBehaviour
{
    private GameObject slidingBackgroundPrefab;
    public GameObject columnsParent;
    public int numberOfUprightColumns=1;
    public Vector2 safeUprightY; // upright ==> x=>min y, y=> max y
    public GameObject boundsRefObj;
    public float slidingVelocity = 0.01f;
    public float instantiationPoint = -13f;
    public float destructionPoint = -34f;

    private float widthOnPositiveX;
    private float widthOnNegativeX;
    private float nonAbsWidthOnNegativeX;
    private bool isInstantiating;

    private List<float> occupiedXs = new List<float>();
    private float checkXDelta=2f;




    private void Start() {
        // GetComponent<MeshRenderer>().bounds.max.x objenin pozitif x ekseninde o anki dunya pozisyonunu doduruyor (lokal pozisyonu degil)
        // o anki dunya pozisyonundan o anki cismin pozisyonunun x'ini cikarirsam objenin gercek pozitif x genisligini bulurum
        // aynisi GetComponent<MeshRenderer>().bounds.min.x icin de gecerli
        widthOnPositiveX = boundsRefObj.GetComponent<MeshRenderer>().bounds.max.x - transform.position.x;
        // widthOnNegativeX = Mathf.Abs(Mathf.Abs(boundsRefObj.GetComponent<MeshRenderer>().bounds.min.x) - transform.position.x);
        // Debug.Log(gameObject.name+": widthOnPositiveX="+widthOnPositiveX + " widthOnNegativeX=" + widthOnNegativeX);


        // TODO: work on column generation with code below
        nonAbsWidthOnNegativeX = boundsRefObj.GetComponent<MeshRenderer>().bounds.min.x - transform.position.x;
        widthOnNegativeX = Mathf.Abs(Mathf.Abs(boundsRefObj.GetComponent<MeshRenderer>().bounds.min.x) - transform.position.x);
        Debug.Log(gameObject.name+": widthOnPositiveX="+widthOnPositiveX + " widthOnNegativeX=" + widthOnNegativeX + " nonAbsWidthOnNegativeX="+nonAbsWidthOnNegativeX);
        
        
        numberOfUprightColumns=DecideHowManyColumnsWillBeGenerated();
        for (int i=0; i<numberOfUprightColumns; i++) {
            GameObject newColumnPrefab = Resources.Load(PickAnObjectToGenerate()) as GameObject;
            GameObject newColumn = Instantiate(newColumnPrefab, columnsParent.transform);
            // position of newColumn

            float posX = DeterminePosX();
            float posY = DeterminePosY();
            //Debug.Log(posX +" "+ posY);
            newColumn.transform.localPosition = new Vector3(
                posX,
                posY,
                0
            );
        }
    }


    #region GAME LOGIC
    private string PickAnObjectToGenerate() {
        int choice = Random.Range(0,3);
        string returnValue="";
        switch (choice) {
            case 0:
                returnValue = "ColumnSprite";
            break;

            case 1:
                returnValue = "ColumnSpriteAnimated";
            break;

            case 2:
                returnValue = "DoubleColumnSprite";
            break;
        }
        return returnValue;
    }
    private float DeterminePosX() {
        float posX = Random.Range(nonAbsWidthOnNegativeX,widthOnPositiveX);
        for (int i=0; i<occupiedXs.Count; i++) {
            if (Mathf.Abs(occupiedXs[i] - posX) < checkXDelta) {
                return DeterminePosX();
            } 
        }
        return posX;
    }
    private float DeterminePosY() {
        float posY = Random.Range(safeUprightY.x, safeUprightY.y);
        // kosullar
        return posY;
    }
    private int DecideHowManyColumnsWillBeGenerated() {
        int numOfCol = Random.Range(1,6);
        // some logic
        return numOfCol;
    }
    #endregion




    private void Update() {
        transform.position += Vector3.left * slidingVelocity;
        //Debug.Log(transform.position.x);
        if (!isInstantiating) {
            if (transform.position.x < instantiationPoint) { // instantiation point
                isInstantiating=true;
                if (GameController.gameController.CountBackgroundPool()==0) {
                    slidingBackgroundPrefab = Resources.Load("SligingBackground") as GameObject;
                    GameObject newBackground = Instantiate (slidingBackgroundPrefab, transform.position + (transform.right*(widthOnPositiveX+widthOnNegativeX)), Quaternion.identity);
                } else {
                    slidingBackgroundPrefab = GameController.gameController.GetFromBackroundPool();
                    slidingBackgroundPrefab.transform.position = transform.position + (transform.right*(widthOnPositiveX+widthOnNegativeX));
                    slidingBackgroundPrefab.transform.rotation = Quaternion.identity;
                    slidingBackgroundPrefab.SetActive(true);
                }
                
            }
        }
        if (transform.position.x < destructionPoint) { // destruction point
            // Destroy(gameObject);
            gameObject.SetActive(false);
            GameController.gameController.AddToBackgroundPool(gameObject);
        }
        
    }
}
