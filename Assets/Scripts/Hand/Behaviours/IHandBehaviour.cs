public interface IHandBehaviour
{
    EHandBehaviour type { get; }
    HandType handType {get; set;}
    void OnStart();
    void OnExit();
    void OnGripDown();
    void OnGripUp();
    void OnTriggerDown();
    void OnTriggerUp();
    void OnTrigger(float value); 
    void OnTrackPadDown();
    void OnTrackPadUp();
    void OnTrackPad(UnityEngine.Vector2 value);
    void OnMenuButtonDown();
    void OnMenuButtonUp();
}

public enum EHandBehaviour
{
    Play,
    Edit,
    Record
}