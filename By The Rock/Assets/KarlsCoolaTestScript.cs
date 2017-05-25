using UnityEngine;
using System.Collections;

public class KarlsCoolaTestScript : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("KarleBest!");
            Screen.SetResolution(1920, 1080, true);
        }
    }
}
