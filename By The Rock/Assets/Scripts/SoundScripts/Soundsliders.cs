using UnityEngine;
using System.Collections;

public class Soundsliders : MonoBehaviour {
    //Linus smells

    System.Guid[] guid = new System.Guid[5];
    string[] guidID = new string[5] {"{336ac9cb-550d-413e-be8c-226db9a86162}", "{f5314b91-ab5f-4a46-afb4-880bb6f022fa}", "{180bbb4a-48c6-4c6a-8c07-60e5f7928c57}", "{cddbd9cf-bbcb-49d6-a507-c540d136f8ce}", "{cb00eace-e109-41be-b630-9340826b1c45}"};
    private GameManager gm;
    FMOD.Studio.Bus _Master;
    FMOD.Studio.Bus _Ambience;
    FMOD.Studio.Bus _Dialogues;
    FMOD.Studio.Bus _Music;
    FMOD.Studio.Bus _SFX;

    public GameObject _MSlider;
    public GameObject _ASlider;
    public GameObject _DSlider;
    public GameObject _MUSlider;
    public GameObject _SFXSlider;


    private void Start()
    {
        gm = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        for (int i = 0; i < 5; i++)
        {
            FMOD.Studio.Util.ParseID(guidID[i], out guid[i]);
        }
        gm._fmodSS.getBusByID(guid[0], out _Master);
        gm._fmodSS.getBusByID(guid[1], out _Ambience);
        gm._fmodSS.getBusByID(guid[2], out _Dialogues);
        gm._fmodSS.getBusByID(guid[3], out _Music);
        gm._fmodSS.getBusByID(guid[4], out _SFX);
    }

    private void Update()
    {



    }

    public void Master(float yup)
    {
        _Master.setVolume(yup);
    }

    public void Ambience(float yup)
    {
        _Ambience.setVolume(yup);
    }

    public void Dialogues(float yup)
    {
        _Dialogues.setVolume(yup);
    }

    public void Music(float yup)
    {
        _Music.setVolume(yup);
    }

    public void SFX(float yup)
    {
        _SFX.setVolume(yup);
    }


}
