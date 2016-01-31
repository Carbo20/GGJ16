using UnityEngine;
using System.Collections;

public class ChickenExplosion : MonoBehaviour {

    
    
    [SerializeField] private float m_StunDuration = 0.5f;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


    }

    

    private void CheckCollision()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, k_CollisionRadius, m_WhatIsPlayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == "Player" && m_PlayerThrowing != colliders[i].gameObject.GetComponent<Character_Controller>().PlayerNumber)
            {
                colliders[i].gameObject.GetComponent<Character_Controller>().Stunned();

                Destroy(this.gameObject);
            }
        }
    }

    //public void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        Debug.Log("CHICKEN Stunned!");

    //        if (collision.gameObject.GetComponent<Character_Controller>().m_HaveChicken)
    //        {
    //            collision.gameObject.GetComponent<Character_Controller>().chicken.transform.parent = null;
    //            collision.gameObject.GetComponent<Character_Controller>().m_HaveChicken = false;
    //            collision.gameObject.GetComponent<Character_Controller>().m_Anim.SetBool("carrying", false);
    //            collision.gameObject.GetComponent<Character_Controller>().arm.SetActive(false);

    //            collision.gameObject.GetComponent<Character_Controller>().chicken.GetComponent<ChickenBehaviour>().state = ChickenBehaviour.chickenState.Returning;

    //            collision.gameObject.GetComponent<Character_Controller>().chicken = null;
    //        }

    //        collision.gameObject.GetComponent<Character_Controller>().m_Stunned = true;

    //        collision.gameObject.GetComponent<Character_Controller>().m_IsRolling = false;
    //        collision.gameObject.GetComponent<Character_Controller>().m_Anim.SetBool("dashing", false);


    //        collision.gameObject.GetComponent<Character_Controller>().m_StunLeft = collision.gameObject.GetComponent<Character_Controller>().m_StunDuration;
    //        collision.gameObject.GetComponent<Character_Controller>().m_Rigidbody2D.velocity = new Vector2(0f, 0f);
    //        collision.gameObject.GetComponent<Character_Controller>().m_Rigidbody2D.isKinematic = true;

    //        Destroy(this.gameObject);
    //    }
    //}
}
