using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// IMPORTANT :  MoveToward, Lerp, AddVelocity, AddForce, Translate (déconseillé) permettent d'effectuer des mouvements de différentes manières
/// CAREFULL OF GROUNDCHECK, its buggy
/// its position should be perfect or else Multiple jumps bug!
/// k_GroundedRadius value should be watched too
/// mb change collision detection to check if grounded
/// </summary>
public class Character_Controller : MonoBehaviour
{

    #region Test des différents types de mouvements

    public enum JumpType
    {
        JUMPVELOCITY, JUMPFORCE
    }

    public enum RollType
    {
        ROLLTRANSLATE, ROLLVELOCITY, ROLLFORCE
    }

    [SerializeField] private JumpType jumpType = JumpType.JUMPVELOCITY;
    [SerializeField] private RollType rollType = RollType.ROLLVELOCITY;

    #endregion

    private enum Dir { NONE, EAST, WEST, SOUTH, NORTH };
    private Dir characterDir;    // public only for testing
    private Transform myTransform;

    public int PlayerNumber;

    [SerializeField]
    public GameObject arm;

    #region ROLL ATTACK
    private bool m_Roll;
    ////[SerializeField] private float m_rollDistance;
    [SerializeField] private float m_RollCD = 2f;
    [SerializeField] private float m_rollSpeed = 1f;
    [SerializeField] private float m_rollDuration = 1f;
    [SerializeField] private float k_RollAttackRadius = 0.4f;
    [SerializeField] private LayerMask m_WhatIsCharacter;
    private float m_XDirection = 0f;
    private float m_YDirection = 0f;
    ////[SerializeField] private float m_RollForce;
    
    private bool m_IsRolling;
    private float timeRolling;
    
    private float m_rollCDLeft;
    private bool m_Stunned = false;
    [SerializeField] private float m_StunDuration = 0.5f;
    private float m_StunLeft = 0f;
    #endregion

    #region RUN&SPRINT
    [SerializeField] private float m_MaxSpeed = 10f;                // The normal speed the player can travel in the axis.
    [SerializeField] private float m_MinSpeed = -10f;                // The normal speed the player can travel in the axis.
    #endregion

    #region STUNNED
    private bool stunned = false;
    private float stunDuration = 0f;
    #endregion

    #region CHICKEN
    public bool m_HaveChicken = false;
    [SerializeField] private float m_SpeedModWithChicken = 0.5f;
    [SerializeField] private Transform m_ChickenCheck;   
    const float k_ChickenCheckRadius = 0.4f;
    [SerializeField] private LayerMask m_WhatIsChicken;
    private GameObject chicken;
    #endregion

    #region TEST ASSET 2D
    private Animator m_Anim;                         // Reference to the player's animator component.
    public Rigidbody2D m_Rigidbody2D;                  
    private bool m_FacingRight = true;                  // For determining which way the player is currently facing.
    #endregion

    #region Monobehaviour

