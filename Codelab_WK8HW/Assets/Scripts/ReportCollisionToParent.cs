using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReportCollisionToParent : MonoBehaviour {

    public Transform parentToReportTo;

    void OnCollisionEnter(Collision collision)
    {
        parentToReportTo.SendMessage("OnCollisionEnterChild", collision);
    }
}
