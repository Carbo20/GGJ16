using UnityEngine;
using System.Collections;

public class plumeManager : MonoBehaviour {

    private bool up = true;
    private float speed = .3f;
    private float range= .1f;
    private Vector2 position;

	// Use this for initialization
	void Start () {
        position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (up)
        {
            if (transform.position.y < position.y + range)
            {
                transform.Translate(Vector2.up * speed * Time.deltaTime);
            }
            else
            {
                up = false;
            }
        }
        else
        {
            if (transform.position.y > position.y - range)
            {
                transform.Translate(Vector2.down * speed * Time.deltaTime);
            }
            else
            {
                up = true;
            }
        }
	}
}
