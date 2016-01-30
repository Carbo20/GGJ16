using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InvocationManager : MonoBehaviour {
    [SerializeField]
    float shiftPosIndicator;
    private GameObject chickenGen;
    ChickenPop chickenPop;
    private int randnb;
    public bool isAccomplished;
    [SerializeField]
    List<int> invocationList;
    enum ChickenType { white, blue, green, red, yellow };
    [SerializeField]
    List<GameObject> indicatorList;
    GameObject indicatorGo;

    // Use this for initialization
    void Start () {
        
        chickenGen = GameObject.FindGameObjectWithTag("ChickenGenerator");
        chickenPop = chickenGen.GetComponent<ChickenPop>();
        // randnb = Random.Range(0, chickenPop.nbChickenTypes);
        PickRandomList(invocationList, invocationList.Count);
    //    Debug.Log("randnb: "+ randnb);
    }
	
	// Update is called once per frame
	void Update () {
        if (isAccomplished)
        {
            PickRandomList(invocationList, invocationList.Count);
            isAccomplished = false;

            for (int i = 0; i < invocationList.Count; i++)
            {
                if (invocationList[i] == (int)ChickenType.white)
                {
                    indicatorGo = Instantiate(indicatorList[0]);
                    indicatorGo.transform.localPosition = new Vector3(0f, i * shiftPosIndicator, 0f);
                }
                if (invocationList[i] == (int)ChickenType.blue)
                {
                    indicatorGo = Instantiate(indicatorList[1]);
                    indicatorGo.transform.localPosition = new Vector3(0f, i * shiftPosIndicator, 0f);
                }
                if (invocationList[i] == (int)ChickenType.green)
                {
                    indicatorGo = Instantiate(indicatorList[2]);
                    indicatorGo.transform.localPosition = new Vector3(0f, i * shiftPosIndicator, 0f);
                }
                if (invocationList[i] == (int)ChickenType.red)
                {
                    indicatorGo = Instantiate(indicatorList[3]);
                    indicatorGo.transform.localPosition = new Vector3(0f, i * shiftPosIndicator, 0f);
                }
                if (invocationList[i] == (int)ChickenType.yellow)
                {
                    indicatorGo = Instantiate(indicatorList[4]);
                    indicatorGo.transform.localPosition = new Vector3(0f, i * shiftPosIndicator, 0f);
                }
            }
        }
	}

    void PickRandomList(List <int> list, int size)
    {
        for (int i = 0; i < size; i++)
        {
            list[i] = Random.Range(0, chickenPop.chickenGoList.Count);
            Debug.Log(" invocationList[i]" + list[i]);
        }
    }
}
