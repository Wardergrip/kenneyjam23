using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;

    private static bool IsPaused { get; set; } = false;

    public static void Pause() => IsPaused = true;
    public static void Unpause() => IsPaused = false;

    private float _time = 0;
    private int _seconds = 0;
    private int _minutes = 0;
    private int _hours = 0; 

    public int Seconds { get { return _seconds; } }
    public int Minutes { get { return _minutes; } }
    public int Hours { get { return _hours; } }


    // Update is called once per frame
    void Update()
    {
        if (IsPaused) return;

        _time += Time.deltaTime;
        if (_time >= 1f)
        {
            _time = 0f;
            AddSecond();
        }
    }

    private void AddSecond()
    {
        ++_seconds;
        if (_seconds >= 60)
        {
            _seconds = 0;
            ++_minutes;
            if (_minutes >= 60)
            {
                _minutes = 0;
                ++_hours;
            }
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        textMeshProUGUI.text = 
            string.Concat(
                _hours.ToString(), 
                ":",
                _minutes > 9 ? "" : "0", 
                _minutes.ToString(), 
                ":",
                _seconds > 9 ? "" : "0",
                _seconds.ToString()
                );
    }
}
