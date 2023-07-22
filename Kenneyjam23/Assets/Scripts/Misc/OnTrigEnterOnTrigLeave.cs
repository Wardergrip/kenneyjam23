using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTrigEnterOnTrigLeave : MonoBehaviour
{
    //[Header("Events")]
    //public UnityEvent OnEnterEvent;
    //public UnityEvent OnExitEvent;

    private void OnTriggerEnter(Collider other)
    {
        //OnEnterEvent?.Invoke();
        GameTimer.Pause();
    }

    private void OnTriggerExit(Collider other)
    {
        //OnExitEvent?.Invoke();
        GameTimer.Unpause();
    }
}
