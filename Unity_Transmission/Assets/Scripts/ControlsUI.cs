using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsUI : MonoBehaviour {

	public GameObject onePlayerUI;
	public GameObject twoPlayerUI;
	public GameObject threePlayerUI;

	// Use this for initialization
	void Start () {
		if (PlayerController.playerCount == 1) {
			onePlayerUI.SetActive (true);
		} else {
			onePlayerUI.SetActive (false);
		}

		if (PlayerController.playerCount == 2) {
			twoPlayerUI.SetActive (true);
		} else {
			twoPlayerUI.SetActive (false);
		}

		if (PlayerController.playerCount == 3) {
			threePlayerUI.SetActive (true);
		} else {
			threePlayerUI.SetActive (false);
		}
	}

	public void OnStartButton () {
		SceneManager.LoadScene("GameScene");
	}

	public void OnGoBackButton () {
		SceneManager.LoadScene("MenuScene");
	}
}
