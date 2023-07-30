using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour
{
    GameObject player;
    [SerializeField] private List<GameObject> stack;
    public GameObject lastObj;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
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
    }

    /*public void removeStack(GameObject other)
    {
        if(getStackCount() != 0)
            StartCoroutine(waiter(other));
    }

    private IEnumerator waiter(GameObject other)
    {
        lastObj = stack[stack.Count - 1];
        lastObj.transform.position = other.transform.position;
        lastObj.transform.SetParent(null);
        player.GetComponent<PlayerControl>().changePos(new Vector3(0, -0.41f, 0));
        stack.Remove(lastObj);
        yield return new WaitForSeconds(0.1f);
        if(player.GetComponent<PlayerControl>().getBridgeMove())
            removeStack(other);
    }*/

    public void removeStack(GameObject other)
    {
        stack.Remove(other);
    }

    public GameObject getLastObj()
    {
        return stack[stack.Count - 1];
    }

    public int getStackCount()
    {
        return stack.Count;
    }
}
