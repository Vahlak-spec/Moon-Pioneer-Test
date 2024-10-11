using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : ItemConteinerBase
{
    [SerializeField] private Bar _bar;
    [SerializeField] private Entity _entityPref;
    [SerializeField] private ItemType _generateType;
    [SerializeField] private float _generateTime;
    [Space]
    [SerializeField] private Transform _spawenPosition;    

    private Coroutine _procces;
    private bool _canTake = false;
    private float _tempTimeLeft;

    private void Start()
    {
        _tempTimeLeft = _generateTime;
        _procces = StartCoroutine(Procces());
    }

    private IEnumerator Procces()
    {
        while (true)
        {
            if (!_canTake) 
            {
                _bar.SetValue((_generateTime - _tempTimeLeft) / _generateTime);
                _tempTimeLeft -= Time.fixedDeltaTime;
                if (_tempTimeLeft < 0)
                {
                    _canTake = true;
                    _tryTransit?.Invoke();
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public override bool CanTake(ItemType itemType)
    {
        return _canTake && itemType == _generateType;
    }

    public override Item TakeItem(ItemType itemType)
    {
        _tempTimeLeft = _generateTime;
        _canTake = false;

        Item res = Factory.Instance.Summon<Item>(_entityPref);
        res.transform.position = _spawenPosition.position;

        return res;
    }
}
