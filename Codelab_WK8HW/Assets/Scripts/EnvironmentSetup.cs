using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class EnvironmentSetup : MonoBehaviour {

	bool environmentSetupComplete;

	public Transform floor;

	SteamVR_PlayArea playArea;

	Vector3 playAreaSize;

	void Start()
	{
		playArea = GameObject.FindObjectOfType<SteamVR_PlayArea> ();
	}

	void Update ()
	{
		if (!environmentSetupComplete)
		{
			SetupEnvironment ();
		}
	}

	void SetupEnvironment()
	{
		// Set the floor's scale
		HmdQuad_t quad = new HmdQuad_t ();
		if (SteamVR_PlayArea.GetBounds(playArea.size, ref quad))
		{
			Vector3 floorScale = new Vector3 (
				Mathf.Abs (quad.vCorners0.v0 - quad.vCorners2.v0),
				Mathf.Abs (quad.vCorners0.v2 - quad.vCorners2.v2),
			 	transform.localScale.y
			);

			floor.localScale = floorScale;

			// Save the bounds of the room.
			playAreaSize.x = floor.localScale.x;
			playAreaSize.y = 1f;
			playAreaSize.z = floor.localScale.y;
		}
	}
}
