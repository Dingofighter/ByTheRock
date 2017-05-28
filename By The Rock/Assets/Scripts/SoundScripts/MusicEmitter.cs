using UnityEngine;
using System.Collections;

public class MusicEmitter : BaseEmitter
{


    private string _paramBincrement;
    private string _paramBool;
    FMOD.Studio.EventInstance _HaniaPTSD;

    protected override void Start()
    {
        base.Start();
        _paramBincrement = "SmallPassage";
        _paramBool = "Bool";
        _EventInstance.setParameterValue(_paramBincrement, -1);
        _EventInstance.setParameterValue(_paramBool, 0);
    }

    public void Change(int song = 0, bool fade = true)
    {
        string guid;
        System.Guid thing;

        switch (song)
        {
            default:
            case 0:
                guid = "{62b97563-a446-4cd2-b0c4-70ed712ea8c8}";
                break;
            case 1:
                guid = "{ad94bdfe-8ace-4555-95e8-54f7c350c7ec}";
                break;
        }

        FMOD.Studio.Util.ParseID(guid, out thing);

        if (_HaniaPTSD != null)
        {

            _HaniaPTSD.stop(fade ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT : FMOD.Studio.STOP_MODE.IMMEDIATE);
            _HaniaPTSD.release();
            _HaniaPTSD = null;
        }

        if (_EventInstance != null)
        {

            _EventInstance.stop(fade ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT : FMOD.Studio.STOP_MODE.IMMEDIATE);
            _EventInstance.release();
            _EventInstance = null;
        }

        if (GetComponent<GameManager>()._fmodSS.getEventByID(thing, out _EventDescription) != FMOD.RESULT.OK)
            Debug.Log("Didn't find Event");

        if (_EventDescription.createInstance(out _HaniaPTSD) != FMOD.RESULT.OK)
            Debug.Log("Event not created");

        _HaniaPTSD.start();

    }

    public void play()
    {
        string guid;
        System.Guid thing;

        if (_EventInstance != null)
        {

            _EventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            _EventInstance.release();
            _EventInstance = null;
        }

        guid = "{62b97563-a446-4cd2-b0c4-70ed712ea8c8}";

        FMOD.Studio.Util.ParseID(guid, out thing);


        if (GetComponent<GameManager>()._fmodSS.getEventByID(thing, out _EventDescription) != FMOD.RESULT.OK)
            Debug.Log("Didn't find Event");

        if (_EventDescription.createInstance(out _EventInstance) != FMOD.RESULT.OK)
            Debug.Log("Event not created");

        _EventInstance.setParameterValue("RightAfter Bear", 1);
        _EventInstance.start();
    }

    public void StartFadeOut()
    {
        Stop();

        string guid;
        System.Guid thing;

        guid = "{62b97563-a446-4cd2-b0c4-70ed712ea8c8}";

        FMOD.Studio.Util.ParseID(guid, out thing);

        if (GetComponent<GameManager>()._fmodSS.getEventByID(thing, out _EventDescription) != FMOD.RESULT.OK)
            Debug.Log("Didn't find Event");

        if (_EventDescription.createInstance(out _EventInstance) != FMOD.RESULT.OK)
            Debug.Log("Event not created");

       
    }

    public void startMusic()
    {
        Start();
    }

}