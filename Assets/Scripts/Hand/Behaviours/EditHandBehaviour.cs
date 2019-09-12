using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditHandBehaviour : IHandBehaviour
{
    public EHandBehaviour type { get{ return EHandBehaviour.Edit;}
    }
    HandType _handType = HandType.Null;
    public HandType handType
    {
        get
        {
            return _handType;
        }
        set
        {
            _handType = value;
        }
    }

      public void OnExit()
    {
   
    }

    public void OnGripDown()
    {
      
    }

    public void OnGripUp()
    {
 
    }

    public void OnMenuButtonDown()
    {
        throw new System.NotImplementedException();
    }

    public void OnMenuButtonUp()
    {
        throw new System.NotImplementedException();
    }

    public void OnStart()
    {
       
    }

    public void OnTrackPad(Vector2 value)
    {
       
    }

    public void OnTrackPadDown()
    {
     
    }

    public void OnTrackPadUp()
    {
       
    }

    public void OnTrigger(float value)
    {
      
    }

    public void OnTriggerDown()
    {
       
    }

    public void OnTriggerUp()
    {

    }
}
