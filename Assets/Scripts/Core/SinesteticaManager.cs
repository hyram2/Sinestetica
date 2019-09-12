using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioHelm;
using UnityEngine.Audio;
using UnityEditor;
public class SinesteticaManager : MonoBehaviour {
	private static SinesteticaManager _instance;
	public static SinesteticaManager Instance
	{
		get{
			return _instance;
		}
	}
	[Header("Sinestetica Params")]
	public SinesteticaObject[] sinesteticaInstances;
	public SinesteticaMixerGroup[] sinesteticaAudioGroups;
	public SinesteticaObject playableObject;

	public int nextChannel = 0;
	public int curentAudioGroupIndex = 0;
	public const int channelIndexLimmit = 15;

	[Header("Sampler Params")]
	public AudioClip[] samples;
	public GameObject samplerPrefab;
	public List<SinesteticaObject> samplerObjects = new List<SinesteticaObject>();
	public SinesteticaObject curentSampler;
	public int nextSample = 0;

	[Header("Mic Params")]
	public GameObject micPrefab;
	public SinesteticaObject curentMic;
	bool isPlaying = false;
	bool isRecording = false;
	bool isRemoving = false;
	bool isChangeAudioMixer = false;
	bool isChangeType= false;
	
	[Header("Sampler Params")]
	public SinesteticaHand RightHand;
	public SinesteticaHand LeftHand;

    [Header("Record Params")]
    public int dataLenght = 44100;
    public int nChannels = 0;
    List<float> tempRecording = new List<float>();
    bool isRecordingTest = false;
    //list of recorded clips...
    List<float[]> recordedClips = new List<float[]>();
    public float[] audioData;
    public float[] outputData;
    public float[]  spectrumData;
    


    PlayType curentType{
		get{
			return MainFSM.curent;
		}
		set{
			MainFSM.ChangePlayType(value);
		}
	} 
	void Start(){
		_instance = this;
		Init();
	}
	void Init(){
		Debug.Log("Start Init");
		curentType = PlayType.Sinestetica;

		foreach(SinesteticaObject obj in sinesteticaInstances){
			obj.lineRenderer.positionCount = Settings.lenghtAudioData;
			obj.lineRenderer.enabled = false;
		}
		playableObject.lineRenderer.positionCount = Settings.lenghtAudioData;
		playableObject.playRoutine = PlayRecordRoutine();
		playableObject.audioSource.outputAudioMixerGroup = sinesteticaAudioGroups[curentAudioGroupIndex].audioGroups[playableObject.channel];
		playableObject.curent.sinesteticaMixer = sinesteticaAudioGroups[curentAudioGroupIndex];
		StartCoroutine(playableObject.playRoutine);
        GetCurentValues();
        //InitSoundRecord();

        // GetDataInfos();
    }

    private void InitSoundRecord() {
        //audioSource.clip = Microphone.Start(null, true, 1, 44100);
        float[] tempOutputData = new float[Settings.lenghtAudioData];
        //playableObject.audioSource.GetOutputData(tempOutputData, 0);
        //audioSource.Play();
        //resize our temporary vector every second
        //Invoke("ResizeRecording", 1);
    }
    
    void ResizeRecording()
    {
        if (isRecordingTest)
        {
            //add the next second of recorded audio to temp vector
            float[] clipData = new float[Settings.lenghtAudioData];
            //audioSource.clip.GetData(clipData, 0);
            clipData = playableObject.curent.audioData;

            tempRecording.AddRange(clipData);

            //Invoke("ResizeRecording", 1);
        }
    }

    private void GenerateSoundRecord() {
        
        float[] clipData = new float[Settings.lenghtAudioData];
        clipData = playableObject.curent.audioData;
        //playableObject.audioSource.GetOutputData(clipData, 0);

        //create a larger vector that will have enough space to hold our temporary
        //recording, and the last section of the current recording
        float[] fullClip = new float[clipData.Length + tempRecording.Count];
        for (int i = 0; i < fullClip.Length; i++)
        {
            //write data all recorded data to fullCLip vector
            if (i < tempRecording.Count)
                fullClip[i] = tempRecording[i];
            else
                fullClip[i] = clipData[i - tempRecording.Count];
        }

        recordedClips.Add(fullClip);
        playableObject.recordedAudio = AudioClip.Create("recorded samples", fullClip.Length, 1, Settings.lenghtAudioData, false);
        playableObject.recordedAudio.SetData(fullClip, 0);
        //audioSource.loop = true;
    }
    private void UpdateSoundRecord()
    {
        //else
        //{
            //stop audio playback and start new recording...
            //audioSource.Stop();
            tempRecording.Clear();
            //Microphone.End(null);
            //audioSource.clip = Microphone.Start(null, true, 1, 44100);
            //Invoke("ResizeRecording", 1);
        //}
    }

