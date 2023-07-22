using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private NPCType _type = null;
    public NPCType GetNPCType() { return _type; }

    [SerializeField] private QuestManager _questManager = null;
    public QuestManager GetQuestManager() { return _questManager; }

    [SerializeField] private DialogBlock _dialog = null;

    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer = null;

    // Start is called before the first frame update
    void Start()
    {
        // Set NPC material based on type
        _skinnedMeshRenderer.material = _type.SkinMaterial;

        Invoke(nameof(Interact), 3.0f);
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
        if (true) // TODO: Implement actual check
        {
            //...
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player exited the trigger
        if (true) // TODO: Implement actual check
        {
            //...
        }
    }
}
