using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private NPCType _type = null;
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer = null;

    // Start is called before the first frame update
    void Start()
    {
        // Set NPC material based on type
        _skinnedMeshRenderer.material = _type.SkinMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
