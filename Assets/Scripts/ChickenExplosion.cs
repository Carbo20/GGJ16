using UnityEngine;
using System.Collections;

public class ChickenExplosion : MonoBehaviour {

    public int m_PlayerThrowing;
    private float m_FlyingChickenDuration;
    private float m_ChickenLaunched;
    const float k_CollisionRadius = 0.6f;
    [SerializeField] private LayerMask m_WhatIsPlayer;
    [SerializeField] private float m_StunDuration = 0.5f;

    // Use this for initialization
    void Start()
    {
        m_FlyingChickenDuration = 1f;
        m_ChickenLaunched = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollision();

        m_ChickenLaunched += Time.deltaTime;
        if (m_ChickenLaunched >= m_FlyingChickenDuration)
            EndExplosion();
    }

    private void EndExplosion()
    {
        //lancer l'animation d'explosion
        //poser le cadavre
        //GetComponent<ChickenBehaviour>().enabled = true;
        if(transform.position.x > 3)
            GetComponent<ChickenBehaviour>().state = ChickenBehaviour.chickenState.Returning;
        else
            GetComponent<ChickenBehaviour>().state = ChickenBehaviour.chickenState.inHenhouse;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        enabled = false;
        //Destroy(this.gameObject);
    }

    private void CheckCollision()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, k_CollisionRadius, m_WhatIsPlayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == "Player" && m_PlayerThrowing != colliders[i].gameObject.GetComponent<Character_Controller>().PlayerNumber)
            {
                colliders[i].gameObject.GetComponent<Character_Controller>().m_Stunned = true;
                colliders[i].gameObject.GetComponent<Character_Controller>().m_StunLeft = m_StunDuration;
                colliders[i].gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);

                if (colliders[i].gameObject.GetComponent<Character_Controller>().m_HaveChicken)
                {
                    colliders[i].gameObject.GetComponent<Character_Controller>().chicken.transform.parent = null;
                    colliders[i].gameObject.GetComponent<Character_Controller>().m_HaveChicken = false;
                    colliders[i].gameObject.GetComponent<Character_Controller>().m_Anim.SetBool("carrying", false);
                    colliders[i].gameObject.GetComponent<Character_Controller>().arm.SetActive(false);

                    if (transform.position.x > 3)
                        colliders[i].gameObject.GetComponent<Character_Controller>().chicken.GetComponent<ChickenBehaviour>().state = ChickenBehaviour.chickenState.Returning;
                    else
                        colliders[i].gameObject.GetComponent<Character_Controller>().chicken.GetComponent<ChickenBehaviour>().state = ChickenBehaviour.chickenState.inHenhouse;

                    colliders[i].gameObject.GetComponent<Character_Controller>().chicken = null;
                }
                else if (colliders[i].gameObject.GetComponent<Character_Controller>().m_IsRolling)
                {
                    colliders[i].gameObject.GetComponent<Character_Controller>().timeRolling = 0;
                    colliders[i].gameObject.GetComponent<Character_Controller>().m_IsRolling = false;
                    colliders[i].gameObject.GetComponent<Character_Controller>().m_Rigidbody2D.velocity = new Vector2(0f, 0f);
                    colliders[i].gameObject.GetComponent<Character_Controller>().m_Anim.SetBool("dashing", false);
                }

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
