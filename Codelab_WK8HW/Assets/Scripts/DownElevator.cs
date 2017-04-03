using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownElevator : MonoBehaviour {

	public Transform door;
	float doorOpenHeight = 4.5f;
	bool doorOpen;
	
	void Update ()
	{
		if (!doorOpen)
		{
			door.Translate(0f, 0.4f * Time.deltaTime, 0f);
			if (door.localPosition.y >= doorOpenHeight)
			{
				doorOpen = true;
			}
		}
	}
}
