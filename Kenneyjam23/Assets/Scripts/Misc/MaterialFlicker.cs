using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialFlicker : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    [SerializeField] private Material _matToFlickerTo;
    //private string _colorPropertyName = "_base_color";

    public void Flicker(float length)
    {
        var originalMat = _skinnedMeshRenderer.material;
        _skinnedMeshRenderer.material = _matToFlickerTo;
        StartCoroutine(ChangeColorCoroutine(originalMat, length));
    }

    private IEnumerator ChangeColorCoroutine(Material mat, float length)
    {
        yield return new WaitForSeconds(length);
        _skinnedMeshRenderer.material = mat;
    }
}
