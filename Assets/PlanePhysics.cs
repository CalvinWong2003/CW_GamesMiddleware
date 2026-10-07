using UnityEngine;

public class PlanePhysics : MonoBehaviour, ICollidable
{
    public Vector3 Normal 
    { 
        get { return transform.up; }
        set { transform.up = value; }
    }
   public bool collidingWith(ICollidable c)
    {
        if(c is PlanePhysics)
            return false;
        //Must be a Sphere
        Bounce sphere = c as Bounce;
        return parallel_Distance(sphere.transform.position - transform.position, Normal) - Radius - sphere.Radius < 0;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
