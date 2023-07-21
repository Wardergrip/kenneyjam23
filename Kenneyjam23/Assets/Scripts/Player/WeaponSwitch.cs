using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    private bool _equipped = false;

    [SerializeField] private Transform _hipPistol;
    [SerializeField] private Transform _handPistol;

    [SerializeField] private Transform _hipShotgun;
    [SerializeField] private Transform _handShotgun;

    [SerializeField] private GameObject _pistol;

    private void Start()
    {
        PlayerController.Instance.WeaponChange.AddListener(SwitchPosition);
        _pistol.transform.localScale = new Vector3(_pistol.transform.localScale.x, _pistol.transform.localScale.y, 21);
        PlaceWeapon();
    }

    private void SwitchPosition()
    {
        _equipped = !_equipped;
        PlaceWeapon();
    }

    private void PlaceWeapon()
    {
        if( _equipped )
        {
            _pistol.transform.position = _handPistol.transform.position;
            _pistol.transform.rotation = _handPistol.transform.rotation;
        }
        else
        {
            _pistol.transform.position = _hipPistol.transform.position;
            _pistol.transform.rotation = _hipPistol.transform.rotation;
        }
    }
}
