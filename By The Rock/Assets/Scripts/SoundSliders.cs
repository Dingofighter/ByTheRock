using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SoundSliders : MonoBehaviour
{
    //Linus smells

    System.Guid[] guid = new System.Guid[5];
    string[] guidID = new string[5] { "{73d23202-a63d-4ade-8032-b0d3af7998a0}", "{f5314b91-ab5f-4a46-afb4-880bb6f022fa}", "{180bbb4a-48c6-4c6a-8c07-60e5f7928c57}", "{195ee83c-19c6-42e9-8337-e5f5b47b4868}", "{c537d2cf-a3c2-4b27-bebe-63a0769290a3}" };
    private GameManager gm;
    FMOD.Studio.Bus _Master;
    FMOD.Studio.Bus _Ambience;
    FMOD.Studio.Bus _Dialogues;
    FMOD.Studio.Bus _Music;
    FMOD.Studio.Bus _SFX;

    public Slider _MSlider;
    public Slider _ASlider;
    public Slider _DSlider;
    public Slider _MUSlider;
    public Slider _SFXSlider;

    float masterVolume;
    float ambienceVolume;
    float dialogueVolume;
    float musicVolume;
    float sfxVolume;

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

        _Master.getVolume(out masterVolume, out masterVolume);
        _MSlider.value = masterVolume;
        _Ambience.getVolume(out ambienceVolume, out ambienceVolume);
        _ASlider.value = ambienceVolume;
        _Dialogues.getVolume(out dialogueVolume, out dialogueVolume);
        _DSlider.value = dialogueVolume;
        _Music.getVolume(out musicVolume, out musicVolume);
        _MUSlider.value = musicVolume;
        _SFX.getVolume(out sfxVolume, out sfxVolume);
        _SFXSlider.value = sfxVolume;
    }

    private void Update()
    {
        Master(_MSlider.value);
        Ambience(_ASlider.value);
        Dialogues(_DSlider.value);
        Music(_MUSlider.value);
        SFX(_SFXSlider.value);
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