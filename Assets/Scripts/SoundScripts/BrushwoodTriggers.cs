using UnityEngine;
using System.Collections;

public class BrushwoodTriggers : MonoBehaviour
{
    public float par1, par2;
    private string _paramBincrement;
    private string _paramBool;
    public MusicEmitter _MusicEmitter;
    public BrushwoodManager _BrushwoodManager;
    public Transform _player;
    public BoxCollider _box;
    public float _passage = 0, _passageReverse = 0;
    public int _triggerID = 0;
    public bool _special = false;
    public bool _glade = false;
    public float _dist = 0, _distZ = 0;
    public int _bool;
    public GameObject trigger;

    private float checkx = 0, checkz = 0, _cX = 0, _cZ = 0;
    private Vector3 _pos;

    private bool once = false;
    private bool turnoff = false;


    public void Start()
    {
        _MusicEmitter = FindObjectOfType<MusicEmitter>().GetComponent<MusicEmitter>();
        _BrushwoodManager = GetComponentInParent<BrushwoodManager>();
        _player = FindObjectOfType<PlayerMovement>().GetComponent<Transform>();
        _box = GetComponent<BoxCollider>();
        _paramBincrement = "SmallPassage";
        _paramBool = "Bool";
        trigger = gameObject;

    }

    public void Update()
    {
        _pos = _box.transform.position - _player.transform.position;
        _pos = Quaternion.Euler(0, _box.transform.rotation.eulerAngles.y, 0) * _pos;

        checkx = Mathf.Max(0, (_pos.x < 0 ? -_pos.x : _pos.x) - _box.size.z / 2);
        checkz = Mathf.Max(0, (_pos.z < 0 ? -_pos.z : _pos.z) - _box.size.x / 2);
        _dist = Mathf.Sqrt(checkx * checkx + checkz * checkz);

        if (_special && !turnoff)
        { 
            if (!_glade)
            {
                if (_dist <= 5)
                    _MusicEmitter._EventInstance.setParameterValue("Distance", 10);
                else
                    _MusicEmitter._EventInstance.setParameterValue("Distance", 0);
            }
            else
            {
                _cX = Mathf.Max(0, (_pos.x < 0 ? -_pos.x : _pos.x) - _box.size.x / 2);
                _cZ = Mathf.Max(0, (_pos.z < 0 ? -_pos.z : _pos.z) - _box.size.z / 2);
                _distZ = Mathf.Sqrt(_cX * _cX + _cZ * _cZ);

                if (_distZ <= 3)
                    _MusicEmitter._EventInstance.setParameterValue("Distance", 10);
                else
                    _MusicEmitter._EventInstance.setParameterValue("Distance", 0);
            }
        }

        _MusicEmitter._EventInstance.getParameterValue("Bool", out par1, out par1);


    }



    private void OnTriggerExit(Collider _col)
    {

        if (_col.gameObject.tag == "Player")
        {
            Vector3 pos = _box.transform.position - _player.transform.position;
            pos = Quaternion.Euler(0, -_box.transform.rotation.eulerAngles.y, 0) * pos;

            

            if (pos.x < 0)
            {
                if (_special && !_glade)
                {  
                    turnoff = true;
                }
                else if (_special && _glade)
                {
                    turnoff = false;
                }
                    

                if (_BrushwoodManager.getBool(_triggerID))
                {
                    _bool = 0;
                    _MusicEmitter._EventInstance.setParameterValue(_paramBool, _bool);
                    _MusicEmitter._EventInstance.setParameterValue(_paramBincrement, _passage);
                    _BrushwoodManager.setBool(_triggerID);
                }
            }
            else
            {
                if (_special && !_glade)
                {      
                    turnoff = false;
                }                  
                else if (_special && _glade)
                    turnoff = true;

                if (!_BrushwoodManager.getBool(_triggerID))
                {
                    _bool = 1;
                    _MusicEmitter._EventInstance.setParameterValue(_paramBool, _bool);
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