    private void PlayANote(SinesteticaObject obj){
		obj.helmController.NoteOn(Utils.kMiddleC, 1, 200);
	}
	private void StopANote(SinesteticaObject obj){
		obj.helmController.NoteOff(Utils.kMiddleC);
	}	
	//public void StopRecord(){
	//	sinesteticaInstances[nextChannel].sinesteticaValues = new List<SinesteticaValue>();
	//	sinesteticaInstances[nextChannel].sinesteticaValues.AddRange(playableObject.sinesteticaValues);
	//	sinesteticaInstances[nextChannel].playRoutine = RecordedRoutine(sinesteticaInstances[nextChannel]);
	//	StartCoroutine(sinesteticaInstances[nextChannel].playRoutine);
	//	nextChannel++;
	//}
	//public void RemovePrevious(){
	//	if(nextChannel == 0) return;
	//	nextChannel--;
	//	StopCoroutine(sinesteticaInstances[nextChannel].playRoutine);
	//	StopANote(sinesteticaInstances[nextChannel]);
	//	sinesteticaInstances[nextChannel].lineRenderer.enabled = false;
	//}
	private IEnumerator PlayRecordRoutine(){
		Debug.Log("Start PlayRoutine");

		while(true){
			//print(curentType);
			yield return new WaitForFixedUpdate();
			GetCurentValues();
            //hyram aqui



			//ChangeTypeOfPlaying() 
            //if(curentType == PlayType.Sinestetica){
				PlayUpdate();
				RecordUpdate();
			//}
			//if(curentType == PlayType.Sampler){
			//	SamplerUpdate();
			//}
			//if(curentType == PlayType.Mic){
			//	MicUpdate();
			//}

		}
	}

    private void ChangeTypeOfPlaying(){
		if(playableObject.curent.values.inputs.primaryBGrip && !isChangeType){
			isChangeType = true;
			if(curentType==PlayType.Sinestetica){
				StartSampler();
				//curentType = PlayType.Sampler;
			}
			else{
				curentSampler.audioSource.Stop();
				curentSampler.lineRenderer.enabled=false;
				curentType = PlayType.Sinestetica;
			}

			// if(curentType==PlayType.Sinestetica){
			// 	StartMic();
			// 	curentType = PlayType.Mic;
			// }
			// else{
			// 	StopMic();
			// 	curentType = PlayType.Sinestetica;
			// }
		}
		if(!playableObject.curent.values.inputs.primaryBGrip && isChangeType){
			isChangeType = false;
		}
	}

    private void PlayUpdate()
	{
		UpdateHandRenderer(playableObject);
		if(playableObject.curent.values.inputs.primaryBTrackPad && !isChangeAudioMixer){
			isChangeAudioMixer = true;
			//next
			if(playableObject.curent.values.inputs.primaryTrackPad.x >= 0){
				if(curentAudioGroupIndex == sinesteticaAudioGroups.Length-1){
					curentAudioGroupIndex = 0;
				}
				else{
					curentAudioGroupIndex++;
				}
			}//previous
			else{
				if(curentAudioGroupIndex == 0){
					curentAudioGroupIndex = sinesteticaAudioGroups.Length-1;
				}
				else{
					curentAudioGroupIndex--;
				}
			}
			playableObject.audioSource.outputAudioMixerGroup = sinesteticaAudioGroups[curentAudioGroupIndex].audioGroups[playableObject.channel];
			playableObject.curent.sinesteticaMixer = sinesteticaAudioGroups[curentAudioGroupIndex];
		}
		if(!playableObject.curent.values.inputs.primaryBTrackPad && isChangeAudioMixer){
			isChangeAudioMixer = false;
		}

		if(playableObject.curent.values.inputs.primaryBTrigger && !isPlaying){
			playableObject.lineRenderer.enabled = true;
			isPlaying = true;
            InputManager.Instance.rightPulse = true;
            InputManager.Instance.leftPulse = true;
            PlayANote(playableObject);
		}
		if(!playableObject.curent.values.inputs.primaryBTrigger && isPlaying){
			playableObject.lineRenderer.enabled = false;
			isPlaying = false;
            InputManager.Instance.rightPulse = false;
            InputManager.Instance.leftPulse = false;
            StopANote(playableObject);
		}
		if(playableObject.curent.values.inputs.primaryBTrigger){


            InputManager.Instance.rightForcePulse = (ushort)(Settings.instance.minForce + (playableObject.curent.synthValues.primaryPose.position.y * Settings.instance.maxForce));
            InputManager.Instance.leftForcePulse = (ushort)(Settings.instance.minForce + (playableObject.curent.synthValues.secondaryPose.position.y * Settings.instance.maxForce));

            UpdateValues(playableObject);
			GetHelmData();
			GetOscilloscopePositions();
			UpdateOscilloscopeRenderer(playableObject);
			}		
	}

