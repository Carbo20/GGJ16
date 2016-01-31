using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static int nbChickens = 0;
    public static int maxChickens = 100;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
	}
}
