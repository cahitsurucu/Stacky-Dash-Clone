using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeMovement : MonoBehaviour
{

    [SerializeField] private Direction direction;
    [SerializeField] private List<GameObject> paths;
    public enum Direction { Up, Down};

    public void changeDirection()
    {
        if (direction == Direction.Up)
            direction = Direction.Down;
        else
            direction = Direction.Up;
    }

    public GameObject getPath()
    {
        if(direction == Direction.Up)
        {
            return paths[0];
        }
        return paths[1];
    }
}
