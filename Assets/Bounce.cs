using UnityEngine;

public class Bounce : MonoBehaviour
{
    public PlanePhysics thePlane;
    public float Radius 
    {
        get { return transform.localScale.x/2f; }
        set { transform.localScale = Vector3.one * value * 2; }
    }

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
        if(parallel_Distance(transform.position - thePlane.transform.position, thePlane.Normal) < Radius)
        {
            //transform.position += velocity * Time.deltaTime;
            //velocity = CoR * velocity;
            Vector3 parallelComp = parallel_Comp(velocity, thePlane.Normal);
            Vector3 perpendicularComp = perpendicular_Comp(velocity, thePlane.Normal);
            velocity = perpendicularComp - (CoR * parallelComp);
            transform.position += velocity * Time.deltaTime;
            transform.position -= perpendicularComp * Time.deltaTime;
        }
    }

    /// <summary>
    /// Returns the magnitude of the parallel component of vector v parallel to vector n
    /// </summary>
    /// <param name = "v"> Vector to be decomposed </param>
    /// <param name = "n"> Unit vector parallel to above component </param>
    public float parallel_Distance(Vector3 v, Vector3 n)
    {
        return Vector3.Dot(v, n.normalized);
    }

    public Vector3 parallel_Comp(Vector3 v, Vector3 n)
    {
        return Vector3.Dot(v, n.normalized) * n.normalized;
    }
    public Vector3 perpendicular_Comp(Vector3 v, Vector3 n)
    {
        return v - parallel_Comp(v,n);
    }

}
