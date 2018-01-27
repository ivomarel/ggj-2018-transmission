using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuUI : MonoBehaviour {

	public void OnStartButton (int players) {
		PlayerController.playerCount = players;
		SceneManager.LoadScene("ControlsScene");
	}


	public void OnQuitButton () {
		Application.Quit ();
	}
}
