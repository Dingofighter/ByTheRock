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


    public void Start()
    {
        _MusicEmitter = FindObjectOfType<MusicEmitter>().GetComponent<MusicEmitter>();
        _BrushwoodManager = GetComponentInParent<BrushwoodManager>();
        _player = FindObjectOfType<PlayerMovement>().GetComponent<Transform>();
        _box = GetComponent<BoxCollider>();
        _paramBincrement = "SmallPassage";
        _paramBool = "Bool";

    }



    private void OnTriggerExit(Collider _col)
    {

        if (_col.gameObject.tag == "Player")
        {
            Vector3 pos = _box.transform.position - _player.transform.position;
            pos = Quaternion.Euler(0, -_box.transform.rotation.eulerAngles.y, 0) * pos;

            Debug.Log(pos.x);

            if (pos.x < 0)
            {
                if (_BrushwoodManager.getBool(_triggerID))
                {
                    if (_special)
                    {
                        _MusicEmitter._EventInstance.setParameterValue("ODDEVEN", _MusicEmitter._EventData.bar % 2 - 1 == -1 ? 1 : _MusicEmitter._EventData.beat - 1);
                        _MusicEmitter._EventInstance.setParameterValue("Beat", _MusicEmitter._EventData.beat - 1 == 0 ? 4 : _MusicEmitter._EventData.beat - 1);
                    }

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
