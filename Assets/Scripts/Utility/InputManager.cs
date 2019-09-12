using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance
    {
        get
        {
            return _instance;
        }
    }
    // public delegate void BooleanEvent();
    // public delegate void FloatEvent(float value);
    // public delegate void Vector2Event(Vector2 value);
    // public delegate void PoseEvent(Pose primaryPose, Pose secondaryPose);

    public SteamVR_TrackedObject RightHand, LeftHand;
    private SteamVR_Controller.Device rDevice, lDevice;
    public bool _isLeftHand = false;
    public bool isKeyboardInput = false;
    public bool isLeftHand
    {
        get
        {
            return _isLeftHand;
        }
        set
        {
            _isLeftHand = value;
            //	OnSwitchHand();
        }
    }
    // public static event BooleanEvent OnSwitchHand;
    // public static event BooleanEvent OnPrimaryTriggerDown,OnPrimaryTriggerUp,OnPrimaryGripDown,OnPrimaryGripUp,OnPrimaryTouchPadDown,OnPrimaryTouchPadUp;
    // public static event BooleanEvent OnSecondaryTriggerDown,OnSecondaryTriggerUp,OnSecondaryGripDown,OnSecondaryGripUp,OnSecondaryTouchPadDown,OnSecondaryTouchPadUp;
    // public static event FloatEvent OnPrimaryTrigger,OnSecondaryTrigger;
    // public static event Vector2Event OnPrimaryTouchPad,OnSecondaryTouchPad;
    // public static event PoseEvent OnPose;
    #region Input References
    public Vector2 rTrigger, rTrackPad, rGrip, rMenuButton, rSettings;
    public Vector2 lTrigger, lTrackPad, lGrip, lMenuButton, lSettings;
    public bool rBTrigger, rBTrackPad, rBGrip, rBMenuButton, rBSettings;
    public bool lBTrigger, lBTrackPad, lBGrip, lBMenuButton, lBSettings;
    public Vector3 rPosition, rEulerRotation;
    public Vector3 lPosition, lEulerRotation;
    #endregion
    public InputValues curent = new InputValues(new Pose(), new Pose(), new InputsRef());

    public bool rightPulse, leftPulse;
    public ushort rightForcePulse, leftForcePulse = 500;
    // Use this for initialization
    void Awake()
    {
        if (_instance != null) Destroy(this);
        else _instance = this;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isKeyboardInput) InputManagement();
        else InputKeyboard();
        GetPoses();
    }

    void InputManagement()
    {
        rDevice = SteamVR_Controller.Input((int)RightHand.index);
        lDevice = SteamVR_Controller.Input((int)LeftHand.index);

        #region GetAxis And Bool From the Controllers	
        var rTrigger = rDevice.GetAxis(EVRButtonId.k_EButton_SteamVR_Trigger);
        var rTrackPad = rDevice.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
        var rGrip = rDevice.GetAxis(EVRButtonId.k_EButton_Grip);
        var rMenuButton = rDevice.GetAxis(EVRButtonId.k_EButton_ApplicationMenu);
        var rSettings = rDevice.GetAxis(EVRButtonId.k_EButton_System);
        var rBTrigger = rDevice.GetPress(SteamVR_Controller.ButtonMask.Trigger);
        var rBTrackPad = rDevice.GetPress(SteamVR_Controller.ButtonMask.Touchpad);
        var rBGrip = rDevice.GetPress(SteamVR_Controller.ButtonMask.Grip);
        var rBMenuButton = rDevice.GetPress(SteamVR_Controller.ButtonMask.ApplicationMenu);
        var rBSettings = rDevice.GetPress(SteamVR_Controller.ButtonMask.System);

        var lTrigger = lDevice.GetAxis(EVRButtonId.k_EButton_SteamVR_Trigger);
        var lTrackPad = lDevice.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
        var lGrip = lDevice.GetAxis(EVRButtonId.k_EButton_Grip);
        var lMenuButton = lDevice.GetAxis(EVRButtonId.k_EButton_ApplicationMenu);
        var lSettings = lDevice.GetAxis(EVRButtonId.k_EButton_System);
        var lBTrigger = lDevice.GetPress(SteamVR_Controller.ButtonMask.Trigger);
        var lBTrackPad = lDevice.GetPress(SteamVR_Controller.ButtonMask.Touchpad);
        var lBGrip = lDevice.GetPress(SteamVR_Controller.ButtonMask.Grip);
        var lBMenuButton = lDevice.GetPress(SteamVR_Controller.ButtonMask.ApplicationMenu);
        var lBSettings = lDevice.GetPress(SteamVR_Controller.ButtonMask.System);
        #endregion

        #region Calling events by correct source
        // if(!isLeftHand)
        // { //
        //     if(rBTrigger && !this.rBTrigger)//OnPrimaryTriggerDown();
        //     if(!rBTrigger && this.rBTrigger) //OnPrimaryTriggerUp();
        //     if(rBGrip && !this.rBGrip) //OnPrimaryGripDown();
        //     if(!rBGrip && this.rBGrip) //OnPrimaryGripUp();
        //     if(rBTrackPad && !this.rBTrackPad) //OnPrimaryTouchPadDown();
        //     if(!rBTrackPad && this.rBTrackPad) //OnPrimaryTouchPadUp();

        // 	if(rBTrigger) //OnPrimaryTrigger(rTrigger.x);
        // 	if(rBTrackPad) //OnPrimaryTouchPad(rTrackPad);	

        // 	if(lBTrigger && !this.lBTrigger) //OnSecondaryTriggerDown();
        //     if(!lBTrigger && this.lBTrigger) //OnSecondaryTriggerUp();
        //     if(lBGrip && !this.lBGrip) //OnSecondaryGripDown();
        //     if(!lBGrip && this.lBGrip) //OnSecondaryGripUp();
        //     if(lBTrackPad && !this.lBTrackPad) //OnSecondaryTouchPadDown();
        //     if(!lBTrackPad && this.lBTrackPad) //OnSecondaryTouchPadUp();

        // 	if(lBTrigger) //OnSecondaryTrigger(lTrigger.x);
        // 	if(lBTrackPad) //OnSecondaryTouchPad(lTrackPad);

        // }else{

        //     if(lBTrigger && !this.lBTrigger) //OnPrimaryTriggerDown();
        //     if(!lBTrigger && this.lBTrigger) //OnPrimaryTriggerUp();
        //     if(lBGrip && !this.lBGrip) //OnPrimaryGripDown();
        //     if(!lBGrip && this.lBGrip) //OnPrimaryGripUp();
        //     if(lBTrackPad && !this.lBTrackPad) //OnPrimaryTouchPadDown();
        //     if(!lBTrackPad && this.lBTrackPad) //OnPrimaryTouchPadUp();

        // 	if(lBTrigger) //OnPrimaryTrigger(lTrigger.x);
        // 	if(lBTrackPad) //OnPrimaryTouchPad(lTrackPad);	

        //     if(rBTrigger && !this.rBTrigger) //OnSecondaryTriggerDown();
        //     if(!rBTrigger && this.rBTrigger) //OnSecondaryTriggerUp();
        //     if(rBGrip && !this.rBGrip) //OnSecondaryGripDown();
        //     if(!rBGrip && this.rBGrip) //OnSecondaryGripUp();
        //     if(rBTrackPad && !this.rBTrackPad) //OnSecondaryTouchPadDown();
        //     if(!rBTrackPad && this.rBTrackPad) //OnSecondaryTouchPadUp();   

        // 	if(rBTrigger) //OnSecondaryTrigger(rTrigger.x);
        // 	if(rBTrackPad) //OnSecondaryTouchPad(rTrackPad);
        // }
        #endregion

        if (!isLeftHand)
        {
            curent.inputs.primaryBTrigger = rBTrigger;
            curent.inputs.primaryBTrackPad = rBTrackPad;
            curent.inputs.primaryBGrip = rBGrip;
            curent.inputs.primaryTrigger = rTrigger.x;
            curent.inputs.primaryTrackPad = rTrackPad;

            curent.inputs.secondaryBTrigger = lBTrigger;
            curent.inputs.secondaryBTrackPad = lBTrackPad;
            curent.inputs.secondaryBGrip = lBGrip;
            curent.inputs.secondaryTrigger = lTrigger.x;
            curent.inputs.secondaryTrackPad = lTrackPad;
        }
        else
        {
            curent.inputs.primaryBTrigger = lBTrigger;
            curent.inputs.primaryBTrackPad = lBTrackPad;
            curent.inputs.primaryBGrip = lBGrip;
            curent.inputs.primaryTrigger = lTrigger.x;
            curent.inputs.primaryTrackPad = lTrackPad;

            curent.inputs.secondaryBTrigger = rBTrigger;
            curent.inputs.secondaryBTrackPad = rBTrackPad;
            curent.inputs.secondaryBGrip = rBGrip;
            curent.inputs.secondaryTrigger = rTrigger.x;
            curent.inputs.secondaryTrackPad = rTrackPad;
        }
        #region Giving the information of Axis and Booleans to external ref
        this.rTrigger = rTrigger;
        this.rTrackPad = rTrackPad;
        this.rGrip = rGrip;
        this.rMenuButton = rMenuButton;
        this.rSettings = rSettings;
        this.rBTrigger = rBTrigger;
        this.rBTrackPad = rBTrackPad;
        this.rBGrip = rBGrip;
        this.rBMenuButton = rBMenuButton;
        this.rBSettings = rBSettings;
        this.lTrigger = lTrigger;
        this.lTrackPad = lTrackPad;
        this.lGrip = lGrip;
        this.lMenuButton = lMenuButton;
        this.lSettings = lSettings;
        this.lBTrigger = lBTrigger;
        this.lBTrackPad = lBTrackPad;
        this.lBGrip = lBGrip;
        this.lBMenuButton = lBMenuButton;
        this.lBSettings = lBSettings;
        #endregion
        if (this.rBTrigger)
        {
            if (rightPulse)
                rDevice.TriggerHapticPulse(rightForcePulse);

            if (leftPulse)
                lDevice.TriggerHapticPulse(leftForcePulse);
        }
    }
    void InputKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rTrigger = new Vector2(1, 0);
            rBTrigger = true;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            rTrigger = new Vector2(0, 0);
            rBTrigger = false;
            //OnPrimaryTrigger(0);
            //OnPrimaryTriggerUp();
        }
        curent.inputs.primaryBTrigger = rBTrigger;
        curent.inputs.primaryTrigger = rTrigger.x;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            lTrigger = new Vector2(1, 0);
            lBTrigger = true;
            //OnSecondaryTrigger(1);
            //OnSecondaryTriggerDown();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            lTrigger = new Vector2(0, 0);
            lBTrigger = false;
            //OnSecondaryTrigger(0);
            //OnSecondaryTriggerUp();
        }
        curent.inputs.secondaryBTrigger = lBTrigger;
        curent.inputs.secondaryTrigger = lTrigger.x;

        if (Input.GetKeyDown(KeyCode.R))
        {
            lTrackPad = new Vector2(0, 0);
            lBTrackPad = true;
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            lTrackPad = new Vector2(0, 0);
            lBTrackPad = false;
        }
        curent.inputs.secondaryBTrackPad = lBTrackPad;
        curent.inputs.secondaryTrackPad = lTrackPad;

    }
    void GetPoses()
    {
        rPosition = RightHand.gameObject.transform.position;
        rEulerRotation = RightHand.gameObject.transform.eulerAngles;
        lPosition = LeftHand.gameObject.transform.position;
        lEulerRotation = LeftHand.gameObject.transform.eulerAngles;
        if (!isLeftHand)
        {
            curent.primaryPose = new Pose
            {
                position = rPosition,
                eulerRotation = rEulerRotation
            };
            curent.secondaryPose = new Pose
            {
                position = lPosition,
                eulerRotation = lEulerRotation
            };
        }
        else
        {
            curent.primaryPose = new Pose
            {
                position = lPosition,
                eulerRotation = lEulerRotation
            };
            curent.secondaryPose = new Pose
            {
                position = rPosition,
                eulerRotation = rEulerRotation
            };
        }
    }

    // public void TriggerHaptic(HandType type, int intensity){
    // 	if(isKeyboardInput) return;


    // 	if(type==HandType.Primary)
    // 	{
    // 		var rDevice = SteamVR_Controller.Input((int)RightHand.index);
    // 		if(rDevice == null){ 
    // 			Debug.LogWarning("Right Controller not detected!");
    // 			return;
    // 		}
    // 		rDevice.TriggerHapticPulse((ushort)intensity);
    // 	}
    // 	else if(type==HandType.Secondary)
    // 	{
    // 		var lDevice = SteamVR_Controller.Input((int)LeftHand.index);
    // 		if(lDevice == null){ 
    // 			Debug.LogWarning("Left Controller not detected!");
    // 			return;
    // 		}
    // 		lDevice.TriggerHapticPulse((ushort)intensity);
    // 	}
    // }
}
