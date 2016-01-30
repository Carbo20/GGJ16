using UnityEngine;
using System.Collections;

public class ChickenBehaviour : MonoBehaviour
{
    public float speed = 5f;
    public float countdown = 0.7f;
    public Vector2 direction;
    // Use this for initialization
    void Start()
    {
        while (direction.x == 0 && direction.y == 0)
            direction.Set(Random.Range(-10, 10), Random.Range(-10, 10));
        direction.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0.0f)
        {
            do direction.Set(Random.Range(-10, 10), Random.Range(-10, 10));
            while (direction.x == 0 && direction.y == 0);
            direction.Normalize();
            countdown = 0.7f;
        }
        transform.Translate(direction * Time.deltaTime * speed);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("hitsomething: ");
        direction.x *= -1;
        direction.y *= -1;
        //direction.Normalize();
    }
}