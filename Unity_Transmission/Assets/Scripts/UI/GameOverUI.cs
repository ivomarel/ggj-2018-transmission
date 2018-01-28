using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
	public GameObject victory;
	public GameObject failure;

	// Use this for initialization
	void Start () {
		if (PlayerController.victory == true) {
			victory.SetActive (true);
			failure.SetActive (false);
		} else {
			victory.SetActive (false);
			failure.SetActive (true);
		}
	}


	public void OnBackToMenu ()
	{
		SceneManager.LoadScene ("MenuScene");
	}
	// Update is called once per frame
	void Update () {
		
	}
}
