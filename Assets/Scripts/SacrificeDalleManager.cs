using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ChickenColors { WHITE, BLUE, RED, YELLOW, GREEN, NB_CHICKEN_COLOR };

public class SacrificeDalleManager : MonoBehaviour {

    [SerializeField]
    private int playerNumber;
    private ChickenColors nextColorNeeded;
    [SerializeField]
    private int nbChickenSacrificed;
    [SerializeField]
    private int nbChickenNeeded = 5;
    [SerializeField]
    private GameObject chick;
    [SerializeField]
    List<Sprite> dead_chickens_sprites;
    [SerializeField]
    List<GameObject> dead_chickens_gao;

    // Use this for initialization
    void Start () {
        nextColorNeeded = (ChickenColors)Random.Range(0, (int)ChickenColors.NB_CHICKEN_COLOR);
        nbChickenSacrificed = 0;
    }
	
	// Update is called once per frame
	void Update () {
    }

    public int GetPlayerNumber()
    {
        return playerNumber;
    }

    public ChickenColors GetColorNeeded()
    {
        return nextColorNeeded;
    }

    public void AddChicken(ChickenBehaviour chicken)
    {
        if (chicken.GetColor() == nextColorNeeded)
        {
            nbChickenSacrificed++;
            nextColorNeeded = (ChickenColors)Random.Range(0, (int)ChickenColors.NB_CHICKEN_COLOR);
            dead_chickens_gao[nbChickenSacrificed].GetComponent<SpriteRenderer>().sprite = dead_chickens_sprites[(int)chicken.GetColor()];
            dead_chickens_gao[nbChickenSacrificed].SetActive(true);
            if (nbChickenSacrificed == nbChickenNeeded)
            {
                Debug.Log("Player " + playerNumber + " wins!");
            }
        }
        Destroy(chicken.gameObject);
        //todo decrementer nb poule
    }
}
