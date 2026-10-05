using UnityEngine;

public interface ICollidable
{
    bool collidingWith (ICollidable c);
    //void Resolve (ICollidable c, ref newPosition, ref newVelocity);
}
