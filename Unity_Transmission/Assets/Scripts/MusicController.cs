using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicController : MonoBehaviour {
	
	public AudioClip[] music;
	public float volume= 1.0f;
	private AudioSource audioSource;
	private int musicIndex = 0;

	// Use this for initialization
	void Start () {
		//MusicController.play ();
		audioSource = GetComponent<AudioSource>();

		audioSource.clip = music[musicIndex];
		audioSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
		if (!audioSource.isPlaying) {
			musicIndex = (musicIndex + 1) % music.Length;
			audioSource.clip = music[musicIndex];
			audioSource.Play();
		}
		//audio.clip = music[musicIndex];
		//return new WaitForSeconds(audio.AudioClip.length);
		//audio.Play();
		
	}
}
