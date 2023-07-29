using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScoreDisplayer : MonoBehaviour
{
    static public string TimeText { get; set; }

    [SerializeField] private TextMeshProUGUI _textMeshProText;
    [SerializeField] private string _prefix;
    [SerializeField] private string _postfix;

    void Start()
    {
        _textMeshProText.text = $"{_prefix}{TimeText}{_postfix}";
    }
}
