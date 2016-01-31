using UnityEngine;
using System.Collections;

public class Autodestroy : MonoBehaviour {

    private float m_Duration = 2f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        m_Duration -= Time.deltaTime;
        if(m_Duration <= 0)
            Destroy(this.gameObject);
	}
}
