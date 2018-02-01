using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {

    public Slider copCarSlider;
    public Slider worldSizeSlider;

    private void Start()
    {
        copCarSlider.value = GameManager.copcarSpawnInterval;
        worldSizeSlider.value = WorldGrid.nRooms;
    }

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

    public void OnCopCarSlider (float i) {
        GameManager.copcarSpawnInterval = copCarSlider.value;
    }

    public void OnWorldSizeSlider (float i) {
        WorldGrid.nRooms = (int)i;
    }
}
