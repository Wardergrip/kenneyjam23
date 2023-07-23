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
        var player = other.GetComponent<PlayerController>();
        if (player == null) return;
        GameTimer.Pause();
    }

    private void OnTriggerExit(Collider other)
    {
        //OnExitEvent?.Invoke();
        var player = other.GetComponent<PlayerController>();
        if (player == null) return;
        GameTimer.Unpause();
    }
}
