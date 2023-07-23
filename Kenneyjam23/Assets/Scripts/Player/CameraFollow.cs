using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private Transform _playerTransform = null;

    [SerializeField] private float _height = 15f;

    [SerializeField] private float _radius = 15f;

    [SerializeField] private float _rotation = 40f;

    [SerializeField] float _smoothSpeed = 0.125f;

    private Vector3 _offset = Vector3.zero;

    private PlayerController _playerController;

    void Start()
    {
        transform.position = _playerTransform.position + _offset;

        transform.localEulerAngles = new Vector3(_rotation, 0, 0);

        _playerController = PlayerController.Instance;

        float angleRad = Mathf.Deg2Rad * (_playerController.CameraRotation + 180);

        float offsetX = Mathf.Sin(angleRad) * _radius;
        float offsetZ = Mathf.Cos(angleRad) * _radius;

        _offset = new Vector3(offsetX, _height, offsetZ);
    }

    void LateUpdate()
    {
        if (_playerTransform == null)
        {
            Debug.LogWarning("No player transform to follow the camera around");
            return;
        }

        float angleRad = Mathf.Deg2Rad * (_playerController.CameraRotation + 180);

        float offsetX = Mathf.Sin(angleRad) * _radius;
        float offsetZ = Mathf.Cos(angleRad) * _radius;

        _offset = new Vector3(offsetX, _height, offsetZ);

        Vector3 desiredPosition = _playerTransform.position + _offset;
        
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);

        Vector3 directionToPlayer = _playerTransform.position - transform.position;
        directionToPlayer.y = 0.0f;

        Quaternion rotation = Quaternion.LookRotation(directionToPlayer.normalized, Vector3.up) * Quaternion.Euler(_rotation, 0.0f, 0.0f);

        transform.localRotation = rotation;

        transform.position = smoothedPosition;
    }
}
