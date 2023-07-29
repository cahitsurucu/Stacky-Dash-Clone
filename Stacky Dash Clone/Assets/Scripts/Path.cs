using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class Path : MonoBehaviour
{
    [SerializeField] private Direction direction;

    [SerializeField] private int moveDirection = 1;

    [SerializeField] private float distance = 0;

    public enum Direction { Left, Right, Up, Down};

    public void setDirection(Direction temp)
    {
        direction = temp;
    }

    public Direction getDirection()
    {
        return direction;
    }

    public int getMoveDirection()
    {
        return moveDirection;
    }

    public void setMoveDirection()
    {
        moveDirection *= -1;
    }

    public void setDistance(float temp)
    {
        distance = temp;
    }

    public float getDistance()
    {
        return distance;
    }
}
