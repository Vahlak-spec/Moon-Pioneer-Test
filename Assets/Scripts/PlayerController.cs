using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed;


    private Vector3 _moweDirection;
    private Coroutine _procces;

    public void MoveJoystickDragHandler(Vector3 dir) => _moweDirection = dir;

    public void Init()
    {
        _procces = StartCoroutine(Procces());
    }

    private IEnumerator Procces()
    {
        while (true)
        {
            transform.position = transform.position + (_moweDirection * _speed * Time.deltaTime);
            if (_moweDirection != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(_moweDirection);
            yield return new WaitForEndOfFrame();
        }
    }
}
