using UnityEngine;
using System.Collections;

public class EnableStuff : MonoBehaviour {

    public GameObject stuffToEnable;
    public GameObject stuffToDisable;

    public GameObject riverhunt;
    public GameObject riverglade;

    public float _dist = 0, _distZ = 0;
    private float checkx = 0, checkz = 0, _cX = 0, _cZ = 0;
    private Vector3 _pos;
    public Transform _player;
    public BoxCollider _box;

    void Start()
    {
        _player = FindObjectOfType<PlayerController>().GetComponent<Transform>();
        _box = GetComponent<BoxCollider>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        _pos = _box.transform.position - _player.transform.position;
        _pos = Quaternion.Euler(0, _box.transform.rotation.eulerAngles.y, 0) * _pos;

        checkx = Mathf.Max(0, (_pos.x < 0 ? -_pos.x : _pos.x) - _box.size.z / 2);
        checkz = Mathf.Max(0, (_pos.z < 0 ? -_pos.z : _pos.z) - _box.size.x / 2);
        _dist = Mathf.Sqrt(checkx * checkx + checkz * checkz);
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player" && _pos.x > 0)
        {
            stuffToEnable.SetActive(true);
            riverglade.SetActive(true);
        }
        else if (c.tag == "Player")
        {
            //Other håll
        }

        /*if (c.tag == "Player")
        {
            stuffToEnable.SetActive(true);
        }*/
    }

    void OnTriggerExit(Collider c)
    {
        if (c.tag == "Player" && _pos.x < 0)
        {
            stuffToDisable.SetActive(false);
            riverhunt.SetActive(false);
        }
        else if (c.tag == "Player")
        {
            //Other håll
        }
    }
}
