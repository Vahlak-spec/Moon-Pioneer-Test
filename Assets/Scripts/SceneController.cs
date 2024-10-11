using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] private ControllWatcher _controllWatcher;
    [SerializeField] private PlayerController _player;

    private void Start()
    {
        _controllWatcher.OnMoveAction += OnDragJoystick;

        _player.Init();
    }

    Vector3 newDirection;
    private void OnDragJoystick(Vector3 direction) 
    {
        newDirection = Vector3.zero;
        newDirection.x = direction.y;
        newDirection.y = 0;
        newDirection.z = -direction.x;
        _player.MoveJoystickDragHandler(newDirection);
    }
}
