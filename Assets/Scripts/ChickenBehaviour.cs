using UnityEngine;
using System.Collections;

public class ChickenBehaviour : MonoBehaviour
{
    public float speed = 5f;
    public float countdown = 0.7f;
    public Vector2 direction;
    public Vector2 henhouse;
    public chickenState state;
    public bool facingleft = true;
    public enum chickenState { inHenhouse, Captured, Returning };
    // Use this for initialization
    void Start()
    {
        //state = chickenState.inHenhouse;
        //henhouse.Set(transform.position.x, transform.position.y);
        henhouse.Set(0, 0);
        while (direction.x == 0 && direction.y == 0)
            direction.Set(Random.Range(-10, 10), Random.Range(-10, 10));
        direction.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == chickenState.Captured)
        {

        }
        else if (state == chickenState.Returning)
        {
            direction.Set(henhouse.x - transform.position.x, henhouse.y - transform.position.y);
            direction.Normalize();
            //direction.Set(transform.position.x - henhouse.x, transform.position.y - henhouse.y);
        }
        else
        {
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
            facingleft = direction.x < 0.0f;
        }
        else if (direction.x > 0.0f)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            facingleft = direction.x > 0.0f;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (state == chickenState.inHenhouse && other.gameObject.name == "Henhouse")
            state = chickenState.Returning;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (state == chickenState.Returning && other.gameObject.name == "Henhouse")
            state = chickenState.inHenhouse;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("hitsomething: ");
        direction.x *= -1;
        direction.y *= -1;
        //direction.Normalize();
    }
}