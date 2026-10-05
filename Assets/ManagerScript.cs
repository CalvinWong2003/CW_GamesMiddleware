using UnityEngine;
using System.Linq;
using System.Collections.Generic;
public class ManagerScript : MonoBehaviour
{
    List<ICollidable> allObjects;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        // Finds all active MonoBehaviours and filters them down to your interface type
        allObjects = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
                                         .OfType<ICollidable>().ToList();

                                         print(allObjects.Count);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < allObjects.Count; i++)
        {
            for (int j = i + 1; j < allObjects.Count;)
            {
                if(allObjects[i].collidingWith(allObjects[j]))
                {
                    //Collision has occurred
                    //allObjects[i].Resolve(allObjects[j])
                }
            }
        }
    }
}
