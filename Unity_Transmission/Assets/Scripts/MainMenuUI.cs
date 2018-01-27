using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour {

	public void OnStartButton () {
		SceneManager.LoadScene ("ControlsScene");
	}


	public void OnQuitButton () {

	}
}
