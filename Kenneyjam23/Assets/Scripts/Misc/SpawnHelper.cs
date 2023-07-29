using UnityEngine;

public class SpawnHelper : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;

    public void SpawnAttached(Transform parent)
    {
        Instantiate(_prefab, parent);
    }

    public void SpawnOnPosition(Transform parent)
    {
        Instantiate(_prefab, parent.position,parent.rotation);
    }
}
