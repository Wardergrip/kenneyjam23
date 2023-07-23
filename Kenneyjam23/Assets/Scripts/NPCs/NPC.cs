using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private NPCType _type = null;
    public NPCType Type
    {
        get { return _type; }
    }

    [SerializeField] private QuestManager _questManager = null;
    public QuestManager QuestManager
    {
        get { return _questManager; }
    }

    [Header("Leave choices empty! Choices are set through code")]
    [SerializeField] private DialogBlock _dialog = null;

    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer = null;

    // Start is called before the first frame update
    void Start()
    {
        // Set NPC material based on type
        _skinnedMeshRenderer.material = _type.SkinMaterial;

        // Set interact and exit dialogs as choices for NPC dialog
        if (_dialog != null)
        {
            List<DialogBlock> choices = new List<DialogBlock>() { QuestManager.InteractDialog, QuestManager.ExitDialog };

            _dialog.SetChoices(choices);
        }
    }

    public void Interact()
    {
        // Check if dialog exists
        if (_dialog == null) return;

        // Set dialog to show
        DialogHandler dialogHandler = FindObjectOfType<DialogHandler>();
        dialogHandler.CurrentDialogBlock = _dialog;

        // Open dialog
        dialogHandler.OpenDialog();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger
        if (other.gameObject.GetComponent<PlayerController>())
        {
            Interact();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player exited the trigger
        if (other.gameObject.GetComponent<PlayerController>())
        {
            // Get dialog handler
            DialogHandler dialogHandler = FindObjectOfType<DialogHandler>();

            // Close dialog
            dialogHandler.CloseDialog();
        }
    }
}
