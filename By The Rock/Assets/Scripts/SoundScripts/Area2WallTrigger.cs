using UnityEngine;
using System.Collections;

public class Area2WallTrigger : MonoBehaviour {

    public GameObject wall;

    public void enableWall()
    {
        wall.SetActive(true);
    }
}
