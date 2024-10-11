using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    [SerializeField] private Image image;

    private Transform _camera;

    private void Start()
    {
        _camera = Camera.main.transform;
    }

    public void SetValue(float t)
    {
        image.fillAmount = t;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + _camera.forward);
    }
}
