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
public class CharacterController : MonoBehaviour
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
    private List<int> m_SpeedModKeys;
    private Dictionary<int, m_SpeedModificators> m_SpeedMod; // int -> id of the speed modif (spider web, buff...)

    public struct m_SpeedModificators {
        public float value;
        public float duration;

        public m_SpeedModificators(float _value, float _duration)
            {
                value = _value;
                duration = _duration;
            }
    }

    #region ROLL ATTACK
    private bool m_Roll;
    ////[SerializeField] private float m_rollDistance;
    [SerializeField] private float m_RollCD = 2f;
    [SerializeField] private float m_rollSpeed = 1f;
    [SerializeField] private float m_rollDuration = 1f;
    private float m_XDirection = 0f;
    private float m_YDirection = 0f;
    ////[SerializeField] private float m_RollForce;
    
    private bool m_IsRolling;
    private float timeRolling;
    
    private float m_rollCDLeft;
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
    private bool m_HaveChicken = false;
    [SerializeField] private float m_SpeedModWithChicken = 0.5f;
    #endregion

    #region TEST ASSET 2D
    private Animator m_Anim;                         // Reference to the player's animator component.
    private Rigidbody2D m_Rigidbody2D;                  
    private bool m_FacingRight = true;                  // For determining which way the player is currently facing.

    [SerializeField] private bool m_isJoystickConnected = false;
    #endregion

    #region Monobehaviour

    private void Awake()
    {
        // Setting up references.
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

	void Start () {
        m_IsRolling = false;
        m_rollCDLeft = 0;
        myTransform = this.transform;

        m_SpeedModKeys = new List<int>();
        m_SpeedMod = new Dictionary<int, m_SpeedModificators>();
    }
	
	// Update is called once per frame
	void Update () {
               
    }

    private void FixedUpdate()
    {
        // Read the inputs. 
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Pass all parameters to the character control script.
        //Debug.Log("MOOOOOOOOOOOVE  " );
        CheckRoll(h, v);
        Move(h, v, m_Roll);
        m_Roll = false;

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
 
            t_moveSpeedX = Mathf.Min(Mathf.Max(m_MinSpeed, moveX * m_MaxSpeed * Time.deltaTime * 50 * SpeedMult), m_MaxSpeed);
            t_moveSpeedY = Mathf.Min(Mathf.Max(m_MinSpeed, moveY * m_MaxSpeed * Time.deltaTime * 50 * SpeedMult), m_MaxSpeed);

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
            if ( (x!=0 || y!=0) && m_rollCDLeft <= 0 && (Input.GetButtonDown("Roll1")))
            {
                m_Roll = true;
                m_rollCDLeft = m_RollCD;
                Debug.Log("ROLLATTACK  X= " + x + "  Y= " + y);
                m_IsRolling = true;
                timeRolling = m_rollDuration;
                m_XDirection = x;
                m_YDirection = y;
            }
        }
        else
        {
            if (timeRolling > 0)
                timeRolling -= Time.deltaTime;
            else
            {
                m_IsRolling = false;
                Debug.Log("ROLLATTACK END");
                m_Rigidbody2D.velocity = new Vector2(0f, 0f);
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

}
