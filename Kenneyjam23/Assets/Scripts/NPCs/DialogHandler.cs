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
    [SerializeField] private RectTransform _buttonSocket;
    [SerializeField] private GameObject _dialogRespondPrefab;
    [SerializeField] private float _ySpaceBetweenButtons = 10f;
    private List<Button> _respondButtons = new List<Button>();

    void Start()
    {
        Debug.Assert(_rootDialogBlock, $"Root dialog block is null.");

        // Cache the original scale of the dialog window
        _originalPanelScale = _panelTransform.localScale;

        // Store the first dialog
        _currentDialogBlock = _rootDialogBlock;
        _currentDialogBlock.OnActivate.Invoke();

        // Start opening the dialog window
        OpenDialog();

        // Make the dialog window dissapear
        _panelTransform.localScale = Vector3.zero;
    }

    public void OpenDialog()
    {
        StopAllCoroutines();

        // Update the text and buttons witht he current dialog
        UpdatePanel();

        // Start a coroutine that will scale the dialog to the full scale
        StartCoroutine(ScaleCoroutine(ScaleResult.Show,_scaleTime));
    }

    public void CloseDialog()
    {
        StopAllCoroutines();

        // Start a coroutine that will scale the dialog to 0
        StartCoroutine(ScaleCoroutine(ScaleResult.Hide,_scaleTime));
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

        // Loop over all choices
        for (int i = 0; i < _currentDialogBlock.Choices.Count; ++i)
        {
            // Add a new instance of it to the buttons list
            var button = Instantiate(_dialogRespondPrefab, _panelTransform);
            _respondButtons.Add(button.GetComponent<Button>());
            // Set position
            RectTransform buttonTransform = button.GetComponent<RectTransform>();
            buttonTransform.position = _buttonSocket.position - Vector3.up * _ySpaceBetweenButtons * i;

            // Define action for onClick
            int choiceIdx = i;
            UnityAction action = () => 
            {
                // Retrieve the current dialog block
                DialogBlock block = _currentDialogBlock.Choices[choiceIdx];

                // Cancel the button click when the predicate in the dialog returns false
                if (block.Predicate != null && !block.Predicate.Validate()) return;

                // Apply the new dialog block
                _currentDialogBlock = _currentDialogBlock.Choices[choiceIdx];
                _currentDialogBlock.OnActivate.Invoke();

                UpdatePanel();
            };
            button.GetComponent<Button>().onClick.AddListener(action);

            // Set text
            button.GetComponentInChildren<TextMeshProUGUI>().text = _currentDialogBlock.Choices[i].ChoiceText;
        }

        // Set dialog text block
        _npcDialogTextBlock.text = _currentDialogBlock.NpcSpeech;
    }

    private IEnumerator ScaleCoroutine(ScaleResult scaleResult, float fadeDuration = 1f)
    {
        float elapsedTime = 0f;

        // If the result is show, we go from zero --> original. If not, we do the opposite.
        Action<float> scalingFunc = scaleResult == ScaleResult.Show 
            ? (float t) => 
        {
            _panelTransform.localScale = Vector3.Lerp(Vector3.zero, _originalPanelScale, t);
        } 
            : (float t) =>
        {
            _panelTransform.localScale = Vector3.Lerp(_originalPanelScale, Vector3.zero, t);
        }
        ;

        // Main loop : scale the dialog depending on the set lambda
        while (elapsedTime < fadeDuration)
        {
            float t = EaseInOut(elapsedTime / fadeDuration);

            scalingFunc(t);

            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        // Avoid scaling too much (less then 0 or above original size)
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
        // Formulas used:
        //   4x^3             before time has reached 0.5
        //   (x-1)*(2x-2)^2+1 after time has reached 0.5
        return t < 0.5f ? 4f * t * t * t : (t - 1f) * (2f * t - 2f) * (2f * t - 2f) + 1f;
    }
}
