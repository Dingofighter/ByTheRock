using UnityEngine;
using System.Collections;

public class MusicEmitter : BaseEmitter {


    private string _paramBincrement;
    private string _paramBool;

    protected override void Start ()
    {

        base.Start();
        _paramBincrement = "SmallPassage";
        _paramBool = "Bool";
        Debug.Log(_EventInstance);
        _EventInstance.setParameterValue(_paramBincrement, -1);
        _EventInstance.setParameterValue(_paramBool, 0);
    }
}