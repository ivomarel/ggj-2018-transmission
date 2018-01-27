using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Blood : MonoBehaviour {

	public float fadeDelay = 3;
	public float fadeDuration = 3;

	SpriteRenderer sr;

	// Use this for initialization
	IEnumerator Start () {
		sr = GetComponentInChildren<SpriteRenderer> ();
		yield return new WaitForSeconds (fadeDelay);
		sr.DOFade(0, fadeDuration).OnComplete(() => {Destroy(gameObject);});
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
