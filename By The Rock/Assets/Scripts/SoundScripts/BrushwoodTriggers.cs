using UnityEngine;
using System.Collections;

public class BrushwoodTriggers : MonoBehaviour
{

    private string _paramBincrement;
    private string _paramBool;
    public MusicEmitter _MusicEmitter;
    public BrushwoodManager _BrushwoodManager;
    public Transform _player;
    public BoxCollider _box;
    public int _passage = 0, _passageReverse = 0;
    public int _triggerID = 0;
    public bool _special = false;
    public float _dist = 0;

    private float checkx = 0, checkz = 0;
    private Vector3 _pos;


    public void Start()
    {
        _MusicEmitter = FindObjectOfType<MusicEmitter>().GetComponent<MusicEmitter>();
        _BrushwoodManager = GetComponentInParent<BrushwoodManager>();
        _player = FindObjectOfType<PlayerMovement>().GetComponent<Transform>();
        _box = GetComponent<BoxCollider>();
        _paramBincrement = "SmallPassage";
        _paramBool = "Bool";

    }

    public void Update()
    {
        _pos = _box.transform.position - _player.transform.position;
        _pos = Quaternion.Euler(0, _box.transform.rotation.eulerAngles.y, 0) * _pos;

        checkx = Mathf.Max(0, (_pos.x < 0 ? -_pos.x : _pos.x) - _box.size.z / 2);
        checkz = Mathf.Max(0, (_pos.z < 0 ? -_pos.z : _pos.z) - _box.size.x / 2);
        _dist = Mathf.Sqrt(checkx * checkx + checkz * checkz);

        if (_dist <= 5)
            _MusicEmitter._EventInstance.setParameterValue("Distance", 10);
        else
            _MusicEmitter._EventInstance.setParameterValue("Distance", 0);
    }



    private void OnTriggerExit(Collider _col)
    {

        if (_col.gameObject.tag == "Player")
        {
            Vector3 pos = _box.transform.position - _player.transform.position;
            pos = Quaternion.Euler(0, -_box.transform.rotation.eulerAngles.y, 0) * pos;

            

            if (pos.x < 0)
            {
                if (_BrushwoodManager.getBool(_triggerID))
                {

                    _MusicEmitter._EventInstance.setParameterValue(_paramBool, 0);
                    _MusicEmitter._EventInstance.setParameterValue(_paramBincrement, _passage);
                    _BrushwoodManager.setBool(_triggerID);

                }
            }
            else
            {
                if (!_BrushwoodManager.getBool(_triggerID))
                {
                    _MusicEmitter._EventInstance.setParameterValue(_paramBool, 1);
                    _MusicEmitter._EventInstance.setParameterValue(_paramBincrement, _passageReverse);
                    _BrushwoodManager.setBool(_triggerID, true);
                }
            }
        }
    }



    /*
     
     
     Char -> Enters Trigger + tobpmtrue = music change && biik == 0

    when musiken changes = tobpm = false

    Char -> Enters Trigger + tobpm = false = musik changes && bool == 1
     
     
     
     
     
     
     
     
     
     
     
     
     
     */


}
