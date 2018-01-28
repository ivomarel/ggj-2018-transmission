using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentAudio : Singleton<PersistentAudio>
{
	// Use this for initialization
	void Start ()
	{
		DontDestroyOnLoad (gameObject);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (SceneManager.GetActiveScene ().name == "GameScene") {
			Destroy (gameObject);
		}
	}
}
