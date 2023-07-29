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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tile"))
        {

        }
    }

    public void addStack(GameObject other)
    {
        stack.Add(other.gameObject);
        other.gameObject.tag = "Untagged";
        other.gameObject.transform.SetParent(player.transform);
        other.gameObject.transform.position = lastObj.transform.position + new Vector3(0, 0.042f, 0);
        lastObj = other.gameObject;
        player.GetComponent<PlayerControl>().changePos();
    }

    public void removeStack(GameObject other)
    {

    }
}
