using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Dialog Predicates")]
public class DialogBlock : ScriptableObject
{
    [Header("Text to display when it is an option to interact. Won't be showed if it's the root.")]
    public string ChoiceText = "Ask for a choicetext";
    [Header("Text to display that the NPC says. Will not be displayed if choices is empty.")]
    public string NpcSpeech = "You want some NPC speech? Here it is.";
    [Header("The condition that needs to be met before this dialog can be shown on screen, null = always show")]
    public DialogPredicate Predicate = null;
    [Header("This event will be called when this dialog become the active dialog")]
    public UnityEvent OnActivate = new UnityEvent();

    [Header("Respond options, if empty it will close the dialog")]
    public List<DialogBlock> Choices = new List<DialogBlock>();

    public void SetChoices(List<DialogBlock> choices)
    {
        // Remove the existing choices
        Choices.Clear();

        // Set the given choices
        Choices.AddRange(choices);
    }
}
