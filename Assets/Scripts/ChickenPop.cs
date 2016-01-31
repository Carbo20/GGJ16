using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChickenPop : MonoBehaviour {

    GameObject chickenGo;
    [SerializeField]
    public List<GameObject>  chickenGoList;
    [SerializeField]
    public static bool run;
    private float countdown;
    private int number = 0;
    [SerializeField] private int numTurnToSpawnExplo = 5;

    [SerializeField]
    private Transform parent;

    // Use this for initialization
    void Start () {
        run = true;
        //isAlreadyCancelled = false;
        //nbChickenTypes = 3;//Placeholder
        //ChickenList: (0:Red, 1:Teal,2:Purple...)
        //chickenPickedRandomly = Random.Range(0, chickenGoList.Count);
        //Debug.Log("chickenPickedRandomly: "+ chickenPickedRandomly);
        //if (chickenGo == null)
        //{
        //    InvokeRepeating("SpawnChicken", popTime + Random.Range(0.1f, 1.0f), popTime);
        //}
    }

    // Update is called once per frame
    void Update ()
    {
        if(run && GameManager.nbChickens < GameManager.maxChickens)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0.0f)
            {
                number++;
                if (number == numTurnToSpawnExplo)
                {
                    number = 0;
                    chickenGo = Instantiate(chickenGoList[5]);// ExploChicken;
                    chickenGo.transform.parent = parent;
                    chickenGo.transform.position = transform.position;
                    countdown = Random.Range(1.0f, 5.0f);
                }
                chickenGo = Instantiate(chickenGoList[Random.Range(0, chickenGoList.Count-1)]);//as GameObject;
                chickenGo.transform.parent = parent;
                chickenGo.transform.position = transform.position;
                GameManager.nbChickens++;
                countdown = Random.Range(1.0f, 5.0f);
            }
            //chickenPickedRandomly = Random.Range(0, chickenGoList.Count);
        }
    }
}
