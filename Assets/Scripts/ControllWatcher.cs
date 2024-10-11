using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControllWatcher : MonoBehaviour
{
    [SerializeField] private Joystick _joystick;

    public UnityAction<Vector3> OnMoveAction;

    private void FixedUpdate()
    {
        OnMoveAction.Invoke(_joystick.Direction);
    }
}
