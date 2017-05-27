using UnityEngine;
using System.Collections;

public class FootstepsEmitter : BaseEmitter {

    protected override void Start()
    {
        base.Start();
    }

    public void PlayStep(int stepnr)
    {
        var _t = this.transform.position;
        Play();
        _EventInstance.setParameterValue("Surface", 1.5f);
        
        _3dAttributes.position.x = _t.x;
        _3dAttributes.position.y = _t.y;
        _3dAttributes.position.z = _t.z;
        _EventInstance.set3DAttributes(_3dAttributes);
        _EventInstance.release();
        Debug.Log("Step nr: " + stepnr);
    }
}
