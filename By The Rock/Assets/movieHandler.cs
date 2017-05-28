using UnityEngine;
using System.Collections;

public class movieHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        
	
	}

    public void playMovie()
    {
        Debug.Log("playing intro");
        MovieTexture movie = (MovieTexture) GetComponent<Renderer>().material.mainTexture;
        movie.Play();
    }

    public void playMovieWithAudio()
    {
        MovieTexture movie = (MovieTexture)GetComponent<Renderer>().material.mainTexture;
        AudioSource audio = GetComponent<AudioSource>();
        audio.clip = movie.audioClip;
        movie.Play();
        audio.Play();
    }
}
