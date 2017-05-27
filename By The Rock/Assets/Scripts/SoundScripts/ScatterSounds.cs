using UnityEngine;
using System.Collections;

public class ScatterSounds : BaseEmitter
{
    public ScatterManager _SM;
    public float _minDist = 0.1f, _maxDist = 0.3f;
    float relativeX, relativeY;
    Vector3 _Player;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        _SM = GetComponentInParent<ScatterManager>();
        ScatterPosition();
        _Player = _SM.PlayerPosition();
    }

    private void Update()
    {
        _Player = _SM.PlayerPosition();
        transform.position = new Vector3(_Player.x + relativeX, _Player.y, _Player.z + relativeY);
        _EventInstance.getPlaybackState(out _playbackState);

        _3dAttributes.position.x = _Player.x;
        _3dAttributes.position.y = _Player.y;
        _3dAttributes.position.z = _Player.z;

        if (_playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED)
        {
            _EventInstance.release();
            Destroy(gameObject);
        }
    }

    void ScatterPosition()
    {
        _Player = _SM.PlayerPosition();
    }
}
