using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SteamVR namespace
using Valve.VR;


public class BasicPickup : MonoBehaviour {

	// A class designed by Valve to keep track of controller data (left/right, on/off, etc.)
	public SteamVR_ControllerManager cm;	// Assign in inspector

	Rigidbody currentlyHeld;

	// Property to hold the currently used controller
	SteamVR_Controller.Device myDevice {
		get { 
			return SteamVR_Controller.Input ((int) GetComponent<SteamVR_TrackedObject> ().index);
		}
	}


	// 1. DETECT IF A PICKUPABLE OBJECT IS WITHIN OUR TRIGGER
	void OnTriggerStay(Collider other) {
		// 2. DETECT IF WE ARE HOLDING DOWN THE CONTROLLER TRIGGER	
		if (myDevice.GetHairTrigger ())
		{
			// 3. TURN OFF PHYSICS SIMULATION FOR THE THING WE'RE PICKING UP
			currentlyHeld = other.GetComponent<Rigidbody>();
			currentlyHeld.isKinematic = true;

			// 4. PARENT THE OBJECT TO THE CONTROLLER.
			currentlyHeld.transform.SetParent(transform);
		}
	}


	void Update()
	{
		// 5. DROP A CURRENTLY HELD OBJECT IF WE RELEASE THE TRIGGER
		if (currentlyHeld != null && !myDevice.GetHairTrigger()) {
			currentlyHeld.isKinematic = false;
			currentlyHeld.transform.parent = null;
			currentlyHeld = null;
		}
	}

}
