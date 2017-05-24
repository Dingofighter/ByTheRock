using UnityEngine;
using System.Collections;

public class ScatterSounds : BaseEmitter
{
    public ScatterManager _SM;
    public float _minDist = 5.8f, _maxDist = 40f;
    float relativeX, relativeY;
    Vector3 _Player;

    // Use this for initialization
    protected override void Start ()
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

        if (_playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED)
        {
            _EventInstance.release();
            Destroy(gameObject);
        }
    }

    void ScatterPosition()
    {
        _Player = _SM.PlayerPosition();
        float angle = Random.Range(0, Mathf.PI * 2);
        float distance = Random.Range(_minDist, _maxDist);
        relativeX = Mathf.Cos(angle)*distance;
        relativeY = Mathf.Sin(angle)*distance;

        transform.position = new Vector3(_Player.x + Mathf.Cos(angle) * distance, _Player.y, _Player.z + Mathf.Sin(angle) * distance);
    }
}
