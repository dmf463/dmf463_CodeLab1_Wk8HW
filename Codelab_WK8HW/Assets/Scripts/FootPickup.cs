using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootPickup : MonoBehaviour {

    bool glued;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Glue")
        {
            
        }
    }
}
