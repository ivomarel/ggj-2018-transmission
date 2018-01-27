using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Xbox : MonoBehaviour {

	static GamePadState[] prevStates = new GamePadState[4];

	public static GamePadState GetState (PlayerIndex p) {
		return GamePad.GetState(p);
	}

	public static GamePadState GetPrevState (PlayerIndex p) {
		for (int i = 0; i < 4; ++i) {
			if (i == (int)p) {
				return prevStates[i];
			}
		}
		Debug.LogErrorFormat("Getting previous state '{0}' but this did not exist. Is your Xbox script added to the scene?", p);
		return new GamePadState();
	}



	void Update () {
		for (int i = 0; i < 4; ++i) {
			PlayerIndex testPlayerIndex = (PlayerIndex)i;
			prevStates[i] = GamePad.GetState (testPlayerIndex);
		}
	}
}
