using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ChickenColors { WHITE, BLUE, RED, YELLOW, GREEN, NB_CHICKEN_COLOR };

public class SacrificeDalleManager : MonoBehaviour {

    [SerializeField]
    private int playerNumber;
    [SerializeField]
    private ChickenColors nextColorNeeded;
    [SerializeField]
    private int nbChickenSacrificed;
    [SerializeField]
    private int nbChickenNeeded = 5;
    [SerializeField]
    List<Sprite> dead_chickens_sprites;
    [SerializeField]
    List<GameObject> dead_chickens_gao;
    [SerializeField]
    List<Color> color_indicator;
    [SerializeField]
    GameObject indicator_gao;

    // Use this for initialization
    void Start () {
        nextColorNeeded = (ChickenColors)Random.Range(0, (int)ChickenColors.NB_CHICKEN_COLOR);
        indicator_gao.GetComponent<SpriteRenderer>().color = color_indicator[(int)nextColorNeeded];
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
            
            nextColorNeeded = (ChickenColors)Random.Range(0, (int)ChickenColors.NB_CHICKEN_COLOR);
            indicator_gao.GetComponent<SpriteRenderer>().color = color_indicator[(int)nextColorNeeded];
            dead_chickens_gao[nbChickenSacrificed].GetComponent<SpriteRenderer>().sprite = dead_chickens_sprites[(int)chicken.GetColor()];
            StartCoroutine("SetActiveChicken");

        }
        Destroy(chicken.gameObject);
        GameManager.nbChickens--;
    }

    void EndGame()
    {
        Debug.Log("Player " + playerNumber + " wins!");
        GameObject[] playerArray = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i <= playerArray.Length; ++i)
        {
            if (playerArray[i].GetComponent<Character_Controller>().PlayerNumber != playerNumber)
                playerArray[i].GetComponent<Character_Controller>().Dies();
        }
        GameObject[] chicksArray = GameObject.FindGameObjectsWithTag("Chicken");
        foreach (GameObject chick in chicksArray)
        {
            GameObject corpse = Instantiate(chick.GetComponent<ChickenBehaviour>().dead_chicken);
            corpse.transform.position = chick.GetComponent<ChickenBehaviour>().transform.position;
            Destroy(chick);
        }
        //todo: 
        //display winner
        //block player movements
        //play demonlord animation
    }

    IEnumerator SetActiveChicken()
    {
        yield return new WaitForSeconds(0.8f);
        dead_chickens_gao[nbChickenSacrificed].SetActive(true);
        nbChickenSacrificed++;
        if (nbChickenSacrificed == nbChickenNeeded)
        {
            Debug.Log("Player " + playerNumber + " wins!");
            indicator_gao.SetActive(false);
            EndGame();
        }

    }
}
