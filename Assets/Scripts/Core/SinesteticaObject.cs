using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioHelm;
public class SinesteticaObject : MonoBehaviour {
	public System.Guid id = new System.Guid();
	public Transform OscilloscopeTransform;
	//0 to 15
	public int channel;
	public HelmController helmController;
	public AudioSource audioSource;
	public LineRenderer lineRenderer;
	public bool isPlaying = false;
	public SinesteticaValue curent = new SinesteticaValue();
	public List<SinesteticaValue> sinesteticaValues = new List<SinesteticaValue>();
	public IEnumerator playRoutine;

	//Sampler -- Transform into a SamplerObject : SinesteticaObject
    public Oscilloscope oscilloscopeRef;
    //Record Audio
    public AudioClip recordedAudio;
}
