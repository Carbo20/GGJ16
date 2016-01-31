using UnityEngine;
using System.Collections;

public class BigChickenManager : MonoBehaviour {
    private float speed = 5;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(transform.localScale.x < 6.2f)
        {

            transform.localScale = new Vector2(transform.localScale.x + Time.deltaTime*speed, transform.localScale.y + Time.deltaTime * speed);
            //transform.Rotate(Vector3.forward, 30);
        }
        else
        {
            transform.localScale=new Vector2(6.2f, 6.2f);
          //  transform.localRotation = Quaternion.identity;
        }
	}
}
