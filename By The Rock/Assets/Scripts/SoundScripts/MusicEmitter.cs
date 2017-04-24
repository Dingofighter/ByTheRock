using UnityEngine;
using System.Collections;

public class MusicEmitter : MonoBehaviour {

    [FMODUnity.EventRef]
    public string _EventMusic;
    //public FMODUnity.ParamRef[] _Params = new FMODUnity.ParamRef[0];
    public FMODUnity.EmitterGameEvent _PlayEvent = FMODUnity.EmitterGameEvent.None;
    public FMODUnity.EmitterGameEvent _StopEvent = FMODUnity.EmitterGameEvent.None;

    public FMOD.Studio.EventDescription _EventDescription = null;
    public FMOD.Studio.EventInstance _EventInstance = null;
    public FMOD.Studio.EVENT_CALLBACK _EventCallback = null;
    public FMOD.Studio.TIMELINE_BEAT_PROPERTIES _EventData;

    public bool _HasTriggered = false;
    public bool _AllowFadeout = true;
    public bool _isQuitting = false;
    public bool _TriggerOnce = false;
    private string _paramBincrement;
    private string _paramBool;

    void Start ()
    {
        //HandleGameEvent(FMODUnity.EmitterGameEvent.ObjectStart);
        Play();
        _paramBincrement = "SmallPassage";
        _paramBool = "Bool";
        _EventInstance.setParameterValue(_paramBincrement, -1);
        _EventInstance.setParameterValue(_paramBool, 0);
    }
	
    public FMOD.RESULT StudioEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, System.IntPtr eventInstance, System.IntPtr parameters)
    {
        /*if (type == FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER)
        {

        }*/
        if (type == FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT)
        {
            FMOD.Studio.TIMELINE_BEAT_PROPERTIES _beat = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)System.Runtime.InteropServices.Marshal.PtrToStructure(parameters, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
            _EventData = _beat;
        }
        return FMOD.RESULT.OK;
    }

    void OnDestroy()
    {
        if (!_isQuitting)
        {
            HandleGameEvent(FMODUnity.EmitterGameEvent.ObjectDestroy);
            if (_EventInstance != null && _EventInstance.isValid())
                FMODUnity.RuntimeManager.DetachInstanceFromGameObject(_EventInstance);
        }
    }

    private void Update()
    {

    }


    void GetEvent()
    {
        _EventDescription = FMODUnity.RuntimeManager.GetEventDescription(_EventMusic);
    }


    public void Play()
    {

        if (_HasTriggered && _TriggerOnce)
            return;

        if (string.IsNullOrEmpty(_EventMusic))
            return;

        if (_EventDescription == null)
            GetEvent();

        if (_EventInstance == null)
            _EventDescription.createInstance(out _EventInstance);

        _EventCallback = new FMOD.Studio.EVENT_CALLBACK(StudioEventCallback);
        _EventInstance.setCallback(_EventCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT);
        _EventInstance.start();

        //add 3d attributes here when converting to a more base script

        _HasTriggered = true;

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
}