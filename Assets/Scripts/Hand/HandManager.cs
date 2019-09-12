using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    private HandFSM MainFSM,SecondFSM;

    private RecordHandBehaviour recordBehaviour = new RecordHandBehaviour();
    private EditHandBehaviour editBehaviour = new EditHandBehaviour();
    private PlayHandBehaviour playBehaviour = new PlayHandBehaviour();

    // Start is called before the first frame update
    void Start()
    {
        MainFSM = new HandFSM(HandType.Primary);
        SecondFSM = new HandFSM(HandType.Secondary);
        OnPlayMode();
    }

    public void OnPlayMode()
    {
        MainFSM.ChangeBehaviour(playBehaviour);
        SecondFSM.ChangeBehaviour(recordBehaviour);
    }
    public void OnEditMode()
    {
        MainFSM.ChangeBehaviour(editBehaviour);
        SecondFSM.ChangeBehaviour(editBehaviour);
    }
    public void SwitchHand() { InputManager.Instance.isLeftHand = !InputManager.Instance.isLeftHand;}
    
}
