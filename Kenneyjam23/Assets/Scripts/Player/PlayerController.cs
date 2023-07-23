using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [SerializeField] private Animator _animator = null;

    private CharacterController _cc = null;

    private Vector2 _inputVec = Vector2.zero;
    private bool _hasGun = false;

    private float _speed = 7f;

    private float _gravity = 0;

    [SerializeField] private float _rotationRadius = 1.5f;
    [SerializeField] private float _rotationSpeed = 3f;
    [SerializeField] private float _rotationLerpSpeed = 10f;

    [SerializeField] private float _cameraRotSpeed = 10f;

    [SerializeField] private float _mouseRange = 20.0f;

    private float _cameraRotation = 0;
    private bool _canRotate = false;
    private float _cameraRotDir;

    public float CameraRotation { get { return _cameraRotation; } }

    public UnityEvent<bool> WeaponChange;

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

    private void LateUpdate()
    {
        Vector3 movementVec = transform.forward * _inputVec.y * _speed + Vector3.up * _gravity;

        _cc?.Move(movementVec * Time.deltaTime);

        RotatePlayer();

        Debug.Log(_cameraRotDir);
        RotateCamera();

        _animator?.SetBool("IsMoving", _inputVec.y > 0);
        _animator?.SetBool("IsMovingReverse", _inputVec.y < 0);
        _animator?.SetBool("HasGun", _hasGun);
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 wasd = context.ReadValue<Vector2>();

        _inputVec = Vector2.up * wasd.y;

        _cameraRotDir = wasd.x * _rotationSpeed;
        _canRotate = Mathf.Abs(_cameraRotDir) > float.Epsilon;
    }

    public void OnShift(InputAction.CallbackContext context)
    {
        _hasGun = context.ReadValueAsButton();

        if(context.performed || context.canceled)
        {
            WeaponChange.Invoke(_hasGun);
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

    private void RotateCamera()
    {
        if (_canRotate)
        {
            _cameraRotation += _cameraRotDir * Time.deltaTime * _cameraRotSpeed;

            _cameraRotation = (_cameraRotation + 360) % 360;
        }
    }
}
