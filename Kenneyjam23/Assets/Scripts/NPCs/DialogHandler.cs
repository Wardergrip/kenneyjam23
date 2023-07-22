using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Events;

public class DialogHandler : MonoBehaviour
{
    private enum ScaleResult
    {
        Hide,
        Show
    }

    [Header("Dialog")]
    [SerializeField] private DialogBlock _rootDialogBlock;
    private DialogBlock _currentDialogBlock;

    [Header("UI")]
    [SerializeField] private float _scaleTime = 1f;
    [SerializeField] private RectTransform _panelTransform;
    private Vector3 _originalPanelScale;
    [SerializeField] private TextMeshProUGUI _npcDialogTextBlock;
    [Tooltip("It is assumed the child has a TextMeshProUGUI component.")]
    [SerializeField] private Button _dialogRespondPrefab;
    [SerializeField] private float _ySpaceBetweenButtons = 10f;
    private List<Button> _respondButtons;

    void Start()
    {
        _dialogRespondPrefab.gameObject.SetActive(false);
        Debug.Assert(_rootDialogBlock, $"Root dialog block is null.");
        _originalPanelScale = _panelTransform.localScale;
        _currentDialogBlock = _rootDialogBlock;
    }

    public void OpenDialog()
    {
        StopAllCoroutines();
        UpdatePanel();

        StartCoroutine(Scale_Coroutine(ScaleResult.Show,_scaleTime));
    }

    public void CloseDialog()
    {
        StopAllCoroutines();
        StartCoroutine(Scale_Coroutine(ScaleResult.Hide,_scaleTime));
    }

    private void UpdatePanel()
    {
        // Remove old respond buttons
        _respondButtons.ForEach(b => Destroy(b.gameObject));
        _respondButtons.Clear();

        // If there are no choices, we've hit the end of the dialog chain.
        if (_currentDialogBlock.Choices.Count <= 0)
        {
            CloseDialog();
            return;
        }

        // Set true so that if we copy it it is copied as active
        _dialogRespondPrefab.gameObject.SetActive(true);
        // Loop over all choices
        for (int i = 0; i < _currentDialogBlock.Choices.Count; ++i)
        {
            // Add a new instance of it to the buttons list
            _respondButtons.Add(Instantiate(_dialogRespondPrefab.gameObject).GetComponent<Button>());
            var button = _respondButtons.Last();
            // Set position
            Vector3 pos = button.transform.position;
            pos.y += _ySpaceBetweenButtons * i;
            button.transform.position = pos;

            // Define action for onClick
            UnityAction action = () => 
            {
                _currentDialogBlock = _currentDialogBlock.Choices[i];
                UpdatePanel();
            };
            button.onClick.AddListener(action);

            // Set text
            button.GetComponentInChildren<TextMeshProUGUI>().text = _currentDialogBlock.Choices[i].ChoiceText;
        }
        // Disable the thing we coppying from
        _dialogRespondPrefab.gameObject.SetActive(false);

        // Set dialog text block
        _npcDialogTextBlock.text = _currentDialogBlock.NpcSpeech;
    }

    private IEnumerator Scale_Coroutine(ScaleResult scaleResult, float fadeDuration = 1f)
    {
        float elapsedTime = 0f;

        // If the result is show, we go from zero --> original. If not, we do the opposite.
        Func<float, int> scalingFunc = scaleResult == ScaleResult.Show 
            ? (float t) => 
        {
            _panelTransform.localScale = Vector3.Lerp(Vector3.zero, _originalPanelScale, t);
            return 0; 
        } 
            : (float t) =>
        {
            _panelTransform.localScale = Vector3.Lerp(_originalPanelScale, Vector3.zero, t);
            return 0;
        }
        ;

        // Main loop
        while (elapsedTime < fadeDuration)
        {
            float t = EaseInOut(elapsedTime / fadeDuration);

            scalingFunc(t);

            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        switch (scaleResult)
        {
            case ScaleResult.Hide:
                _panelTransform.localScale = Vector3.zero;
                _panelTransform.gameObject.SetActive(false);
                break;
            case ScaleResult.Show:
                _panelTransform.localScale = _originalPanelScale;
                _panelTransform.gameObject.SetActive(true);
                break;
            default:
                Debug.LogError("How did we get here? Error from DialogHandler.");
                break;
        }
    }

    private float EaseInOut(float t)
    {
        return t < 0.5f ? 4f * t * t * t : (t - 1f) * (2f * t - 2f) * (2f * t - 2f) + 1f;
    }
}
