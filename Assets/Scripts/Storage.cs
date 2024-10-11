using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : ItemConteinerBase
{
    [SerializeField] private int _conteinerLenth;
    [Space]
    [SerializeField] private Transform _beginPoint;
    [SerializeField] private int _xL;
    [SerializeField] private float _locaXStep;
    [SerializeField] private int _zL;
    [SerializeField] private float _locaZStep;
    [SerializeField] private float _locaYStep;
    [Space]
    [SerializeField] private bool _storageFullIsMessage;
    [SerializeField] private string _storageFullMessageText;
    [Space]
    [SerializeField] private bool _storageEmptyIsMessage;
    [SerializeField] private string _storageEmptyMessageText;
    private Item[] _items;
    private int _tempItem;
    private void Start()
    {
        _tempItem = -1;
        _items = new Item[_conteinerLenth];
    }
    public override bool CanTake(ItemType itemType)
    {
        if (_tempItem < 0) 
        {
            if (_storageEmptyIsMessage)
                MessageController.Instance.SummonMessage(_storageEmptyMessageText);

            return false; 
        }

        for(int i = 0; i <= _tempItem; i++)
        {
            if (_items[i].Type == itemType)
                return true;
        }
        return false;
    }
    public override bool CanGive(ItemType itemType)
    {
        if (_tempItem < _items.Length - 1)
        {
            return true;
        }

        if (_storageFullIsMessage)
            MessageController.Instance.SummonMessage(_storageFullMessageText);
        return false;
    }

    public override Item TakeItem(ItemType itemType)
    {
        for(int i = _tempItem; i >= 0; i--)
        {
            if (_items[i].Type == itemType)
            {
                _tempItem--;

                Item res = _items[i];

                Resort(i);
                _tryTransit?.Invoke();
                return res;
            }
        }
        return null;
    }
    private void Resort(int index)
    {
        for(int i = index; i <= _tempItem; i++)
        {
            _items[i] = _items[i + 1];
            _items[i].MakeTransition(GetNewItemPosition(i), GetNewItemAngle(), gameObject.transform);
        }
    }
    public override void GiveItem(Item item)
    {
        _tempItem++;
        _items[_tempItem] = item;
        _items[_tempItem].MakeTransition(GetNewItemPosition(_tempItem), GetNewItemAngle(), gameObject.transform);

        _tryTransit?.Invoke();
    }

    int width;
    int length;
    int height;
    public override Vector3 GetNewItemPosition(int number)
    {
        height = number / (_xL * _zL);
        length = (number - (height * _xL * _zL)) / _xL;
        width = (number - (height * _xL * _zL)) % _xL;

        Vector3 res = _beginPoint.localPosition;

        res.x += width * _locaXStep;
        res.z += length * _locaZStep;
        res.y += height * _locaYStep;

        return res;
    }
    public override Vector3 GetNewItemAngle()
    {
        return _beginPoint.localEulerAngles;
    }
}
