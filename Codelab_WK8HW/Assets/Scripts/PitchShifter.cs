using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchShifter : MonoBehaviour {

    AudioSource myAudio;

    public float shiftSpeed = 0.1f;
    public float shiftRange = 0.4f;
    float originalPitch;

    float noiseTime = 0.0f;

	void Start () {
        myAudio = GetComponent<AudioSource>();
        originalPitch = myAudio.pitch;
	}
	
	void Update () {
        float newPitch = MyMath.Map(Mathf.PerlinNoise(noiseTime, 0f), 0f, 1f, originalPitch-shiftRange, originalPitch+shiftRange);
        myAudio.pitch = newPitch;
        noiseTime += shiftSpeed;
	}
}
