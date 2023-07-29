using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathController : MonoBehaviour
{
    [SerializeField] private List<GameObject> paths;

    public void addPath(GameObject other)
    {
        paths.Add(other);
    }

    public void removePath(GameObject other)
    {
        paths.Remove(other);
    }

    public List<GameObject> getPaths()
    {
        return paths;
    }

}
