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
        if (c is Bounce ball)
        {
            float distanceToPlane = Vector3.Dot(ball.transform.position - transform.position, Normal);
            return Mathf.Abs(distanceToPlane) <= ball.Radius;
        }
        return false;
    }

    public void Resolve(ICollidable c, ref Vector3 newPosition, ref Vector3 newVelocity)
    {
        if (c is Bounce)
        {
            c.collidingWith(this);
        }
        else
        {
            
        }
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
