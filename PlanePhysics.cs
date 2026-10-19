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
        if(c is PlanePhysics)
        {
            return;
        }

        Bounce other = c as Bounce;
        float d0 = other.d0;
        Vector3 velocity = other.velocity;
        Vector3 acceleration = other.acceleration;
        Vector3 oldVelocity = other.oldVelocity;
        Vector3 oldPosition = other.oldPosition;
        float CoR = other.CoR;
        float d1 = other.parallel_Distance(transform.position - other.transform.position, Normal) - other.Radius;
        
        //Calculating Time of Impact (ToI)
        float totalTime = Time.deltaTime;
        float vdrop = (d1 - d0) / totalTime;
        float ToI = -d0 / vdrop;
        Vector3 VoI = oldVelocity + (acceleration * ToI);
        Vector3 PoI = oldPosition + (VoI * ToI);

        //Resolving collision (adjusting velocity for each bounce)
        Vector3 parallelVelocity = other.parallel_Comp(VoI, Normal);
        Vector3 perpendicularVelocity = other.perpendicular_Comp(VoI, Normal);
        Vector3 VoIout = perpendicularVelocity - (CoR * parallelVelocity);

        //Fast Forward to current frame
        float timeRemaining = totalTime - ToI;
        Vector3 finalvelocity = VoIout + (acceleration * timeRemaining);
        Vector3 currentPosition = PoI + (finalvelocity * timeRemaining);
        velocity = finalvelocity;
        other.transform.position += finalvelocity * Time.deltaTime;
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