    //private void RecordUpdate()
    //{
    //    if (nextChannel == 15)
    //    {
    //        if (playableObject.curent.values.inputs.secondaryBTrigger && !isRecording)
    //            LimitException();
    //        return;
    //    }

    //    if (playableObject.curent.values.inputs.secondaryBTrigger && !isRecording)
    //    {
    //        isRecording = true;
    //        playableObject.sinesteticaValues = new List<SinesteticaValue>();
    //    }
    //    if (!playableObject.curent.values.inputs.secondaryBTrigger && isRecording)
    //    {
    //        isRecording = false;
    //        StopRecord();
    //        return;
    //    }
    //    if (playableObject.curent.values.inputs.secondaryBTrigger)
    //    {
    //        playableObject.sinesteticaValues.Add(new SinesteticaValue(playableObject.curent));
    //    }
    //    if (playableObject.curent.values.inputs.secondaryBTrackPad && !isRemoving)
    //    {
    //        isRemoving = true;
    //        RemovePrevious();
    //    }
    //    if (!playableObject.curent.values.inputs.secondaryBTrackPad && isRemoving)
    //    {
    //        isRemoving = false;
    //    }
    //}

    private void RecordUpdate()
    {

        if (playableObject.curent.values.inputs.secondaryBTrigger && !isRecording)
        {
            isRecording = true;
            isRecordingTest = true;
            TestRare.Instance.StartRecord();
            //UpdateSoundRecord();
            //playableObject.sinesteticaValues = new List<SinesteticaValue>();
        }
        if (!playableObject.curent.values.inputs.secondaryBTrigger && isRecording)
        {
            isRecording = false;
            isRecordingTest = false;
            TestRare.Instance.StopRecord();
            //GenerateSoundRecord();
            //StopRecord();
            return;
        }
        if (playableObject.curent.values.inputs.secondaryBTrigger)
        {
            //playableObject.sinesteticaValues.Add(new SinesteticaValue(playableObject.curent));
        }
        if (playableObject.curent.values.inputs.secondaryBTrackPad && !isRemoving)
        {
            isRemoving = true;
            //RemovePrevious();
        }
        if (!playableObject.curent.values.inputs.secondaryBTrackPad && isRemoving)
        {
            isRemoving = false;
        }
    }



    private void StartSampler(){
		curentSampler = Instantiate(samplerPrefab).GetComponent<SinesteticaObject>();
		samplerObjects.Add(curentSampler);
		curentSampler.audioSource.clip = samples[nextSample];
		curentSampler.lineRenderer.enabled = false;
	}

	private void StartMic(){
		curentMic = Instantiate(micPrefab).GetComponent<SinesteticaObject>();
	}
	private void StopMic(){
		Destroy(curentMic);
	}

	private void SamplerUpdate(){
		curentSampler.curent = playableObject.curent;
		if(playableObject.curent.values.inputs.primaryBTrackPad && !isChangeAudioMixer){
			isChangeAudioMixer = true;
			//next
			if(playableObject.curent.values.inputs.primaryTrackPad.x >= 0){
				if(nextSample == samples.Length-1){
					nextSample = 0;
				}
				else{
					nextSample++;
				}
			}//previous
			else{
				if(nextSample == 0){
					nextSample = samples.Length-1;
				}
				else{
					nextSample--;
				}
			}
			curentSampler.audioSource.clip = samples[nextSample];
			curentSampler.oscilloscopeRef.Start();
		}
		if(!playableObject.curent.values.inputs.primaryBTrackPad && isChangeAudioMixer){
			isChangeAudioMixer = false;
		}

		if(playableObject.curent.values.inputs.primaryBTrigger && !isPlaying){
			curentSampler.lineRenderer.enabled = true;
			isPlaying = true;
			curentSampler.audioSource.clip.LoadAudioData();
			curentSampler.audioSource.Play();

		}
		if(!playableObject.curent.values.inputs.primaryBTrigger && isPlaying){
			curentSampler.lineRenderer.enabled = false;
			isPlaying = false;
			curentSampler.audioSource.clip.UnloadAudioData();
			curentSampler.audioSource.Stop();
		}
		if(playableObject.curent.values.inputs.primaryBTrigger){	
				SamplerUpdateOscilloscopeRenderer(curentSampler);
			}		
	}

