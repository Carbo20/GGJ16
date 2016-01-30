using UnityEngine;
using System.Collections;

public class SacrificeDalleManager : MonoBehaviour {

    [SerializeField]
    private GameObject[] dalles;

    [SerializeField]
    private Sprite[] dallesSprites;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void ChangeDalleSprite(int playerId, int chickenCount)
    {
        dalles[playerId].GetComponent<SpriteRenderer>().sprite = dallesSprites[chickenCount];
    }
}