    private void Awake()
    {
        // Setting up references.
        //m_ChickenCheck = transform.Find("ChickenCheck");
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

	void Start () {
        m_IsRolling = false;
        m_rollCDLeft = 0;
        myTransform = this.transform;
    }
	
	// Update is called once per frame
	void Update () {
        if (!m_Stunned)
        {
            // Read the inputs.    
            float h = Mathf.Abs(Input.GetAxis("HorizontalP" + PlayerNumber)) < 0.2 ? 0 : Input.GetAxis("HorizontalP" + PlayerNumber);
            float v = Mathf.Abs(Input.GetAxis("VerticalP" + PlayerNumber)) < 0.2 ? 0 : Input.GetAxis("VerticalP" + PlayerNumber);

            if (!m_HaveChicken)
                CheckRoll(h, v);

            Move(h, v, m_Roll);

            m_Roll = false;

            if (!m_HaveChicken)
                GrabChicken();
            else
                ThrowChicken(h, v);
        }
        else
            CheckStunned();
    }

    private void FixedUpdate()
    {

    }
    #endregion

    #region COMPUTE

    /// <summary>
    /// Get the current input direction from the inputs list and set the character direction.
    /// </summary>
    /// <returns></returns>
    private Dir ComputeDirection(float x, float y)
    {
        if (y > 0.1 && x >= -0.1 && x <= 0.1)
            return Dir.NORTH;
        else if (y < -0.1 && x >= -0.1 && x <= 0.1)
            return Dir.SOUTH;

        if (x > 0.1)
            return Dir.EAST;
        else if (x >= -0.1 && x <= 0.1 && y >= -0.1 && y <= 0.1)
            return Dir.NONE;

        else
            return Dir.WEST;
    }
    #endregion

    #region MOVING THE CHARACTER 
    public void Move(float moveX, float moveY, bool roll)
    {
        if (!m_IsRolling) 
        {
            float t_moveSpeedX, t_moveSpeedY;
            float SpeedMult=1f;

            if(m_HaveChicken)
                SpeedMult = m_SpeedModWithChicken;
 
            t_moveSpeedX = Mathf.Min(Mathf.Max(m_MinSpeed, moveX * m_MaxSpeed * Time.deltaTime * SpeedMult), m_MaxSpeed);
            t_moveSpeedY = Mathf.Min(Mathf.Max(m_MinSpeed, moveY * m_MaxSpeed * Time.deltaTime * SpeedMult), m_MaxSpeed);

            if (Mathf.Abs(t_moveSpeedX) < 0.1f && Mathf.Abs(t_moveSpeedY) < 0.1f)
            {
                m_Anim.SetBool("moving", false);
            }
            else
            {
                m_Anim.SetBool("moving", true);
            }

            m_Rigidbody2D.velocity = new Vector2(t_moveSpeedX, t_moveSpeedY); // Move the character

            if (moveX > 0 && !m_FacingRight) // If the input is moving the player right and the player is facing left...
                Flip();
            else if (moveX < 0 && m_FacingRight) // Otherwise if the input is moving the player left and the player is facing right...
                Flip();
        }
        
        Roll(roll);
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    #endregion

    #region ROLL

    private void CheckRoll(float x, float y)
    {
        if (m_rollCDLeft >= 0)
            m_rollCDLeft -= Time.deltaTime;

        if (!m_IsRolling)
        {
            if ( (x!=0 || y!=0) && m_rollCDLeft <= 0 && (Input.GetButtonDown("RollP" + PlayerNumber)))
            {
                m_Roll = true;
                m_rollCDLeft = m_RollCD;
                Debug.Log("ROLLATTACK  X= " + x + "  Y= " + y);
                m_IsRolling = true;
                timeRolling = m_rollDuration;
                m_XDirection = x;
                m_YDirection = y;
                m_Anim.SetBool("dashing", true);
            }
        }
        else
        {
            if (timeRolling > 0)
            {
                timeRolling -= Time.deltaTime;

                //Collider2D[] colliders = Physics2D.OverlapCircleAll(m_ChickenCheck.position, k_RollAttackRadius, m_WhatIsCharacter);
                //for (int i = 0; i < colliders.Length; i++)
                //{
                //    if (colliders[i].gameObject.tag == "Player" && colliders[i].gameObject.GetComponent<Character_Controller>().PlayerNumber != PlayerNumber)
                //    {
                //        Debug.Log("Player " + PlayerNumber + " is stunning player " + colliders[i].gameObject.GetComponent<Character_Controller>().PlayerNumber);
                //        colliders[i].gameObject.GetComponent<Character_Controller>().m_Stunned = true;
                //        colliders[i].gameObject.GetComponent<Character_Controller>().m_StunLeft = m_StunDuration;
                //        colliders[i].gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
                //    }
                //}
            }
            else
            {
                m_IsRolling = false;
                Debug.Log("ROLLATTACK END");
                m_Rigidbody2D.velocity = new Vector2(0f, 0f);
                m_Anim.SetBool("dashing", false);
            }
        }
    }

    private void Roll(bool roll)
    {
        if (roll)
            m_rollCDLeft = m_RollCD;

        if (m_IsRolling && rollType == RollType.ROLLVELOCITY)
            m_Rigidbody2D.velocity = new Vector2( m_XDirection * m_rollSpeed * Time.deltaTime, m_YDirection * m_rollSpeed * Time.deltaTime);
    }
    #endregion

    #region STUNNED
    private void CheckStunned()
    {
        if (m_StunLeft > 0)
        {
            m_StunLeft -= Time.deltaTime;
            m_Anim.SetBool("stunned", true);
        }
        else
        {
            m_Stunned = false;
            m_Anim.SetBool("stunned", false);
            GetComponent<Rigidbody2D>().isKinematic = false;
        }
    }
    #endregion

    #region CHICKEN
    private void GrabChicken()
    {
        if (!m_IsRolling && Input.GetButtonDown("ManageChickenP" + PlayerNumber))
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_ChickenCheck.position, k_ChickenCheckRadius, m_WhatIsChicken);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject.tag == "Chicken")
                {
                    colliders[i].gameObject.GetComponent<ChickenBehaviour>().state = ChickenBehaviour.chickenState.Captured;
                    colliders[i].gameObject.transform.parent = myTransform;
                    chicken = colliders[i].gameObject;
                    m_HaveChicken = true;
                    m_Anim.SetBool("carrying", true);
                    arm.SetActive(true);
                    break;
                }
            }
        }
        else if (!m_IsRolling && Input.GetButtonDown("PutChickenDownP" + PlayerNumber))
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_ChickenCheck.position, k_ChickenCheckRadius, m_WhatIsChicken);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject.tag == "Chicken")
                {
                    colliders[i].gameObject.GetComponent<ChickenBehaviour>().state = ChickenBehaviour.chickenState.Captured;
                    colliders[i].gameObject.transform.parent = myTransform;
                    chicken = colliders[i].gameObject;
                    m_HaveChicken = true;
                    m_Anim.SetBool("carrying", true);
                    arm.SetActive(true);
                    break;
                }
            }
        }
            //grab chicken
    }

    private void ThrowChicken(float moveX, float moveY)
    {
        if (chicken != null)
        {
            if (Input.GetButtonDown("ManageChickenP" + PlayerNumber))
            {
                chicken.transform.parent = null;
                m_HaveChicken = false;
                m_Anim.SetBool("carrying", false);
                arm.SetActive(false);

                //todo mettre en inhenhouse si dans le poulailler ou returning sinon
                if (Mathf.Abs(moveX) < 0.1f && Mathf.Abs(moveY) < 0.1f)
                {
                    chicken.GetComponent<ChickenBehaviour>().state = ChickenBehaviour.chickenState.Returning;
                }
                else
                {
                    chicken.GetComponent<ChickenBehaviour>().enabled = false;
                    chicken.GetComponent<Rigidbody2D>().velocity = new Vector2(moveX * 10, moveY * 10);
                    chicken.GetComponent<ChickenExplosion>().enabled = true;
                    //rajouter un cd de mort pour le chicken
                }
                chicken = null;
            }
            else if (Input.GetButtonDown("PutChickenDownP" + PlayerNumber))
            {
                chicken.transform.parent = null;
                m_HaveChicken = false;
                m_Anim.SetBool("carrying", false);
                arm.SetActive(false);

                chicken.GetComponent<ChickenBehaviour>().state = ChickenBehaviour.chickenState.Returning;

                chicken = null;
            }
        }
            //Throw chicken
    }


    #endregion

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_IsRolling && collision.gameObject.tag == "Player")
        {
            //check stun...
            Debug.Log("TRIGGER Stunned!");

            Debug.Log("Player " + PlayerNumber + " is stunning player " + collision.gameObject.GetComponent<Character_Controller>().PlayerNumber);

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

            

            collision.gameObject.GetComponent<Character_Controller>().m_StunLeft = m_StunDuration;
            collision.gameObject.GetComponent<Character_Controller>().m_Rigidbody2D.velocity = new Vector2(0f, 0f);
            collision.gameObject.GetComponent<Character_Controller>().m_Rigidbody2D.isKinematic = true;
        }

    }
}
