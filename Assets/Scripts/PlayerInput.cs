//This script handles reading inputs from the player and passing it on to the vehicle. We 
//separate the input code from the behaviour code so that we can easily swap controls 
//schemes or even implement and AI "controller". Works together with the VehicleMovement script

using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	public string verticalAxisName = "Vertical";        //The name of the thruster axis
	public string horizontalAxisName = "Horizontal";    //The name of the rudder axis
	public string brakingKey = "Brake";                 //The name of the brake button

	//We hide these in the inspector because we want 
	//them public but we don't want people trying to change them
	[HideInInspector] public float thruster;			//The current thruster value
	[HideInInspector] public float rudder;				//The current rudder value
	[HideInInspector] public bool isBraking;			//The current brake value

    Vector2 startPos;
    Vector2 direction;

    void Update()
	{
		//If the player presses the Escape key and this is a build (not the editor), exit the game
		if (Input.GetButtonDown("Cancel") && !Application.isEditor)
			Application.Quit();

		//If a GameManager exists and the game is not active...
		if (GameManager.instance != null && !GameManager.instance.IsActiveGame())
		{
			//...set all inputs to neutral values and exit this method
			thruster = rudder = 0f;
			isBraking = false;
			return;
		}

        //Get the values of the thruster, rudder, and brake from the input class
        //thruster = Input.GetAxis(verticalAxisName);
        //rudder = Input.GetAxis(horizontalAxisName);
        thruster = 0.6f;
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Handle finger movements based on touch phase.
            switch (touch.phase)
            {
                // Record initial touch position.
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;

                // Determine direction by comparing the current touch position with the initial one.
                case TouchPhase.Moved:
                    direction = touch.position - startPos;
                    break;

                // Report that a direction has been chosen when the finger is lifted.
                case TouchPhase.Ended:
                    direction = Vector2.zero;
                    break;
            }

            Debug.Log(string.Format("start pos: {0}, end pos: {1}", startPos, touch.position));
        }
        rudder = Mathf.Min(direction.x / Screen.width * 2, 1) * 0.85f;
        isBraking = Input.GetButton(brakingKey);
	}
}
