using UnityEngine;
using System.Collections;

public enum ChickenColors { WHITE, BLUE, RED, YELLOW, GREEN, NB_CHICKEN_COLOR };

public class SacrificeDalleManager : MonoBehaviour {

    [SerializeField]
    private int playerNumber;
    private ChickenColors nextColorNeeded;
    private int nbChickenSacrificed;
    [SerializeField]
    private int nbChickenNeeded = 5;

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
            if (nbChickenSacrificed == nbChickenNeeded)
            {
                Debug.Log("Player " + playerNumber + " wins!");
            }
        }
        else
        {
            Destroy(chicken.gameObject);
            //todo decrementer nb poule
        }
    }
}
