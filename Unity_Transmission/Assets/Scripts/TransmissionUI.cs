using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if !PLATFORM_STANDALONE_OSX
using XInputDotNetPure;
#endif
public class TransmissionUI : MonoBehaviour
{

    public Transform knob;
    public int gearCount;
    public Vector3 gearBoxSize;
    public float gearBoxPadding;

    // The gear area the stick is in. we cannot switch directly to another gear
    private int currentGearArea = -1;

    private float gearRad;

   public  Vector2 transmissionInput;

    float pos = 0;

    // Use this for initialization
    void Start()
    {
        gearRad = 360.0f / (float)gearCount * Mathf.Deg2Rad;
        gearBoxSize = GetComponent<RectTransform>().sizeDelta / 2 + new Vector2(gearBoxPadding, gearBoxPadding);

        if (GameManager.keyboardMode)
        {
            pos = (3f / 12f) * 2 * Mathf.PI;
			PlayerController.instance.updateGear(1);
        }
    }

    // Update is called once per frame
    void Update()
    {

#if !PLATFORM_STANDALONE_OSX
		GamePadState state = GamePad.GetState (PlayerController.instance.gearPlayerIndex);
		GamePadThumbSticks.StickValue transmissionInputStick = state.ThumbSticks.Right;
        transmissionInput = StickValueToV2(transmissionInputStick);
#endif
        if (GameManager.keyboardMode)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                pos = (3f / 12f) * 2 * Mathf.PI;
                PlayerController.instance.updateGear(1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                pos = (1f / 12f) * 2 * Mathf.PI;
                PlayerController.instance.updateGear(2);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                pos = (11f / 12f) * 2 * Mathf.PI;
                PlayerController.instance.updateGear(3);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                pos = (9f / 12f) * 2 * Mathf.PI;
                PlayerController.instance.updateGear(4);
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
				pos = (7f / 12f) * 2 * Mathf.PI;
				PlayerController.instance.updateGear(5);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                pos = (5f / 12f) * 2 * Mathf.PI;
                PlayerController.instance.updateGear(0);
            }

            knob.transform.localPosition = new Vector3(Mathf.Cos(pos), Mathf.Sin(pos)) * 35f;
        }
        else
        {
            if (inputStrength(transmissionInput) <= PlayerController.instance.freeGearThreshold)
            {
                //Debug.LogWarning ("Input strength" + inputStrength(transmissionInput));
                freeGearHandler(transmissionInput);
            }
            else
            {
                lockedGearHandler(transmissionInput);
            }

        }
    }
#if !PLATFORM_STANDALONE_OSX
    Vector2 StickValueToV2 (GamePadThumbSticks.StickValue input) {
        return new Vector2(input.X, input.Y);
    }
#endif
    float getGearRad(int gear) {
		return gearRad + Mathf.PI / 2 - gear*gearRad;
	}

	float inputStrength(Vector2 input) {
		return Mathf.Sqrt (input.x * input.y + input.x * input.y);
	}

	float inputAngle(Vector2 input) {
		return Mathf.Atan2 (input.y, input.x) % (2 * Mathf.PI);
	}

	void freeGearHandler(Vector2 input) {
		knob.localPosition = Vector3.Scale(new Vector3 (input.x, input.y), gearBoxSize);
		PlayerController.instance.updateGear(-1);
		currentGearArea = -1;
	}

	void lockedGearHandler(Vector2 input) {
		if (currentGearArea == -1) {
			float normalizedAngle = (Mathf.PI / 2 + gearRad + 2 * Mathf.PI - inputAngle (input)) / gearRad;
			currentGearArea = Mathf.FloorToInt(normalizedAngle + 0.5f) % gearCount;
		}
	//	Debug.Log ("Closest gear area" + currentGearArea);

		float maxGearAreaAngle = getGearRad(currentGearArea) + PlayerController.instance.gearRestrictedRoamingProportion * gearRad / 2 ;
		float minGearAreaAngle = getGearRad(currentGearArea) - PlayerController.instance.gearRestrictedRoamingProportion * gearRad / 2 ;

		//Debug.Log(minGearAreaAngle + ", " + inputAngle (input) + ", " + maxGearAreaAngle);

		float clampedAngle = Mathf.Clamp(inputAngle(input) + 2*Mathf.PI, minGearAreaAngle + 2*Mathf.PI, maxGearAreaAngle + 2*Mathf.PI);

		knob.localPosition = Vector3.Scale(
			new Vector3 (
				Mathf.Cos(clampedAngle) * inputStrength(input),
				Mathf.Sin(clampedAngle) * inputStrength(input),
				0
			),
			gearBoxSize
		);

		if ( inputStrength(input) > PlayerController.instance.acceptedGearThreshold) {
			if (currentGearArea == -1) {
				Debug.LogError("Gear validated without valid Gear area !!!");
			}
			PlayerController.instance.updateGear(currentGearArea);
		}
	}

	void SelectGear() {
	}
}
 
