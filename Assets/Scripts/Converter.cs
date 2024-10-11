using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Converter : ItemConteinerBase
{
    [SerializeField] private Entity _resolt;
    [SerializeField] private Bar _bar;
    [SerializeField] private NecessaryResources[] _necessaryResources;
    [SerializeField] private float _convertingTime;
    [SerializeField] private Transform _finishPoint;

    private ConverteState _tempState;
    private float _tempConvertTime;
    private Coroutine _procces;
    private void Start()
    {
        _bar.SetValue(0);
        _procces = StartCoroutine(Procces());
    }
    private IEnumerator Procces()
    {
        while (true) 
        {
            if(_tempState == ConverteState.Converting)
            {
                _bar.SetValue((_convertingTime - _tempConvertTime) / _convertingTime);
                _tempConvertTime -= Time.fixedDeltaTime;
                if(_tempConvertTime <= 0)
                {
                    _tempState = ConverteState.WaitToTake;
                    _tryTransit?.Invoke();
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public override bool CanGive(ItemType itemType)
    {
        if(_tempState != ConverteState.WaitToGive) return false;

        foreach(var item in _necessaryResources)
        {
            if (item.IsNeed(itemType)) return true;
        }
        return false;
    }
    public override bool CanTake(ItemType itemType)
    {
        return _tempState == ConverteState.WaitToTake;
    }

    public override void GiveItem(Item item)
    {
        item.MakeTransition(GetNewItemPosition(), GetNewItemAngle(), transform, () => { item.Hide(); });

        foreach(var nr in _necessaryResources)
        {
            if (nr.TryAdd(item.Type)) break;
        }

        foreach(var nr in _necessaryResources)
        {
            if (!nr.IsFull())
                return;
        }

        foreach (var nr in _necessaryResources)
            nr.Clear();

        _tempConvertTime = _convertingTime;
        _tempState = ConverteState.Converting;

        _tryTransit?.Invoke();
    }

    public override Item TakeItem(ItemType itemType)
    {
        _tempState = ConverteState.WaitToGive;
        _bar.SetValue(0);
        _tryTransit?.Invoke();
        Item res = Factory.Instance.Summon<Item>(_resolt);
        res.transform.position = _finishPoint.position;
        return res;
    }

    private enum ConverteState
    {
        WaitToGive,
        Converting,
        WaitToTake
    }

    [System.Serializable]
    private class NecessaryResources
    {
        [SerializeField] private ItemType _itemType;
        [SerializeField] private int _num;

        private int _tempNum;
        public void Clear()
        {
            _tempNum = 0;
        }
        public bool IsFull()
        {
            return _tempNum >= _num;
        }
        public bool IsNeed(ItemType itemType)
        {
            return itemType == _itemType && _tempNum < _num;
        }
        public bool TryAdd(ItemType itemType)
        {
            if(itemType == _itemType)
            {
                _tempNum++;
                return true;
            }
            return false;
        }

    }

}
