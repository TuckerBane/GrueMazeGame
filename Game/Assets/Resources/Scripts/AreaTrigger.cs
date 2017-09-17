using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter Area");
        EnterArea ea;
        ea.area = transform;
        FFMessage<EnterArea>.SendToLocal(ea);
    }
    private void OnTriggerExit(Collider other)
    {
        LeaveArea la;
        la.area = transform;
        FFMessage<LeaveArea>.SendToLocal(la);
    }
}
