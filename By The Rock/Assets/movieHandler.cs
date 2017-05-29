using UnityEngine;
using System.Collections;

public class movieHandler : BaseEmitter {

	// Use this for initialization
	protected override void Start () {
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void playMovie()
    {
        Debug.Log("playing intro");
        MovieTexture movie = (MovieTexture) GetComponent<Renderer>().material.mainTexture;
        movie.Play();
		Play();
    }

    public void stopAudio()
    {
        _EventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _EventInstance.release();
    }

    /*public void playMovieWithAudio()
    {
        MovieTexture movie = (MovieTexture)GetComponent<Renderer>().material.mainTexture;
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = movie.audioClip;
        movie.Play();
        audio.Play();
    }*/
}
