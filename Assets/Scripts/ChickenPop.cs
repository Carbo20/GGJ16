using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChickenPop : MonoBehaviour {
    [SerializeField]
    int maxChickenNb;
    GameObject chickenGo;
    [SerializeField]
    public List<GameObject>  chickenGoList;
    [SerializeField]
    private float popTime;
    private int chickenPickedRandomly;
    private bool isAlreadyCancelled;
    private int nbChickenInstanciated;
    // Use this for initialization
    void Start () {
        nbChickenInstanciated = 0;
        isAlreadyCancelled = false;
        //nbChickenTypes = 3;//Placeholder
        //ChickenList: (0:Red, 1:Teal,2:Purple...)
        chickenPickedRandomly = Random.Range(0, chickenGoList.Count);
        Debug.Log("chickenPickedRandomly: "+ chickenPickedRandomly);
        if (chickenGo == null)
        {
            InvokeRepeating("SpawnChicken", popTime, popTime);
        }
        
    }

    void SpawnChicken()
    {
        chickenGo = Instantiate(chickenGoList[chickenPickedRandomly]);//as GameObject;
        chickenGo.transform.position = transform.position;
        nbChickenInstanciated++;
        chickenPickedRandomly = Random.Range(0, chickenGoList.Count);
    }
    // Update is called once per frame
    void Update () {
	if(chickenGo != null && isAlreadyCancelled == false && nbChickenInstanciated == maxChickenNb)
        {
            isAlreadyCancelled = true;
            CancelInvoke("SpawnChicken");
        }
	}
}
