using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainFSM {
	public static PlayType curent;
	public static void ChangePlayType(PlayType newPlayType){
		curent = newPlayType;
	}
}

public enum PlayType{
	Sinestetica,
	Sampler,
	Mic
}