	private void MicUpdate(){
		curentMic.curent = playableObject.curent;
		SamplerUpdateOscilloscopeRenderer(curentMic);
	}
	private void RecordSample(){

	}

	//private IEnumerator RecordedRoutine(SinesteticaObject obj){
	//	bool isPlaying = false;
	//	string audioGroupName = "";
	//	while(true){
	//		foreach(SinesteticaValue value in obj.sinesteticaValues){
	//			yield return new WaitForFixedUpdate();
	//			obj.curent = value;
				
	//			if(obj.curent.sinesteticaMixer.name != audioGroupName){
	//				audioGroupName = obj.curent.sinesteticaMixer.name;
	//				obj.audioSource.outputAudioMixerGroup = obj.curent.sinesteticaMixer.audioGroups[obj.channel];			 	
	//			}
				
	//			if(obj.curent.values.inputs.primaryBTrigger && !isPlaying){
	//				obj.lineRenderer.enabled = true;
	//				isPlaying = true;
	//				PlayANote(obj);
	//		 	}
	//			if(!obj.curent.values.inputs.primaryBTrigger && isPlaying){
	//				obj.lineRenderer.enabled = false;
	//				isPlaying = false;
	//				StopANote(obj);
	//			}
	//			if(obj.curent.values.inputs.primaryBTrigger){
	//				UpdateValues(obj);
	//				UpdateOscilloscopeRenderer(obj);
	//			}
	//		}
	//		yield return new WaitForFixedUpdate();
	//	}
	//}
	public void LimitException(){
		Debug.LogError("Limite de canais do Helm atingido");
	}
	public void UpdateOscilloscopeRenderer(SinesteticaObject obj){
		obj.lineRenderer.SetPositions(obj.curent.oscilloScopePositions);
		SetOscilloscopePose(obj);
		OscilloscopeManager.SetOscilloScaler(obj.OscilloscopeTransform,obj.lineRenderer,obj.curent.synthValues);
		// OscilloscopeManager.SetColor(obj.lineRenderer, obj.curent.synthValues.yMedia);
		obj.lineRenderer.SetColors(obj.curent.rightColor,obj.curent.leftColor);

	}
	
	public void UpdateHandRenderer(SinesteticaObject obj){
		NoteTuner left,right;

		right = HelmTunerManager.GetNoteByValue(obj.curent.synthValues.primaryPose.position.y);
		left = HelmTunerManager.GetNoteByValue(obj.curent.synthValues.secondaryPose.position.y);

		string rightNote = HelmTunerManager.NoteNames[(int)right.note];
		string leftNote = HelmTunerManager.NoteNames[(int)left.note];
		
		if(right.microtonicValue >= right.noteValue && right.microtonicValue <= right.noteValue+0.003f)
		{
			RightHand.textHand.text = rightNote;
			//InputManager.Instance.TriggerHaptic(HandType.Primary,500);
			RightHand.textHand.color = right.microtonicColor; 
		}
		else
		{
			RightHand.textHand.text = "";
			RightHand.textHand.color = Color.white; 
		}

		if(left.microtonicValue >= left.noteValue && left.microtonicValue <= left.noteValue+0.003f)
		{
			LeftHand.textHand.text = leftNote;
			//InputManager.Instance.TriggerHaptic(HandType.Secondary,500);
			LeftHand.textHand.color = left.microtonicColor; 
		}
		else
		{
			LeftHand.textHand.text = "";
			LeftHand.textHand.color = Color.white; 
		}
		// float lh,ls,lv,rh,rs,rv;

		// Color.RGBToHSV(rightColor,out rh,out rs,out rv);
		// Color.RGBToHSV(leftColor,out lh,out ls,out lv);
		
		// rv = rv * (right.octave/7);
		// lv = lv * (left.octave/7);
		
		// rightColor = Color.HSVToRGB(rh,rs,rv);
		// leftColor = Color.HSVToRGB(lh,ls,lv);

		RightHand.material.color = right.microtonicColor;
		LeftHand.material.color = left.microtonicColor;

		obj.curent.rightColor = right.microtonicColor;
		obj.curent.leftColor = left.microtonicColor;
		
	}

