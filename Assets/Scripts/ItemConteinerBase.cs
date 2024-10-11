using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemConteinerBase : MonoBehaviour
{
    protected Action _tryTransit;

    public void Init(Action tryTransit)
    {
        _tryTransit += tryTransit;
    }

    public virtual bool CanTake(ItemType itemType)
    {
        return false;
    }
    public virtual bool CanGive(ItemType itemType)
    {
        return false;
    }
    public virtual Item TakeItem(ItemType itemType) 
    {
        return null;
    }
    public virtual void GiveItem(Item item) { }


    public virtual Vector3 GetNewItemPosition(int number = 0)
    {
        return Vector3.zero;
    }
    public virtual Vector3 GetNewItemAngle()
    {
        return Vector3.zero;
    }
}
