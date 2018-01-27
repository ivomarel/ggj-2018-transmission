using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsUI : MonoBehaviour {

	public void OnStartButton () {
		SceneManager.LoadScene ("GameScene");
	}

	public void OnGoBackButton () {
		SceneManager.LoadScene ("MenuScene");
	}
}
