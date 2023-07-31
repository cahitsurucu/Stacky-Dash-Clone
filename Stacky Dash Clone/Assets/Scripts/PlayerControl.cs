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
    [SerializeField] bool isMoveBridge = false;
    [SerializeField] bool canCollide = true;

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
            if (direction == Direction.Left)
            {
                pathCreator.GetComponent<Path>().setDirection(Path.Direction.Right);
            }
            else if (direction == Direction.Right)
            {
                pathCreator.GetComponent<Path>().setDirection(Path.Direction.Left);
            }
            else if (direction == Direction.Up)
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

    public void changePos(Vector3 offset)
    {
        Transform child = transform.GetChild(0);
        child.localPosition += offset;
    }

    private void changePath(GameObject other)
    {
        pathController.addPath(other.gameObject.GetComponent<BridgeMovement>().getPath());
        pathCreator = other.gameObject.GetComponent<BridgeMovement>().getPath().GetComponent<PathCreator>();
        other.gameObject.GetComponent<BridgeMovement>().changeDirection();
        currentMoveDirection = pathCreator.GetComponent<Path>().getMoveDirection();
        if(currentMoveDirection == -1)
        {
            distanceTravelled = pathCreator.GetComponent<Path>().getDistance();
        }
    }

    private void setPath(GameObject other)
    {
        Debug.Log(other.gameObject.tag);
        if(pathCreator != null)
        {
            if (direction == Direction.Left)
            {
                pathCreator.GetComponent<Path>().setDirection(Path.Direction.Right);
            }
            else if (direction == Direction.Right)
            {
                pathCreator.GetComponent<Path>().setDirection(Path.Direction.Left);
            }
            else if (direction == Direction.Up)
            {
                pathCreator.GetComponent<Path>().setDirection(Path.Direction.Down);
            }
            else
            {
                pathCreator.GetComponent<Path>().setDirection(Path.Direction.Up);
            }
            pathCreator.GetComponent<Path>().setMoveDirection();
            pathCreator.GetComponent<Path>().setDistance(distanceTravelled);
            pathCreator = null;
            distanceTravelled = 0;
        }
        if(other.gameObject.CompareTag("BridgeStart"))
        {
            isMoving = true;
            isMoveBridge = true;
        }
        else
        {
            isMoving = true;
            isMoveBridge = false;
        }
        changePath(other);
    }

    private IEnumerator resetCollide()
    {
        yield return new WaitForSeconds(1f);
        canCollide = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Path"))
        {
            pathController.addPath(other.gameObject);
        }
        else if (other.gameObject.CompareTag("BridgePath"))
        {
            pathController.addPath(other.gameObject.transform.parent.gameObject);// bridge path
        }
        else if (other.gameObject.CompareTag("Tile"))
        {
            stackManager.addStack(other.gameObject);
        }
        else if (other.gameObject.CompareTag("BridgeStart"))
        {
            if(!isMoveBridge)
                setPath(other.gameObject);
            isMoveBridge = true;
        }
        else if (other.gameObject.CompareTag("BridgeFinish"))
        {
            
            if(isMoveBridge)
                setPath(other.gameObject);
            isMoveBridge = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Path"))
        {
            pathController.removePath(other.gameObject);
        }
        else if(other.gameObject.CompareTag("BridgePath"))
        {
            pathController.removePath(other.gameObject.transform.parent.gameObject); // bridge path
        }
        else if(other.gameObject.CompareTag("BridgeStart") && canCollide)
        {
            canCollide = false;
            StartCoroutine(resetCollide());
            other.gameObject.tag = "BridgeFinish";
        }
        else if(other.gameObject.CompareTag("BridgeFinish") && canCollide)
        {
            canCollide = false;
            StartCoroutine(resetCollide());
            other.gameObject.tag = "BridgeStart";
        }
    }

    public bool getBridgeMove()
    {
        return isMoveBridge;
    }
}
