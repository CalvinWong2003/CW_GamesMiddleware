using UnityEngine;

public interface ICollidable
{
    bool collidingWith (ICollidable c);
    void Resolve(ICollidable c, ref Vector3 newPosition, ref Vector3 newVelocity);
}
