using UnityEngine;
using System.Collections;

public class AreaTrigger : MonoBehaviour {

    public MusicEmitter _ME;
    public Transform _player;
    private BoxCollider _box;
    public bool glade = false;

    // Use this for initialization
    void Start ()
    {
        _player = FindObjectOfType<PlayerController>().GetComponent<Transform>();
        _box = gameObject.GetComponent<BoxCollider>();
        _ME = FindObjectOfType<MusicEmitter>().GetComponent<MusicEmitter>();
    }

    private void OnTriggerExit(Collider _col)
    {
        if (_col.gameObject.tag == "Player")
        {
            Vector3 pos = _box.transform.position - _player.transform.position;
            pos = Quaternion.Euler(0, -_box.transform.rotation.eulerAngles.y, 0) * pos;

                if (pos.z < 0)
                {
                    _ME._EventInstance.setParameterValue("GlHi", 2);
                }
                else
                {
                    _ME._EventInstance.setParameterValue("GlHi", 0);
                }
            Debug.Log(pos.z);
        }
    }
}
