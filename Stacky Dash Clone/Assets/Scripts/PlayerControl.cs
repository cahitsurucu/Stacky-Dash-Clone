using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PlayerControl : MonoBehaviour
{

    Vector3 startPos, endPos;
    [SerializeField] private PathCreator pathCreator;
    public float distanceTravelled;
    public float speed;
    [SerializeField] private PathController pathController;
    [SerializeField] private Direction direction;
    [SerializeField] private int currentMoveDirection;
    [SerializeField] private StackManager stackManager;

    [SerializeField] bool isMoving = false;

    public enum Direction {Left, Right, Up, Down};

    private void Start()
    {
        pathController = this.gameObject.GetComponent<PathController>();
        stackManager = this.gameObject.GetComponent<StackManager>();
    }

    void Update()
    {
        if(!isMoving)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startPos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                endPos = startPos - Input.mousePosition;
                float x = endPos.x < 0f ? endPos.x * -1 : endPos.x;
                float y = endPos.y < 0f ? endPos.y * -1 : endPos.y;
                if (x < y)
                {
                    if (endPos.y > 0)
                    {
                        setDirection(Direction.Down);
                    }
                    else
                    {
                        setDirection(Direction.Up);
                    }
                }
                else
                {
                    if (endPos.x > 0)
                    {
                        setDirection(Direction.Left);
                    }
                    else
                    {
                        setDirection(Direction.Right);
                    }
                }
            }
        }

        if(isMoving)
        {
            Move();
        }
    }

    private void setDirection(Direction temp)
    {
        direction = temp;
        List<GameObject> paths = pathController.getPaths();
        for(int i = 0;i<paths.Count;i++)
        {
            if(direction == (Direction)paths[i].GetComponent<Path>().getDirection())
            {
                // set selection path and move the player
                pathCreator = paths[i].GetComponent<PathCreator>();
                currentMoveDirection = pathCreator.GetComponent<Path>().getMoveDirection();
                if(currentMoveDirection == -1)
                {
                    distanceTravelled = pathCreator.GetComponent<Path>().getDistance();
                }
                isMoving = true;
                break;
            }
        }
    }

    public Direction getDirection()
    {
        return direction;
    }

    private void Move()
    {
        distanceTravelled += speed * currentMoveDirection * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled,EndOfPathInstruction.Stop);

        if (transform.position == pathCreator.path.GetPoint(pathCreator.path.NumPoints - 1) || transform.position == pathCreator.path.GetPoint(0))
        {
            if(direction == Direction.Left)
            {
                pathCreator.GetComponent<Path>().setDirection(Path.Direction.Right);
            }
            else if(direction == Direction.Right)
            {
                pathCreator.GetComponent<Path>().setDirection(Path.Direction.Left);
            }
            else if(direction == Direction.Up)
            {
                pathCreator.GetComponent<Path>().setDirection(Path.Direction.Down);
            }
            else
            {
                pathCreator.GetComponent<Path>().setDirection(Path.Direction.Up);
            }
            pathCreator.GetComponent<Path>().setMoveDirection();
            pathCreator.GetComponent<Path>().setDistance(distanceTravelled);
            isMoving = false;
            pathCreator = null;
            distanceTravelled = 0;
        }
    }

    public void changePos()
    {
        Transform child = transform.GetChild(0);
        child.localPosition += new Vector3(0, 0.41f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Path"))
        {
            pathController.addPath(other.gameObject);
        }
        else if(other.gameObject.CompareTag("Tile"))
        {
            stackManager.addStack(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Path"))
        {
            pathController.removePath(other.gameObject);
        }
    }
}
