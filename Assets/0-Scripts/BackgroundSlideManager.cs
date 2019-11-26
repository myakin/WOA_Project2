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

    
    // key = objenin ismi, value = objeye ait pool
    Dictionary <string, List<GameObject>> columnsPool = new Dictionary<string, List<GameObject>>();


    private void OnEnable() {
        // GetComponent<MeshRenderer>().bounds.max.x objenin pozitif x ekseninde o anki dunya pozisyonunu doduruyor (lokal pozisyonu degil)
        // o anki dunya pozisyonundan o anki cismin pozisyonunun x'ini cikarirsam objenin gercek pozitif x genisligini bulurum
        // aynisi GetComponent<MeshRenderer>().bounds.min.x icin de gecerli
        widthOnPositiveX = boundsRefObj.GetComponent<MeshRenderer>().bounds.max.x - transform.position.x;
        // widthOnNegativeX = Mathf.Abs(Mathf.Abs(boundsRefObj.GetComponent<MeshRenderer>().bounds.min.x) - transform.position.x);
        // Debug.Log(gameObject.name+": widthOnPositiveX="+widthOnPositiveX + " widthOnNegativeX=" + widthOnNegativeX);

        nonAbsWidthOnNegativeX = boundsRefObj.GetComponent<MeshRenderer>().bounds.min.x - transform.position.x;
        widthOnNegativeX = Mathf.Abs(Mathf.Abs(boundsRefObj.GetComponent<MeshRenderer>().bounds.min.x) - transform.position.x);
        //Debug.Log(gameObject.name+": widthOnPositiveX="+widthOnPositiveX + " widthOnNegativeX=" + widthOnNegativeX + " nonAbsWidthOnNegativeX="+nonAbsWidthOnNegativeX);

        numberOfUprightColumns=DecideHowManyColumnsWillBeGenerated();
        for (int i=0; i<numberOfUprightColumns; i++) {
            string prefabName = PickAnObjectToGenerate();

            GameObject newColumn = null;
            // direkt columnsPool[prefabName] diye ulasmaya calismak yerine
            // soyle bir check yapmak daha guvenli. cunku eger [prefabName] indexi
            // yoksa hata alirim
            if (columnsPool.ContainsKey(prefabName) && columnsPool[prefabName].Count>0) {
                // 1.listeden 0 indexindeki objeyi cekmek istiyorum
                // 2.listeyi guncellemek istiyorum (0 indexi silmeyi gerektiriyor)
                // o obje ya sahnededir ya da listededir; hem sahnede hem listede olamaz
                // yani istedigim obje su: columnsPool[prefabName][0]
                newColumn = columnsPool[prefabName][0]; // pool'dan cektim, ama henuz pool'umu guncellemedim
                columnsPool[prefabName].RemoveAt(0);

            } else {
                GameObject newColumnPrefab = Resources.Load(prefabName) as GameObject;
                newColumn = Instantiate(newColumnPrefab, columnsParent.transform);
            }

            
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

    private void OnDisable() {
        string nameOfThisObj = gameObject.name;
        // sahnedeye bizim yerlestirdigimiz obje ise isim SlidingBackground olacak
        // sahneye procedural generation ile yerlestirdigimiz obje ise isim
        // SlidingBackground(Clone) olacak
        // bizim her sekilde SlidingBackground stringine ihtiyacimiz var
        if (nameOfThisObj.Contains("(Clone)")) {
            nameOfThisObj = nameOfThisObj.Substring(0, nameOfThisObj.Length-7);
            //nameOfThisObj = nameOfThisObj.Replace("(Clone)","");
            // Obje(Clone)Kolon
            // Obje(Clone)Kolon(Clone)
        }

        //gameObject.SetActive(false);
        foreach(KeyValuePair<string, List<GameObject>> poolObject in columnsPool) {
            if (!columnsPool.ContainsKey(nameOfThisObj)) {
                List<GameObject> poolOfThisNameSpace = new List<GameObject>(); 
                poolOfThisNameSpace.Add(gameObject);
                columnsPool.Add(nameOfThisObj, poolOfThisNameSpace);
            } else {
                columnsPool[nameOfThisObj].Add(gameObject);
            }
        }
        occupiedXs.Clear();
    }


    private void Start() {
        
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
        //Debug.Log("nonAbsWidthOnNegativeX="+nonAbsWidthOnNegativeX+" widthOnPositiveX="+widthOnPositiveX+" posX="+posX);
        for (int i=0; i<occupiedXs.Count; i++) {
            if (Mathf.Abs(occupiedXs[i] - posX) < checkXDelta) {
                return DeterminePosX();
            } 
        }
        occupiedXs.Add(posX);
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
        if (GameController.gameController.isGamePlaying) {
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
                isInstantiating=false;
                GameController.gameController.AddToBackgroundPool(gameObject);
            }
        }
        
    }
}
