using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChestFailMessage : MonoBehaviour
{
    [Header("UI elements")]
    [SerializeField] private Image _image = null;
    [SerializeField] private TMP_Text _text = null;

    [Header("Logic data")]
    [SerializeField] private float _aliveTime = 3.0f;
    [SerializeField] private float _moveSpeed = 2.0f;

    private float _curTime = 0.0f;

    private void Start()
    {
        StartCoroutine(DestroyAfterTime());
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(_aliveTime);
        Destroy(gameObject);
    }

    private void Update()
    {
        // Move the message up and rotate the message towards the camera
        transform.position += Vector3.up * _moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(Vector3.Normalize(transform.position - Camera.main.transform.position), Vector3.up);

        _curTime += Time.deltaTime;

        // Update the opacity of the text and image
        Color imageColor = _image.color;
        imageColor.a = 1.0f - 1.0f * _curTime / _aliveTime;
        _image.color = imageColor;

        Color textColor = _text.color;
        textColor.a = 1.0f - 1.0f * _curTime / _aliveTime;
        _text.color = textColor;
    }
}
