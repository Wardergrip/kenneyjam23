using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public GameObject Cube;

    [SerializeField] private Animator _animator = null;

    private CharacterController _cc = null;

    private Vector2 _inputVec = Vector2.zero;
    private bool _hasGun = false;

    private float _speed = 7f;

    private float _gravity = 0;

    [SerializeField] private float _rotationRadius = 1.5f;
    [SerializeField] private float _rotationSpeed = 5f;

    public UnityEvent WeaponChange;

    /// <summary>
    /// Code for moving the player based on input from the keyboard
    /// </summary>

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _cc = GetComponent<CharacterController>();

        _gravity = Physics.gravity.y;
    }

    private void FixedUpdate()
    {
        Vector3 movementVec = new Vector3(_inputVec.x * _speed, _gravity, _inputVec.y * _speed);

        _cc?.Move(movementVec * Time.fixedDeltaTime);
    }

    private void LateUpdate()
    {
        RotatePlayer();

        _animator?.SetBool("IsMoving", _inputVec != Vector2.zero);
        _animator?.SetBool("HasGun", _hasGun);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        _inputVec = context.ReadValue<Vector2>();
    }

    public void OnShift(InputAction.CallbackContext context)
    {
        _hasGun = context.ReadValueAsButton();

        if(context.performed || context.canceled)
        {
            WeaponChange.Invoke();
        }
    }

    private void RotatePlayer()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) == false) return;

        Vector3 worldMousePos = hit.point;

        Vector2 dir = new Vector2(worldMousePos.x - transform.position.x, worldMousePos.z - transform.position.z);

        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0, angle, 0);

        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
    }
}
