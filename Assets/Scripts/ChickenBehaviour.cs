using UnityEngine;
using System.Collections;

public class ChickenBehaviour : MonoBehaviour
{
    public float speed = 5f;
    public float countdown = 0.7f;
    public Vector2 direction;
    public Vector2 henhouse;
    public chickenState state;
    public enum chickenState { inHenhouse, Captured, Returning, Flying };
    [SerializeField]
    //private float flytime = 0.0f;
    private float flySpeed = 8.2f;
    private float roamingSpeed = 0.7f;
    private float runningSpeed = 2.1f;
    [SerializeField]
    public ChickenColors color;

    #region Ajouts LD
    private float m_FlyingChickenDuration = 0.5f;
    private float m_ChickenLaunched = 0f;
    public int m_PlayerThrowing;
    const float k_CollisionRadius = 0.6f;
    [SerializeField] private LayerMask m_WhatIsPlayer;
    [SerializeField] private LayerMask m_WhatIsChicken;


    public GameObject dead_chicken;
    [SerializeField] GameObject explosion;

    [SerializeField] private bool m_Explosive = false;
    const float k_ExplosionRadius = 1f;

    bool dead = false;

    #endregion

    // Use this for initialization
    void Start()
    {
        //state = chickenState.inHenhouse;
        //henhouse.Set(transform.position.x, transform.position.y);
        henhouse.Set(0, 0);
        speed = roamingSpeed;
        while (direction.x == 0 && direction.y == 0)
            direction.Set(Random.Range(-10, 10), Random.Range(-10, 10));
        direction.Normalize();
    }

    // Update is citalled once per frame
    void Update()
    {
        if (state == chickenState.Captured)
        {
            transform.localPosition = new Vector2(-0.01f, -0.02f);
        }
        else if (state == chickenState.Flying)
        {
            //Debug.Log(string.Format("i fly at {0} for {1}", speed, flytime));
            //flytime -= Time.deltaTime;
            CheckCollision();

            m_ChickenLaunched += Time.deltaTime;
            if (m_ChickenLaunched >= m_FlyingChickenDuration)
                EndThrow();
        }
        else
        {
            //time = 0.0f;
            if (transform.position.x > 3)
            {
                state = chickenState.Returning;
                speed = runningSpeed;
                direction.Set(henhouse.x - transform.position.x, henhouse.y - transform.position.y);
                direction.Normalize();
            }
            else
            {
                state = chickenState.inHenhouse;
                speed = roamingSpeed;
                countdown -= Time.deltaTime;
                if (countdown <= 0.0f)
                {
                    do direction.Set(Random.Range(-10, 10), Random.Range(-10, 10));
                    while (direction.x == 0 && direction.y == 0);
                    direction.Normalize();
                    countdown = 0.7f;
                }
            }
            transform.Translate(direction * Time.deltaTime * speed);
            if (direction.x < 0.0f)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (direction.x > 0.0f)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }

    private void EndThrow()
    {
        //lancer l'animation d'explosion
        //poser le cadavre
        m_ChickenLaunched = 0;
        if (transform.position.x > 3)
            state = chickenState.Returning;
        else
            state = chickenState.inHenhouse;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
    }

    private void CheckCollision()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, k_CollisionRadius, m_WhatIsPlayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == "Player" && m_PlayerThrowing != colliders[i].gameObject.GetComponent<Character_Controller>().PlayerNumber)
            {
                colliders[i].gameObject.GetComponent<Character_Controller>().Stunned();
                Die();
                break;
            }
        }
    }

    public void Die()
    {
        if (!dead)
        {
            dead = true;
            GameManager.nbChickens--;
            GameObject corpse = Instantiate(dead_chicken);
            corpse.transform.position = transform.position;

            if (m_Explosive)
            {
                //lancer l'explosion
                GameObject explode = Instantiate(explosion);
                explode.transform.position = transform.position;

                // Tuer tous les chickens dans un radius autour
                Collider2D[] collidersChicken = Physics2D.OverlapCircleAll(transform.position, k_ExplosionRadius, m_WhatIsChicken);
                for (int i = 0; i < collidersChicken.Length; i++)
                {
                    if (collidersChicken[i].gameObject.tag == "Chicken")
                    {
                        collidersChicken[i].gameObject.GetComponent<ChickenBehaviour>().Die();
                    }
                }
                // Stun tous les joueurs dans ce radius
                Collider2D[] collidersPlayer = Physics2D.OverlapCircleAll(transform.position, k_ExplosionRadius, m_WhatIsPlayer);
                for (int i = 0; i < collidersPlayer.Length; i++)
                {
                    if (collidersPlayer[i].gameObject.tag == "Player")
                    {
                        collidersPlayer[i].gameObject.GetComponent<Character_Controller>().Stunned();
                    }
                }
            }
            if (transform.parent != null && transform.gameObject.tag == "Player")
            {
                transform.parent.GetComponent<Character_Controller>().m_HaveChicken = false;
                transform.parent.GetComponent<Character_Controller>().m_Anim.SetBool("carrying", false);
                transform.parent.GetComponent<Character_Controller>().arm.SetActive(false);
                transform.parent.GetComponent<Character_Controller>().chicken = null;
            }


            Destroy(this.gameObject);
        }
    }

    public void Flying(Vector2 vFly)
    {
        state = ChickenBehaviour.chickenState.Flying;
        m_ChickenLaunched = 0;

        //GetComponent<Rigidbody2D>().velocity = new Vector2(vFly.x * 10, vFly.y * 10);
        //speed = flySpeed;
        //flytime = 10.0f;
    }

    /*void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log("triggerd");
        if (state != chickenState.Captured && other.gameObject.name == "Henhouse")
        {
            //Debug.Log("exited");
            state = chickenState.Returning;
        }
    }*/

    /*void OnTriggerEnter2D(Collider2D other)
    {
        if (state == chickenState.Returning && other.gameObject.name == "Henhouse")
            state = chickenState.inHenhouse;
    }*/

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            //Debug.Log("hitsomething: ");
            GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
            if (m_ChickenLaunched > 0 && m_ChickenLaunched < m_FlyingChickenDuration)
            {

                Die();
                //EndThrow();
            }

            direction.x *= -1;
            direction.y *= -1;
            direction.Normalize();
        }
    }

    public ChickenColors GetColor()
    {
        return color;
    }
}