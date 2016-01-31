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

    [SerializeField] private LayerMask m_WhatIsPlayer;
    [SerializeField] private LayerMask m_WhatIsChicken;
    [SerializeField] private GameObject m_TopLeft;
    [SerializeField] private GameObject m_BottomRight;

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
        if (state == (sprites.Count - 1))
            CheckCollision();

	}

    private void CheckCollision()
    {
        Vector2 VTopLeft = new Vector2(m_TopLeft.transform.position.x, m_TopLeft.transform.position.y);
        Vector2 VBottomRight = new Vector2(m_BottomRight.transform.position.x, m_BottomRight.transform.position.y);

        Collider2D[] PlayerColliders = Physics2D.OverlapAreaAll(VTopLeft, VBottomRight, m_WhatIsPlayer);
        for (int i = 0; i < PlayerColliders.Length; i++)
        {
            if (PlayerColliders[i].gameObject.tag == "Player")
            {
                PlayerColliders[i].gameObject.GetComponent<Character_Controller>().Stunned();
            }
        }

        Collider2D[] chickenColliders = Physics2D.OverlapAreaAll(VTopLeft, VBottomRight, m_WhatIsChicken);
        for (int i = 0; i < chickenColliders.Length; i++)
        {
            if (chickenColliders[i].gameObject.tag == "Chicken")
            {
                chickenColliders[i].gameObject.GetComponent<ChickenBehaviour>().Die();
            }
        }
    }

    //public void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Player" && state == (sprites.Count -1))
    //    {
    //        collision.gameObject.GetComponent<Character_Controller>().Stunned();
    //    }

    //    /*if (collision.gameObject.tag == "Chicken" && state == (sprites.Count - 1))
    //    {
    //        collision.gameObject.GetComponent<ChickenBehaviour>().Die();
    //    }*/
    //}
}
