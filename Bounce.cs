using UnityEngine;

public class Bounce : MonoBehaviour, ICollidable
{
    public float Radius 
    {
        get { return transform.localScale.x/2f; }
        set { transform.localScale = Vector3.one * value * 2; }
    }
    //Mass of two spheres
    float mA = 10f;
    float mB = 5f;

    internal Vector3 velocity = Vector3.zero;
    internal Vector3 acceleration = Vector3.zero;
    internal Vector3 oldVelocity = Vector3.zero;
    internal Vector3 oldPosition = Vector3.zero;
    internal float d0 = 0;
    internal float CoR = 0.75f; //Coefficient of Restitution

    //Vector3 p1 = transform.position;
    //Vector3 p2 = Sphere2.transform.position;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        print(transform.position);
    }
    public bool collidingWith(ICollidable c)
    {
        if(c is PlanePhysics)
        {
            return c.collidingWith(this);
        }
        //Must be sphere on sphere
        Bounce otherSphere = c as Bounce;
        return (Vector3.Distance(transform.position, otherSphere.transform.position) - Radius - otherSphere.Radius) < 0;
    }
    // Update is called once per frame
    void Update()
    {
        acceleration = new Vector3 (0, -9.8f, 0);

        oldVelocity = velocity;
        oldPosition = transform.position;

        // v = u + a * t
        //velocity = velocity + acceleration * Time.deltaTime;
        velocity += acceleration * Time.deltaTime;

        // s = u * t
        transform.position += velocity * Time.deltaTime;

        //detect collision
        
        //if (d1 < 0)
        //{
        //    //transform.position += velocity * Time.deltaTime;
        //    //velocity = CoR * velocity;
        //    //Vector3 parallelComp = parallel_Comp(velocity, thePlane.Normal);
        //    //Vector3 perpendicularComp = perpendicular_Comp(velocity, thePlane.Normal);
        //    //velocity = perpendicularComp - (CoR * parallelComp);
        //    //transform.position += velocity * Time.deltaTime;
        //    //transform.position -= perpendicularComp * Time.deltaTime;


        //    // transform.position -= perpendicularVelocity * Time.deltaTime;
        //}
        //d0 = d1;
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

    public void Resolve(ICollidable c, ref Vector3 newPosition, ref Vector3 newVelocity)
    {
        if(c is Bounce)
        {
            return;
        }
    }
}
