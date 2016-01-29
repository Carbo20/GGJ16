using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChickenPop : MonoBehaviour {
    [SerializeField]
    List<GameObject>  chickenGo;
    [SerializeField]
    private float popTime;
    private int nbChickenTypes;
    private int chickenPickedRandomly;
    // Use this for initialization
    void Start () {
        nbChickenTypes = 3;//Placeholder
        //ChickenList: (0:Red, 1:Teal,2:Purple...)
        chickenPickedRandomly = Random.Range(0, nbChickenTypes);
        Debug.Log("chickenPickedRandomly: "+ chickenPickedRandomly);
        if (chickenGo == null)
        {
            InvokeRepeating("SpawnChicken", popTime, popTime);
        }
    }

    void SpawnChicken()
    {
        chickenGo = Instantiate(Resources.Load("Prefabs/Chicken"+chickenPickedRandomly)) as GameObject;

    }
    // Update is called once per frame
    void Update () {
	
	}
}
