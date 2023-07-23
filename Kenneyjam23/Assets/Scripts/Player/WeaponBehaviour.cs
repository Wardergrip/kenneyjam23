using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class WeaponBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _hipPistol;
    [SerializeField] private Transform _hipShotgun;

    [SerializeField] private Transform _handSocket;

    [SerializeField] private GameObject _pistol;
    [SerializeField] private GameObject _shotgun;

    private Gun _currentGun;

    private Gun _pistolGunComp;
    private Gun _shotGunComp;

    private bool _canShoot = false;
    private bool _isPistol = false;

    public UnityEvent WeaponSwap;

    private void Start()
    {
        PlayerController.Instance.WeaponChange.AddListener(SwitchPosition);

        _pistolGunComp = _pistol.GetComponent<Gun>();
        _shotGunComp = _shotgun.GetComponent<Gun>();

        _currentGun = _pistolGunComp;
        _isPistol = true;

        AttachWeapon(_hipPistol, _pistol);
        AttachWeapon(_hipShotgun, _shotgun);
    }

    private void LateUpdate()
    {
        //the gun is socketed to the hand of the animation but swirls, this assures the gun always aims forward
        if (_canShoot)
        {
            _handSocket.transform.rotation = transform.rotation;
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if(context.performed && _canShoot)
        {
            _currentGun.TryShoot();
        }
    }

    public void SwapWeapon(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();

        if (!context.performed) return;

        if(value == -1)
        {
            _currentGun = _pistolGunComp;
            _isPistol = true;
        }
        else if(value == 1)
        {
            _currentGun = _shotGunComp;
            _isPistol = false;
        }

        PlaceWeapon();

        WeaponSwap.Invoke();
    }

    private void SwitchPosition(bool equipped)
    {
        _canShoot = equipped;

        PlaceWeapon();
    }

    private void PlaceWeapon()
    {
        if(_canShoot)
        {
            if(_isPistol)
            {
                AttachWeapon(_handSocket, _pistol);
                AttachWeapon(_hipShotgun, _shotgun);
            }
            else
            {
                AttachWeapon(_handSocket, _shotgun);
                AttachWeapon(_hipPistol, _pistol);
            }
        }
        else
        {
            if (_isPistol)
            {
                AttachWeapon(_hipPistol, _pistol);
            }
            else
            {
                AttachWeapon(_hipShotgun, _shotgun);
            }
        }
    }

    private void AttachWeapon(Transform socket, GameObject weapon)
    {

        weapon.transform.parent = socket;

        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
    }
}
