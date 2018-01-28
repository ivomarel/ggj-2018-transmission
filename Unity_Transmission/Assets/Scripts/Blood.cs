using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Blood : MonoBehaviour
{

	public float fadeDelay = 3;
	public float fadeDuration = 3;

	public AudioClip[] deathSounds;

	SpriteRenderer sr;

	// Use this for initialization
	IEnumerator Start ()
	{
		int r = Random.Range (0, deathSounds.Length);
		GetComponent<AudioSource> ().PlayOneShot (deathSounds [r]);

		sr = GetComponentInChildren<SpriteRenderer> ();
		yield return new WaitForSeconds (fadeDelay);
		sr.DOFade (0, fadeDuration).OnComplete (() => {
			Destroy (gameObject);
		});
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
