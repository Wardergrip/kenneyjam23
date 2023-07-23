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
        Vector3 movementDir = Camera.main.transform.forward;
        movementDir.y = 0.0f;
        movementDir.Normalize();

        Vector3 movementVec = movementDir * _speed * _inputVec.y + Vector3.up * _gravity;

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

    public void OnRotate(InputAction.CallbackContext context)
    {
        //_cameraRotDir = context.ReadValue<float>();

        _canRotate = context.performed;
    }

    private void RotatePlayer()
    {
        Vector3 direction = Camera.main.transform.forward;
        direction.y = 0.0f;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction.normalized, Vector3.up), _rotationLerpSpeed * Time.deltaTime);
    }

    private void RotateCamera()
    {
        //Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        //RaycastHit hit;

        //_canRotate = false;
        //_cameraRotDir = 0;

        //if (Physics.Raycast(ray, out hit))
        //{
        //    Vector3 worldMousePos = hit.point;

        //    Vector3 dir = new Vector3(worldMousePos.x - transform.position.x, 0, worldMousePos.z - transform.position.z);

        //    float angle = Vector3.SignedAngle(transform.forward, dir, Vector3.up);

        //    Debug.Log(angle);

        //    if (Mathf.Abs(angle) > _mouseRange)
        //    {
        //        _cameraRotDir = angle / Mathf.Abs(angle);
        //        _canRotate = true;
        //    }
        //}

        if (_canRotate)
        {
            _cameraRotation += _cameraRotDir * Time.deltaTime * _cameraRotSpeed;

            _cameraRotation = (_cameraRotation + 360) % 360;
        }
    }
}
