using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private NPCType _type = null;
    public NPCType GetNPCType() { return _type; }

    [SerializeField] private QuestManager _questManager = null;
    public QuestManager GetQuestManager() { return _questManager; }

    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer = null;

    // Start is called before the first frame update
    void Start()
    {
        // Set NPC material based on type
        _skinnedMeshRenderer.material = _type.SkinMaterial;
    }
}
