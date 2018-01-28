using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{

	public void OnBackToMenu ()
	{
		SceneManager.LoadScene ("MenuScene");
	}
}
