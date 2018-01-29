using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenuUI : MonoBehaviour {

	public void OnStartButton (int players) {
        GameManager.easyMode = false;
        GameManager.keyboardMode = false;
		PlayerController.playerCount = players;
		SceneManager.LoadScene("ControlsScene");
	}

    public void OnStartKeyboardButton (bool isEasy) {
        GameManager.easyMode = isEasy;
        GameManager.keyboardMode = true;
		SceneManager.LoadScene("ControlsScene");
    }

	public void OnQuitButton () {
		Application.Quit ();
	}
}
