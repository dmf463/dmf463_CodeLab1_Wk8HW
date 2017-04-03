using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;


// Put this script on the thing that can be picked up
public class InteractionPickup : MonoBehaviour {

	Rigidbody rb;

	// Used for tracking hand velocity
	Vector3 handPrevPos;
	Vector3 handPos;
	Vector3 handVel;

	void Start() {
		rb = GetComponent<Rigidbody> ();
        
		handPrevPos = Vector3.zero;
		handVel = Vector3.zero;
	}

	// These functions will only get called if this object has an 'interactable' component on it.

	// A 'Hand' is an abstraction of a hand, could be a controller, mouse, etc.
	// This happens every frame that a hand is near the object
	void HandHoverUpdate(Hand hand)
	{
		// Trigger on Vive, Left button on mouse, etc.
		if (hand.GetStandardInteractionButton ()) {
			hand.AttachObject (gameObject);
		}
	}


	// This happens the first frame that this object is attached to a hand
	void OnAttachedToHand() {
		rb.isKinematic = true;
	}


	// This is like "Update" as long as we're holding something
	void HandAttachedUpdate(Hand hand) {

		// Track hand position
		handPos = hand.transform.position;

		if (!hand.GetStandardInteractionButton ()) {
			hand.DetachObject (gameObject);
		}
	}


	void FixedUpdate() {
		// Start tracking hand velocity
		handVel = handPos - handPrevPos;

		handPrevPos = handPos;
	}


	void OnDetachedFromHand(Hand hand) {
		rb.isKinematic = false;

		// Apply forces to the detached object as if we are throwing it
//		if () {
//			rb.AddForce(hand.GetTrackedObjectVelocity()*10f, ForceMode.Impulse);
//		} else {
			rb.velocity = handVel * 10f;
//		}
		rb.AddTorque (hand.GetTrackedObjectAngularVelocity () * 10f, ForceMode.Impulse);
	}
}
