using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

    public GameManager GM;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ScanForKeyStroke();
    }

    void ScanForKeyStroke()
    {
        if (Input.GetKeyDown("escape")) GM.TogglePauseMenu();
    }
}
