using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField] private GameObject _player = null;

    [SerializeField] private Vector3 _offset = Vector3.zero;

    [SerializeField] private float _rotation = 0f;

    [SerializeField] float _smoothSpeed = 0.125f;

    private Transform _playerTransform = null;

    void Start()
    {
        if( _player != null )
        {
            _playerTransform = _player.GetComponent<Transform>();

            transform.position = _playerTransform.position + _offset;

            transform.localEulerAngles = new Vector3(_rotation, 0, 0);
        }
    }

    void LateUpdate()
    {
        if (_playerTransform == null)
        {
            Debug.LogWarning("No player transform to follow the camera around");
            return;
        }

        Vector3 desiredPosition = _playerTransform.position + _offset;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);

        //this is to change rotation on runtime for debug puproses
        transform.localEulerAngles = new Vector3(_rotation, 0, 0);

        transform.position = smoothedPosition;
    }
}
