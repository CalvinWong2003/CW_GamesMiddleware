using UnityEngine;

public class Bounce : MonoBehaviour
{
    Vector3 velocity = Vector3.zero;
    Vector3 acceleration = Vector3.zero;

    float CoR = 0.75f; //Coefficient of Restitution

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        print(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        acceleration = new Vector3 (0, -9.8f, 0);

        // v = u + a * t
        //velocity = velocity + acceleration * Time.deltaTime;
        velocity += acceleration * Time.deltaTime;

        // s = u * t
        transform.position += velocity * Time.deltaTime;

        //detect collision
        if(transform.position.y < 0.5f)
        {
            velocity = -CoR * velocity;
        }
    }
}
