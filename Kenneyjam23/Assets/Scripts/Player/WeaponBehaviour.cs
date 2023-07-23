using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _hipPistol;
    [SerializeField] private Transform _handPistol;
    //
    //[SerializeField] private Transform _hipShotgun;
    //[SerializeField] private Transform _handShotgun;

    [SerializeField] private GameObject _pistol;
    //[SerializeField] private GameObject _shotgun;

    private Gun _currentGun;

    private bool _canShoot = false;

    private void Start()
    {
        PlayerController.Instance.WeaponChange.AddListener(SwitchPosition);

        _currentGun = _pistol.GetComponent<Gun>();

        PlaceWeapon();
    }

    private void LateUpdate()
    {
        //the gun is socketed to the hand of the animation but swirls, this assures the gun always aims forward
        if (_canShoot)
        {
            _handPistol.transform.rotation = transform.rotation;
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if(context.performed && _canShoot)
        {
            _currentGun.TryShoot();
        }
    }

    private void SwitchPosition(bool equipped)
    {
        _canShoot = equipped;

        PlaceWeapon();
    }

    private void PlaceWeapon()
    {
        if (_canShoot)
        {
            AttachWeapon(_handPistol);

        }
        else
        {
            AttachWeapon(_hipPistol);
        }
    }

    private void AttachWeapon(Transform socket)
    {
        _pistol.transform.parent = socket;

        _pistol.transform.localPosition = Vector3.zero;
        _pistol.transform.localRotation = Quaternion.identity;
    }
}
