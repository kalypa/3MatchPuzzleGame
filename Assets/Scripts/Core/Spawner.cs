using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Shape[] shapes;

    public Shape SpawnShape()
    {
        Shape shape = null;
        shape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;
        if (shape)
        {
            return shape;
        }
        else
        {
            Debug.LogWarning("shape is null");
            return null;
        }
    }

    Shape GetRandomShape()
    {
        int randVal = Random.Range(0, shapes.Length);
        if (shapes[randVal])
        {
            return shapes[randVal];
        }
        else
        {
            Debug.LogWarning("shapes[randVal] is null");
            return null;
        }
    }
}
