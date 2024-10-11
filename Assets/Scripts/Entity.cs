using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public bool IsHide => _isHide;
    private bool _isHide;
    public virtual void Summon() { _isHide = false; }
    public virtual void Hide() { _isHide = true; }
}
