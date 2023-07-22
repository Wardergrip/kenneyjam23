using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class DialogBlock : ScriptableObject
{
    [Header("Text to display when it is an option to interact. Won't be showed if it's the root.")]
    public string ChoiceText = "Ask for a choicetext";
    [Header("Text to display that the NPC says. Will not be displayed if choices is empty.")]
    public string NpcSpeech = "You want some NPC speech? Here it is.";

    [Header("Respond options, if empty it will close the dialog")]
    public List<DialogBlock> Choices;
}
