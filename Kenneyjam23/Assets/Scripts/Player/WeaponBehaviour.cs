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

        PlaceWeapon(false);
    }

    private void LateUpdate()
    {
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
        PlaceWeapon(equipped);

        _canShoot = equipped;
    }

    private void PlaceWeapon(bool equipped)
    {
        if (equipped)
        {
            _pistol.transform.parent = _handPistol;

            _pistol.transform.localPosition = Vector3.zero;
            _pistol.transform.localRotation = Quaternion.identity;
            
        }
        else
        {
            _pistol.transform.parent = _hipPistol;
        
            Debug.Log("resetted to hip");

            _pistol.transform.localPosition = Vector3.zero;
            _pistol.transform.localRotation = Quaternion.identity;
        }
    }
}
