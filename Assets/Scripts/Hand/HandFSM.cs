using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum HandType
{     
    Primary,
    Secondary,
    Null
}
public class HandFSM
{
    public HandType type;
    private IHandBehaviour currentBehaviour;
    public HandFSM(HandType type)
    {
        // this.type = type;
        // if(type==HandType.Primary)
        //     SubscribePrimaryInputEvents();
        // else 
        //     SubscribeSecondaryInputEvents();
    }
    public void ChangeBehaviour(IHandBehaviour newBehaviour)
    {
        if(currentBehaviour != null) currentBehaviour.OnExit();
        newBehaviour.handType = type;
        newBehaviour.OnStart();
        currentBehaviour = newBehaviour;
    }
    // void SubscribePrimaryInputEvents(){
    //     InputManager.OnPrimaryTriggerDown+=OnTriggerDown;
    //     InputManager.OnPrimaryTriggerUp+=OnTriggerUp;
    //     InputManager.OnPrimaryTouchPadDown+=OnTrackPadDown;
    //     InputManager.OnPrimaryTouchPadUp+=OnTrackPadUp;
    //     InputManager.OnPrimaryGripDown+=OnGripDown;
    //     InputManager.OnPrimaryGripUp+=OnGripUp;
    //     InputManager.OnPrimaryTrigger+=OnTrigger;
    //     InputManager.OnPrimaryTouchPad+=OnTrackPad;
    // }
    // void SubscribeSecondaryInputEvents(){
    //     InputManager.OnSecondaryTriggerDown+=OnTriggerDown;
    //     InputManager.OnSecondaryTriggerUp+=OnTriggerUp;
    //     InputManager.OnSecondaryTouchPadDown+=OnTrackPadDown;
    //     InputManager.OnSecondaryTouchPadUp+=OnTrackPadUp;
    //     InputManager.OnSecondaryGripDown+=OnGripDown;
    //     InputManager.OnSecondaryGripUp+=OnGripUp;
    //     InputManager.OnSecondaryTrigger+=OnTrigger;
    //     InputManager.OnSecondaryTouchPad+=OnTrackPad;
    // }
    public EHandBehaviour GetCurrentBehaviourType() { return currentBehaviour.type; }
    public void OnGripDown(){currentBehaviour.OnGripDown();}
    public void OnGripUp(){currentBehaviour.OnGripUp();}
    public void OnTrackPadDown(){currentBehaviour.OnTrackPadDown();}
    public void OnTrackPadUp(){currentBehaviour.OnTrackPadUp();}
    public void OnTriggerDown(){currentBehaviour.OnTriggerDown();}
    public void OnTriggerUp(){currentBehaviour.OnTriggerUp();}
    void OnTrigger(float value){ currentBehaviour.OnTrigger(value);}
    void OnTrackPad(Vector2 value){ currentBehaviour.OnTrackPad(value); }
}
