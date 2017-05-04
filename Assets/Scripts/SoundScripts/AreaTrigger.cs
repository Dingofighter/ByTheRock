using UnityEngine;
using System.Collections;

public class AreaTrigger : MonoBehaviour {

    public BigBirdEmitter _BBE;
    public BigBirdEmitter.area areasP, areasN;
    public MusicEmitter _ME;
    public Transform _player;
    private BoxCollider _box;
    public bool glade = false;

    // Use this for initialization
    void Start ()
    {
        _BBE = FindObjectOfType<BigBirdEmitter>().GetComponent<BigBirdEmitter>();
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

            if (!glade)
            {
                if (pos.x < 0)
                {
                    _BBE.areas = areasN;
                    _BBE.ChangeArea();
                }
                else
                {
                    _BBE.areas = areasP;
                    _BBE.ChangeArea();
                }
            }
            else
            {
                if (pos.z < 0)
                {
                    _BBE.areas = areasN;
                    _BBE.ChangeArea();
                    _ME._EventInstance.setParameterValue("GlHi", 2);
                }
                else
                {
                    _BBE.areas = areasP;
                    _BBE.ChangeArea();
                    _ME._EventInstance.setParameterValue("GlHi", 0);
                }

            }
        }
    }
}
