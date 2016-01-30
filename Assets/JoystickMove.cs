using UnityEngine;
using System.Collections;

public class JoystickMove : MonoBehaviour {

    public float speed = 5f;
    public int PlayerNumber;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            Debug.Log("joystick 1 button 0");
        }
        if (Input.GetKeyDown(KeyCode.Joystick2Button0))
        {
            Debug.Log("joystick 2 button 0");
        }

        if (Mathf.Abs(Input.GetAxis("HorizontalP"+PlayerNumber)) > 0.1f)
        {
            transform.Translate(new Vector3(Input.GetAxis("HorizontalP"+PlayerNumber) * speed * Time.deltaTime, 0, 0));
        }
        if (Mathf.Abs(Input.GetAxis("VerticalP" + PlayerNumber)) > 0.1f)
        {
            transform.Translate(new Vector3(0, Input.GetAxis("VerticalP"+PlayerNumber) * speed * Time.deltaTime, 0));
        }
       
	}
}
