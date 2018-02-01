using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if !PLATFORM_STANDALONE_OSX
using XInputDotNetPure;
#endif
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : Singleton<PlayerController>
{

    /// <summary>
    /// The speed increase when fully pushing the controller
    /// </summary>
    /// 

    public AudioClip crashSound;

    public static int playerCount = 1;
    public static bool victory = false;

    public float victoryThreshold = 0.8f;

    [Header("Gears")]
#if !PLATFORM_STANDALONE_OSX
	public PlayerIndex gearPlayerIndex;
#endif
    public Gear[] gears;

    [Range(0, 1)]
    public float acceptedGearThreshold = 0.8f;
    [Range(0, 1)]
    public float freeGearThreshold = 0.4f;
    [Range(0, 1)]
    public float gearRestrictedRoamingProportion = 0.15f;

    public float badGearRumbleMultiplier;

    internal int currentGearIndex;

    public float health;
    internal float maxHealth;

    public Gear currentGear
    {
        get
        {
            if (currentGearIndex == -1)
            {
                return new Gear()
                {
                    isNeutral = true
                };
            }
            return gears[currentGearIndex];
        }
    }

     bool keyboardMode {
        get {
            return GameManager.keyboardMode;
        } 
    }

    internal float maxSpeed {
        get {
            return gears[gears.Length - 1].maxSpeed;
        }
    }

    //public AnimationCurve speedUpCurve;
    [Header("Speeding")]
#if !PLATFORM_STANDALONE_OSX
	public PlayerIndex speedPlayerIndex;
#endif
    public float speedIncrease;
    public float speedAutoDecrease;
    public float speedBreakDecrease;
    public float maxBackwardsSpeed;
    // public float maxSpeed;

    internal float currentSpeed;

    [Header("Steering")]
#if !PLATFORM_STANDALONE_OSX
	public PlayerIndex steerPlayerIndex;
#endif
    public float steeringSpeed;
    public float steerAutoDecrease;
    public float maxSteer;

    private float score;
    private float maxScore = 100f;

    internal float currentSteerRotation;

    //Components
    Rigidbody rb;
#if !PLATFORM_STANDALONE_OSX
	ButtonState prevState;
#endif
    float horizontalInput;
    float verticalInput;

    private PartyGuy[] guys;

    public AudioClip[] carPeopleSounds;

    // Use this for initialization
    IEnumerator Start()
    {
        maxHealth = health;
        rb = GetComponent<Rigidbody>();
        InvokeRepeating("RandomCar", 10, 10);
        yield return null;
        yield return null;
        guys = FindObjectsOfType<PartyGuy>();
        setPlayersIndexes();
    }

    void RandomCar()
    {
        GetComponent<AudioSource>().PlayOneShot(carPeopleSounds[Random.Range(0, carPeopleSounds.Length)]);
    }


    // Update is called once per frame
    void Update()
    {
        Speeding();
        Steering();

        UpdateScore();
        // FIXME should not be done every update when menu is setup
        checkVictory();
    }

    void UpdateScore()
    {
        if (guys == null)
            return;

        float converted = 0;
        float total = 0;
        foreach (PartyGuy guy in guys)
        {
            if (guy == null)
                continue;
            if (guy.currentState == PartyGuy.State.Trancing)
            {
                converted++;
            }
            total++;
        }
        score = converted;
        maxScore = total;
    }

    public float getPlayerScore()
    {
        return score / maxScore;
    }

    public void updateGear(int gear)
    {
        currentGearIndex = gear;
    }

    public float getSpeed()
    {
        return currentSpeed;
    }

    void Speeding()
    {
        if (keyboardMode)
        {
            verticalInput = Input.GetAxis("Vertical");

			if (!GameManager.easyMode) {
               verticalInput = Mathf.Max(0, verticalInput);
            }
        }
        else
        {
#if PLATFORM_STANDALONE_OSX

#else
            GamePadState state = GamePad.GetState (speedPlayerIndex);

			verticalInput = state.Triggers.Right;
#endif
        }
        //	verticalInput = Mathf.Max (0, verticalInput);

        currentSpeed = transform.InverseTransformVector(rb.velocity).z;

        currentSpeed *= (1 - speedAutoDecrease * Time.deltaTime);

        if (GameManager.easyMode)
        {
            currentSpeed +=  2 * verticalInput * Time.deltaTime;
        }
        else
        {
            if (!currentGear.isNeutral)
            {
                //Only adding gas when not neutral
                //0 is reverse
                if (currentGearIndex == 0)
                {
                    currentSpeed -= speedIncrease * verticalInput * Time.deltaTime;
                    if (currentSpeed <= -2)
                        currentSpeed = -2;
                }
                else if (currentSpeed >= currentGear.minSpeed && currentSpeed <= currentGear.maxSpeed)
                {
                    currentSpeed += speedIncrease * verticalInput * Time.deltaTime;
#if !PLATFORM_STANDALONE_OSX
                GamePad.SetVibration(gearPlayerIndex, 0f, 0f);
#endif
                }
                else
                {
                    // Rumble we reach the max or min speed, gear is not fit
                    setClutchRumble(currentSpeed, currentGear.minSpeed, currentGear.maxSpeed);
                }
            }
            else
            {
#if !PLATFORM_STANDALONE_OSX
			GamePad.SetVibration (gearPlayerIndex, 0f, 0f);
#endif
            }

        }
        //currentSpeed = Mathf.Clamp(currentSpeed, -maxBackwardsSpeed, currentGear.maxSpeed);

        //Actually applying to the RB
        rb.velocity = transform.TransformVector(new Vector3(0, 0, currentSpeed));
    }

    void setClutchRumble(float speed, float minSpeed, float maxSpeed)
    {
        float powerX = 0f;
        float powerY = 0f;
        if (speed < minSpeed)
        {
            powerX = minSpeed - speed;
        }
        if (maxSpeed < speed)
        {
            powerY = speed - maxSpeed;
        }
#if !PLATFORM_STANDALONE_OSX
		GamePad.SetVibration (gearPlayerIndex, powerX, powerY);
#endif
    }

    void Steering()
    {
        if (keyboardMode)
        {
            horizontalInput = Input.GetAxis("Horizontal");
        }
        else
        {
#if PLATFORM_STANDALONE_OSX

#else
			GamePadState state = GamePad.GetState (steerPlayerIndex);

			horizontalInput = state.ThumbSticks.Left.X;
#endif
        }
        //if (Mathf.Abs (horizontalInput) < 0.1f) {
        currentSteerRotation *= (1 - steerAutoDecrease * Time.deltaTime);
        //}

        currentSteerRotation += horizontalInput * steeringSpeed * Time.deltaTime;
        currentSteerRotation = Mathf.Clamp(currentSteerRotation, -maxSteer, maxSteer);


        //Actually applying to the RB
        rb.angularVelocity = new Vector3(0, currentSteerRotation * currentSpeed);
    }

    void setPlayersIndexes()
    {
#if !PLATFORM_STANDALONE_OSX
        speedPlayerIndex = PlayerIndex.One;
		gearPlayerIndex = PlayerIndex.One;
		steerPlayerIndex = PlayerIndex.One;
		BottomBar.instance.updatePlayersControlsUI (1, 1, 1);

		if (playerCount == 2) {
			gearPlayerIndex = PlayerIndex.Two;
			BottomBar.instance.updatePlayersControlsUI (1, 1, 2);
		}

		if (playerCount == 3) {
			speedPlayerIndex = PlayerIndex.Two;
			gearPlayerIndex = PlayerIndex.Three;
			BottomBar.instance.updatePlayersControlsUI (2, 1, 3);
		}
#endif
    }


	void OnCollisionEnter (Collision collisionInfo)
	{
		CopCar cop = collisionInfo.collider.GetComponent<CopCar> ();
		//if (cop != null) 
		{
			float hitAmount = collisionInfo.relativeVelocity.magnitude;
			health -= hitAmount;
			GetComponent<AudioSource> ().PlayOneShot (crashSound, hitAmount / 10);
			if (health <= 0) {
				SceneManager.LoadScene ("GameOver");
			}
		}
	}

	void checkVictory ()
	{
		//Debug.Log (score / maxScore);
		//	Debug.Log (victoryThreshold);
		if (score / maxScore > victoryThreshold) {
			Debug.Log ("success");
			victory = true;
			SceneManager.LoadScene ("GameOver");
		}
	}
}

[System.Serializable]
public struct Gear
{
	public float minSpeed;
	public float maxSpeed;
	public float speedIncrease;
	internal bool isNeutral;
}
