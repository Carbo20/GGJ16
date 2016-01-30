using UnityEngine;
using System.Collections;

public class ChickenExplosion : MonoBehaviour {

    private float m_FlyingChickenDuration;
    private float m_ChickenLaunched;    

    // Use this for initialization
    void Start()
    {
        m_FlyingChickenDuration = 1f;
        m_ChickenLaunched = 0;
    }

    // Update is called once per frame
    void Update()
    {
        m_ChickenLaunched += Time.deltaTime;
        if (m_ChickenLaunched >= m_FlyingChickenDuration)
            EndExplosion();
    }

    private void EndExplosion()
    {
        //lancer l'animation d'explosion
        //poser le cadavre
        GetComponent<ChickenBehaviour>().enabled = true;
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

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("CHICKEN Stunned!");

            if (collision.gameObject.GetComponent<Character_Controller>().m_HaveChicken)
            {
                collision.gameObject.GetComponent<Character_Controller>().chicken.transform.parent = null;
                collision.gameObject.GetComponent<Character_Controller>().m_HaveChicken = false;
                collision.gameObject.GetComponent<Character_Controller>().m_Anim.SetBool("carrying", false);
                collision.gameObject.GetComponent<Character_Controller>().arm.SetActive(false);

                collision.gameObject.GetComponent<Character_Controller>().chicken.GetComponent<ChickenBehaviour>().state = ChickenBehaviour.chickenState.Returning;

                collision.gameObject.GetComponent<Character_Controller>().chicken = null;
            }

            collision.gameObject.GetComponent<Character_Controller>().m_Stunned = true;

            collision.gameObject.GetComponent<Character_Controller>().m_IsRolling = false;
            collision.gameObject.GetComponent<Character_Controller>().m_Anim.SetBool("dashing", false);


            collision.gameObject.GetComponent<Character_Controller>().m_StunLeft = collision.gameObject.GetComponent<Character_Controller>().m_StunDuration;
            collision.gameObject.GetComponent<Character_Controller>().m_Rigidbody2D.velocity = new Vector2(0f, 0f);
            collision.gameObject.GetComponent<Character_Controller>().m_Rigidbody2D.isKinematic = true;

            Destroy(this.gameObject);
        }
    }
}
