using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputValues {
	public Pose primaryPose, secondaryPose;
	public InputsRef inputs;

    public InputValues(Pose primaryPose, Pose secondaryPose, InputsRef inputs)
    {
		this.primaryPose = primaryPose;
		this.secondaryPose = secondaryPose;
		this.inputs = inputs;
    }
		public InputValues(InputValues values)
    {
		this.primaryPose = values.primaryPose;
		this.secondaryPose = values.secondaryPose;
		this.inputs = values.inputs;
    }
}
public struct Pose //RigidTransform
{
	public Vector3 position, eulerRotation;
}
public struct InputsRef
{
	public float primaryTrigger, secondaryTrigger;
	public Vector2 primaryTrackPad, secondaryTrackPad;
	public bool primaryBTrigger, primaryBTrackPad, primaryBGrip;
	public bool secondaryBTrigger, secondaryBTrackPad, secondaryBGrip;
}
//values to helm 
public class SynthValues {
	public static float maxHeight = 2;
	public static float maxPosition= 5;
	public Pose primaryPose, secondaryPose;
	public float yMedia{
		get {
			return	(primaryPose.position.y+secondaryPose.position.y)/2;
		}
	}
	public InputsRef inputs;
    public SynthValues(InputValues values)
    {
		this.primaryPose = new Pose{
											position= new Vector3(values.primaryPose.position.x/5,values.primaryPose.position.y/maxHeight,values.primaryPose.position.z/5),
											eulerRotation = values.primaryPose.eulerRotation/360
											};
		this.secondaryPose = new Pose{ 
											position = new Vector3(values.secondaryPose.position.x/5,values.secondaryPose.position.y/maxHeight,values.secondaryPose.position.z/5),
											eulerRotation = values.secondaryPose.eulerRotation/360
											};
		this.inputs = values.inputs;
    }
}
public class SinesteticaValue
{
	private InputValues _values;
	public InputValues values{
			get{
				return _values;
			}
			set{
				_values = value;
				synthValues= new SynthValues(value);
			}
	}
	public SynthValues synthValues;
	public float[] audioData = new float[Settings.lenghtAudioData];
    public float[] audioOutputData;

    public Vector3[] oscilloScopePositions;

	public Color rightColor,leftColor;
	public SinesteticaMixerGroup sinesteticaMixer;
	public SinesteticaValue(SinesteticaValue obj){
		this.values = new InputValues(obj.values);
		this.audioData = obj.audioData;
		this.oscilloScopePositions = obj.oscilloScopePositions;
		this.sinesteticaMixer = obj.sinesteticaMixer;
		this.rightColor = obj.rightColor;
		this.leftColor = obj.leftColor;
        this.audioOutputData = obj.audioOutputData;
	}
		public SinesteticaValue(){}
}