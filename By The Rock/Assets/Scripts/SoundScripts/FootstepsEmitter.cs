using UnityEngine;
using System.Collections;

public class FootstepsEmitter : BaseEmitter {

    Transform _t;

    protected override void Start()
    {
        _TriggerOnce = true;
        _t = this.transform;
        base.Start();
    }

    public void PlayStep(int stepType)
    {
        Debug.Log("FOOTSTEPSOUND" + stepType);
        _3dAttributes.position.x = _t.position.x;
        _3dAttributes.position.y = _t.position.y;
        _3dAttributes.position.z = _t.position.z;       
        _EventInstance.start();
        _EventInstance.set3DAttributes(_3dAttributes);
    }
}
