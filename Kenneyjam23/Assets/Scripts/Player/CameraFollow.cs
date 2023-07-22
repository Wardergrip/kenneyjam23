using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private Transform _playerTransform = null;

    [SerializeField] private Vector3 _offset = Vector3.zero;

    [SerializeField] private float _rotation = 0f;

    [SerializeField] float _smoothSpeed = 0.125f;

    private PlayerController _playerController;

    void Start()
    {
        transform.position = _playerTransform.position + _offset;

        transform.localEulerAngles = new Vector3(_rotation, 0, 0);

        _playerController = PlayerController.Instance;
    }

    void LateUpdate()
    {
        if (_playerTransform == null)
        {
            Debug.LogWarning("No player transform to follow the camera around");
            return;
        }

        float angleRad = Mathf.Deg2Rad * (_playerController.CameraRotation + 180);

        float offsetX = Mathf.Sin(angleRad) * 15;
        float offsetZ = Mathf.Cos(angleRad) * 15;

        _offset = new Vector3(offsetX, _offset.y, offsetZ);

        Vector3 desiredPosition = _playerTransform.position + _offset;
        
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);

        // Calculate the direction from the camera to the player
        Vector3 directionToPlayer = _playerTransform.position - transform.position;

        // Calculate the target y-rotation angle in degrees
        float targetYRot = Mathf.Atan2(directionToPlayer.x, directionToPlayer.z) * Mathf.Rad2Deg;

        // Smoothly interpolate between the current y-rotation and the target y-rotation
        float yRot = Mathf.LerpAngle(transform.localEulerAngles.y, targetYRot, _smoothSpeed * Time.fixedDeltaTime);

        transform.localEulerAngles = new Vector3(_rotation, yRot, 0);
        
        transform.position = smoothedPosition;
    }
}
