using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private List<Transform> _gunOutputs = new List<Transform>();

    [Header("Gun Properties")]
    [SerializeField, Range(1, 100)] private int _maxAmmo;
    private int _currentAmmo;
    [SerializeField, Range(0, 10)] private float _reloadTime;
    [SerializeField, Range(0.1f, 100)] private float _rateOfFire;
    private float _shootTimer = 0;
    [SerializeField] private bool _automaticReload = true;
    private bool _isReloading = false;
    //private bool _justShot = false;

    [Header("Resources")]
    public GameObject _projectilePrefab;

    [Header("Events")]
    public UnityEvent<float> StartReloadEvent;
    public UnityEvent ReloadDoneEvent;
    public UnityEvent<Transform> ShotFiredEvent;

    // Properties
    public int CurrentAmmo
    {
        get { return _currentAmmo; }
    }
    public int MaxAmmo
    {
        get { return _maxAmmo; }
    }
    public float ReloadTime
    {
        get { return _reloadTime; }
    }
    public float RateOfFire
    {
        get { return _rateOfFire; }
    }
    public float TimeBetweenShots
    {
        get { return 1f / _rateOfFire; }
    }
    public bool IsReloading
    {
        get { return _isReloading; }
    }

    // Functionality
    private void Awake()
    {
        if (_gunOutputs == null)
        {
            Debug.Log("gunOutput not assigned");
        }
        if (_projectilePrefab == null)
        {
            Debug.Log("no projectile assigned");
        }
        _currentAmmo = _maxAmmo;
    }

    public void Reload()
    {
        if (_isReloading) return;

        Invoke(nameof(InstantReload), _reloadTime);
        _isReloading = true;
        StartReloadEvent?.Invoke(_reloadTime);
    }

    private void InstantReload()
    {
        _currentAmmo = _maxAmmo;
        _shootTimer = 0;
        _isReloading = false;
        transform.localRotation = Quaternion.identity;
        ReloadDoneEvent?.Invoke();
    }

    private void Update()
    {
        if (_shootTimer > 0)
        {
            _shootTimer -= Time.deltaTime;
        }
        if (_automaticReload)
        {
            if (_currentAmmo <= 0)
            {
                Reload();
            }
        }
    }

    public bool TryShoot()
    {
        if (IsReloading)
        {
            return false;
        }

        // Do we have ammo?
        if (_currentAmmo <= 0)
        {
            return false;
        }

        // Has enough time passed since our last shot? (RateOfFire)
        if (_shootTimer > 0)
        {
            return false;
        }

        _shootTimer = TimeBetweenShots;

        foreach(Transform output in _gunOutputs)
        {
            Instantiate(_projectilePrefab, output.position, output.rotation);

            --_currentAmmo;
        }

        ShotFiredEvent?.Invoke(_gunOutputs[0]);

        return true;
    }
}
