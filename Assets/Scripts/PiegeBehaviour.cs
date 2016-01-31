using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PiegeBehaviour : MonoBehaviour {

    [SerializeField]
    private List<Sprite> sprites;
    [SerializeField]
    private List<float> times;
    private float elapsedTime = 0;
    private int state = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (elapsedTime < times[state])
        {
            elapsedTime += Time.deltaTime;
        }
        else
        {
            state = (state + 1) % sprites.Count;
            GetComponent<SpriteRenderer>().sprite = sprites[state];
            elapsedTime = 0;

        }

	}
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && state == (sprites.Count -1))
        {
            collision.gameObject.GetComponent<Character_Controller>().Stunned();
        }

        /*if (collision.gameObject.tag == "Chicken" && state == (sprites.Count - 1))
        {
            collision.gameObject.GetComponent<ChickenBehaviour>().Die();
        }*/
    }
}
