using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private StackManager stackManager;
    [SerializeField] private bool isFinished = false;
    [SerializeField] private GameObject emtpy;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        stackManager = player.GetComponent<StackManager>();
        emtpy = GameObject.Find("Empty");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("BridgeTile") && player.GetComponent<PlayerControl>().getBridgeMove())
        {
            if (stackManager.getStackCount() != 0 && !isFinished)
                removeStack(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("BridgeTile"))
        {
            isFinished = true;
        }
    }

    private void removeStack(GameObject other)
    {
        StartCoroutine(waiter(other));
    }

    private IEnumerator waiter(GameObject other)
    {
        GameObject lastObj = stackManager.getLastObj();
        lastObj.transform.position = other.transform.position;
        lastObj.transform.SetParent(emtpy.transform);
        player.GetComponent<PlayerControl>().changePos(new Vector3(0, -0.41f, 0));
        stackManager.removeStack(lastObj);
        yield return new WaitForSeconds(0.1f);
        if (!isFinished)
            removeStack(other);
        yield return new WaitForSeconds(0.1f);
    }
}
