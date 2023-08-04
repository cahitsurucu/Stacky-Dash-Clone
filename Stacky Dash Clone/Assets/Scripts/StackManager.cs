using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    GameObject player;
    [SerializeField] private List<GameObject> stack;
    public GameObject lastObj;
    [SerializeField] private GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        manager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        stack.Add(lastObj);
    }

    public void addStack(GameObject other)
    {
        stack.Add(other.gameObject);
        other.gameObject.tag = "BridgeTile";
        other.gameObject.transform.SetParent(player.transform);
        other.gameObject.transform.position = lastObj.transform.position + new Vector3(0, 0.052f, 0);
        lastObj = other.gameObject;
        player.GetComponent<PlayerControl>().changePos(new Vector3(0, 0.41f, 0));
        manager.increaseScore();
    }

    public void removeStack(GameObject other)
    {
        manager.decreaseScore();
        stack.Remove(other);
        if(stack.Count != 0)
            lastObj = stack[stack.Count - 1];
    }

    public GameObject getLastObj()
    {
        return stack[stack.Count - 1];
    }

    public int getStackCount()
    {
        return stack.Count;
    }

    public void stopCoroutine()
    {
        StopAllCoroutines();
    }

    public void destroyTile()
    {
        StartCoroutine(destroy());
    }

    private IEnumerator destroy()
    {
        GameObject temp = getLastObj();
        stack.Remove(temp);
        Destroy(temp);
        player.GetComponent<PlayerControl>().changePos(new Vector3(0, -0.41f, 0));
        yield return new WaitForSeconds(0.05f);
        if (stack.Count != 0)
            destroyTile();
        else
            player.GetComponent<PlayerControl>().setFinish();
    }
}
