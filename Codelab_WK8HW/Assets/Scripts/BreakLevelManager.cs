using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakLevelManager : MonoBehaviour {

    int _statuesBroken;
    public int statuesBroken
    {
        get { return _statuesBroken; }
        set
        {
            _statuesBroken = value;

            Debug.Log(_statuesBroken);

            if (statuesBroken >= 6)
            {
                GameObject.Find("Up Elevator (Conditional Trigger)").GetComponent<ElevatorScript>().OpenDoor();
            }

        }
    }
}