	public void SamplerUpdateOscilloscopeRenderer(SinesteticaObject obj){
		//obj.lineRenderer.SetPositions(obj.curent.oscilloScopePositions);
		SetOscilloscopePose(obj);
		OscilloscopeManager.SetOscilloScaler(obj.OscilloscopeTransform,obj.lineRenderer,obj.curent.synthValues);
		// OscilloscopeManager.SetColor(obj.lineRenderer, obj.curent.synthValues.yMedia);
		obj.lineRenderer.SetColors(obj.curent.rightColor,obj.curent.leftColor);
	}

	public void SetOscilloscopePose(SinesteticaObject obj){
		obj.OscilloscopeTransform.position = obj.curent.values.primaryPose.position;
		obj.OscilloscopeTransform.eulerAngles = obj.curent.values.primaryPose.eulerRotation;
	}
	public void GetCurentValues(){
		playableObject.curent.values = InputManager.Instance.curent;
	}
	public void GetHelmData(){
		Native.HelmGetBufferData(Settings.playableChannel,playableObject.curent.audioData, playableObject.curent.audioData.Length, 2);
        //audioData = playableObject.curent.audioData;
    }


    public void GetDataInfos() {
        //spectrumData = new float[dataLenght];
        outputData = new float[dataLenght];
        //playableObject.audioSource.GetSpectrumData(spectrumData, nChannels, FFTWindow.Rectangular);
        playableObject.audioSource.GetOutputData(outputData, nChannels);

    }

    public void GetOscilloscopePositions(){
		playableObject.curent.oscilloScopePositions = OscilloscopeManager.LineOscillation(playableObject.curent.audioData);
	}
	// public void GetSamplerOscilloscopePositions(){

	// }
	public void UpdateValues(SinesteticaObject obj){
		try{
			obj.helmController.SetParameterAtIndex((int)ParamIndex.xPositionMainHand, obj.curent.synthValues.primaryPose.position.x);
			obj.helmController.SetParameterAtIndex((int)ParamIndex.yPositionMainHand, obj.curent.synthValues.primaryPose.position.y);
			obj.helmController.SetParameterAtIndex((int)ParamIndex.zPositionMainHand, obj.curent.synthValues.primaryPose.position.z);
			obj.helmController.SetParameterAtIndex((int)ParamIndex.xRotationMainHand, obj.curent.synthValues.primaryPose.eulerRotation.x);
			obj.helmController.SetParameterAtIndex((int)ParamIndex.yRotationMainHand, obj.curent.synthValues.primaryPose.eulerRotation.y);
			obj.helmController.SetParameterAtIndex((int)ParamIndex.zRotationMainHand, obj.curent.synthValues.primaryPose.eulerRotation.z);
			obj.helmController.SetParameterAtIndex((int)ParamIndex.xPositionSecondHand, obj.curent.synthValues.secondaryPose.position.x);
			obj.helmController.SetParameterAtIndex((int)ParamIndex.yPositionSecondHand, obj.curent.synthValues.secondaryPose.position.y);
			obj.helmController.SetParameterAtIndex((int)ParamIndex.zPositionSecondHand, obj.curent.synthValues.secondaryPose.position.z);
			obj.helmController.SetParameterAtIndex((int)ParamIndex.xRotationSecondHand, obj.curent.synthValues.secondaryPose.eulerRotation.x);
			obj.helmController.SetParameterAtIndex((int)ParamIndex.yRotationSecondHand, obj.curent.synthValues.secondaryPose.eulerRotation.y);
			obj.helmController.SetParameterAtIndex((int)ParamIndex.zRotationSecondHand, obj.curent.synthValues.secondaryPose.eulerRotation.z);   
			obj.helmController.SetParameterAtIndex((int)ParamIndex.pressureTrigger, obj.curent.synthValues.inputs.primaryTrigger);
		}
		catch(System.Exception e){
			Debug.LogError(e.Message);
		}
	}

	public void ChangePlayableAudioMixer(){
		
	}
	void OnApplicationQuit(){
		StopCoroutine(playableObject.playRoutine);
		StopAllCoroutines();
		foreach(SinesteticaObject obj in sinesteticaInstances){
			if(obj.playRoutine != null) StopCoroutine(obj.playRoutine);
			Destroy(obj);
		}
		sinesteticaInstances = null;
		Destroy(playableObject);
		playableObject = null;
	}
}
public enum ParamIndex
{
	xPositionMainHand = 0,
	yPositionMainHand = 1,
	zPositionMainHand = 2,
	xRotationMainHand = 3,
	yRotationMainHand = 4,
	zRotationMainHand = 5,
	xPositionSecondHand = 6,
	yPositionSecondHand = 7,
	zPositionSecondHand = 8,
	xRotationSecondHand = 9,
	yRotationSecondHand = 10,
	zRotationSecondHand = 11,
	pressureTrigger = 12
}
