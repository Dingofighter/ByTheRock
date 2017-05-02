using UnityEngine;
using System.Collections;

public class BaseEmitter : MonoBehaviour {

    [FMODUnity.EventRef]
    public string _EventMusic;
    //public FMODUnity.ParamRef[] _Params = new FMODUnity.ParamRef[0];
    public FMODUnity.EmitterGameEvent _PlayEvent = FMODUnity.EmitterGameEvent.None;
    public FMODUnity.EmitterGameEvent _StopEvent = FMODUnity.EmitterGameEvent.None;

    public FMOD.Studio.EventDescription _EventDescription = null;
    public FMOD.Studio.EventInstance _EventInstance = null;
    public FMOD.ATTRIBUTES_3D _3dAttributes;
    public FMOD.Studio.PLAYBACK_STATE _playbackState;


    public bool _HasTriggered = false;
    public bool _AllowFadeout = true;
    public bool _isQuitting = false;
    public bool _TriggerOnce = false;
    public bool preload = false;

    public FMODUnity.ParamRef[] Params = new FMODUnity.ParamRef[0];

    public bool _OverrideAttenuation = false;
    public float _OverrideMinDistance = -1.0f;
    public float _OverrideMaxDistance = -1.0f;

    // Use this for initialization
    protected virtual void Start ()
    {
        FMODUnity.RuntimeUtils.EnforceLibraryOrder();
        if (preload)
        {
            GetEvent();
            _EventDescription.loadSampleData();
            FMODUnity.RuntimeManager.StudioSystem.update();
            FMOD.Studio.LOADING_STATE loadingState;
            _EventDescription.getSampleLoadingState(out loadingState);
            while (loadingState == FMOD.Studio.LOADING_STATE.LOADING)
            {
                System.Threading.Thread.Sleep(1);
                _EventDescription.getSampleLoadingState(out loadingState);
            }
        }
            
        HandleGameEvent(FMODUnity.EmitterGameEvent.ObjectStart);
    }


    public void Play()
    {
        if (_HasTriggered && _TriggerOnce)
            return;

        if (string.IsNullOrEmpty(_EventMusic))
            return;

        if (_EventDescription == null)
            GetEvent();

        bool isOneshot = false;
        if (!_EventMusic.StartsWith("snapshot", System.StringComparison.CurrentCultureIgnoreCase))
        {
            _EventDescription.isOneshot(out isOneshot);
        }

        bool is3D;
        _EventDescription.is3D(out is3D);

        if (_EventInstance != null && !_EventInstance.isValid())
        {
            _EventInstance = null;
        }

        // Let previous oneshot instances play out
        if (isOneshot && _EventInstance != null)
        {
            _EventInstance.release();
            _EventInstance = null;
        }


        if (_EventInstance == null)
        {
            _EventDescription.createInstance(out _EventInstance);

            if (is3D)
            {
                var rigidBody = GetComponent<Rigidbody>();
                var rigidBody2D = GetComponent<Rigidbody2D>();
                var transform = GetComponent<Transform>();
                if (rigidBody)
                {
                    _EventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject, rigidBody));
                    FMODUnity.RuntimeManager.AttachInstanceToGameObject(_EventInstance, transform, rigidBody);
                }
                else
                {
                    _EventInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject, rigidBody2D));
                    FMODUnity.RuntimeManager.AttachInstanceToGameObject(_EventInstance, transform, rigidBody2D);
                }
            }
        }

        foreach (var param in Params)
        {
            _EventInstance.setParameterValue(param.Name, param.Value);
        }

        if (is3D && _OverrideAttenuation)
        {
            _EventInstance.setProperty(FMOD.Studio.EVENT_PROPERTY.MINIMUM_DISTANCE, _OverrideMinDistance);
            _EventInstance.setProperty(FMOD.Studio.EVENT_PROPERTY.MAXIMUM_DISTANCE, _OverrideMaxDistance);
        }
        

        /*_EventCallback = new FMOD.Studio.EVENT_CALLBACK(StudioEventCallback);
_EventInstance.setCallback(_EventCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT);*/
        _EventInstance.start();

        //add 3d attributes here when converting to a more base script

        _HasTriggered = true;
    }


    void GetEvent()
    {
        _EventDescription = FMODUnity.RuntimeManager.GetEventDescription(_EventMusic);
    }

    void OnDestroy()
    {
        if (!_isQuitting)
        {
            HandleGameEvent(FMODUnity.EmitterGameEvent.ObjectDestroy);
            if (_EventInstance != null && _EventInstance.isValid())
                FMODUnity.RuntimeManager.DetachInstanceFromGameObject(_EventInstance);
        }

        if (preload)
        {
            _EventDescription.unloadSampleData();
        }
    }

    public void Stop()
    {
        if (_EventInstance != null)
        {
            _EventInstance.stop(_AllowFadeout ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT : FMOD.Studio.STOP_MODE.IMMEDIATE);
            _EventInstance.release();
            _EventInstance = null;
        }
    }

    void HandleGameEvent(FMODUnity.EmitterGameEvent _GameEvent)
    {
        if (_PlayEvent == _GameEvent)
            Play();
        if (_StopEvent == _GameEvent)
            Stop();
    }


    void OnApplicationQuit()
    {
        _isQuitting = true;
    }

    public void SetParameter(string name, float value)
    {
        if (_EventInstance != null)
        {
            _EventInstance.setParameterValue(name, value);
        }
    }

    public bool IsPlaying()
    {
        if (_EventInstance != null && _EventInstance.isValid())
        {
            FMOD.Studio.PLAYBACK_STATE playbackState;
            _EventInstance.getPlaybackState(out playbackState);
            return (playbackState != FMOD.Studio.PLAYBACK_STATE.STOPPED);
        }
        return false;
    }
}
