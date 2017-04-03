using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class NativeVRController : MonoBehaviour {

	[SerializeField] Transform leftHand, rightHand;
	
	void Update () {

		// Get controller position & rotations.
		// GetLocalPosition gets the local position of a certain 'VR Node'
		leftHand.localPosition = InputTracking.GetLocalPosition(VRNode.LeftHand);
		leftHand.localRotation = InputTracking.GetLocalRotation (VRNode.LeftHand);

		rightHand.localPosition = InputTracking.GetLocalPosition (VRNode.RightHand);
		rightHand.localRotation = InputTracking.GetLocalRotation (VRNode.RightHand);
	}
}
