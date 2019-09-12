using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmTunerManager : MonoBehaviour {
	private const float noteDifference = 0.010445f; 
	private const float A1 = 0.0917f;
	private const float G3 = 0.446f;
	private const float G5 = 0.696f;
	private const float G6 = 0.821f;
	private const long floatProblemAdd = 1000000000000;
	private static List<NoteTuner> tunerList;
	public static readonly string[] NoteNames = new string[]{"A","Bb","B","C","C#","D","Eb","E","F","F#","G","G#"};

	//public float valueTest;
	// void Update(){
	// 	NoteTuner note = GetNoteByValue(valueTest);
	// 	print("Note:"+note.note.ToString()+note.octave+" Value: "+note.microtonicValue);
	// }
	void Start(){
		GenerateNoteTunerList();
		// int i =0;
		// foreach(NoteTuner note in tunerList){
		// 	print(i+" Note:"+note.note.ToString()+note.octave+" Value: "+note.microtonicValue);
		// 	i++;
		// }
	}

	public static NoteTuner GetNoteByValue(float value){
		var note = tunerList.Find(n => n.noteValue<=value && value < n.noteValue+noteDifference);
		var heightDifference = (value-note.noteValue)/noteDifference;
		Color noteColor,nextNoteColor;
		noteColor = GetColorByNote(note.note);
		if(note.note == (NoteIndex)11)
		{
			nextNoteColor = GetColorByNote((NoteIndex)0);
		}else{
			nextNoteColor = GetColorByNote((NoteIndex)(note.note+1));
		}
		note.microtonicColor = Color.Lerp(noteColor,nextNoteColor,heightDifference);
		if(Settings.instance.ValueOctaveInterpolate){
			float h,s,v;
			Color.RGBToHSV(note.microtonicColor,out h,out s,out v);
			var vInterpolate = v * (((float)note.octave)/7f);
			v = vInterpolate * v * Settings.instance.ValueInterpolateFilter;
			note.microtonicColor = Color.HSVToRGB(h,s,v);
		}
		note.microtonicValue = value;
		return note;
	}
	// public NoteTuner GetNoteByTuner(float value){
	// 	if(tunerList == null) GenerateNoteTunerList();
	// 	int i =0;
	// 	foreach(NoteTuner note in tunerList){
	// 		if(note.octave<= value && value < note.octave + noteDifference){
	// 			print(note.note.ToString());
	// 			print(note.microtonicValue);
	// 			print(note.octave);
	// 			return note;
	// 			break;
	// 		}
	// 		i++;
	// 	}
	// 	return new NoteTuner(); 
	// }
	private void GenerateNoteTunerList(){
		tunerList = new List<NoteTuner>();
		//octave loop
		int k = 0;
		for(int i = 1;i<=7;i++)
		{
			//notes loop
			for(int j= 0;j<12;j++){
				decimal curentNote = (decimal)0.010445f*k+(decimal)0.0917f;
				var tempNote =
				new NoteTuner{
					note = (NoteIndex)j,
					octave = i,
					noteValue = (float)curentNote 
				};
				tunerList.Add(tempNote);
				k++;
			}
		}
	}

	
	public static Color GetColorByNote(NoteIndex note){
		
		switch (note)
		{
			case NoteIndex.A:
			return Settings.instance.A;
			case NoteIndex.Asharp:
			return Settings.instance.Asharp;
			case NoteIndex.B:
			return Settings.instance.B;
			case NoteIndex.C:
			return Settings.instance.C;
			case NoteIndex.Csharp:
			return Settings.instance.Csharp;
			case NoteIndex.D:
			return Settings.instance.D;
			case NoteIndex.Dsharp:
			return Settings.instance.Dsharp;
			case NoteIndex.E:
			return Settings.instance.E;
			case NoteIndex.F:
			return Settings.instance.F;
			case NoteIndex.Fsharp:
			return Settings.instance.Fsharp;
			case NoteIndex.G:
			return Settings.instance.G;
			case NoteIndex.Gsharp:
			return Settings.instance.Gsharp;
		}

		return Color.white;
	}
}

public struct NoteTuner {
	public NoteIndex note;
	public int octave;
	public float noteValue;
	public float microtonicValue;
	public Color microtonicColor;
}
public enum NoteIndex{
	A = 0,
	Asharp = 1,
	B = 2,
	C = 3,
	Csharp = 4,
	D = 5,
	Dsharp = 6,
	E = 7,
	F = 8,
	Fsharp = 9,
	G = 10,
	Gsharp = 11
}