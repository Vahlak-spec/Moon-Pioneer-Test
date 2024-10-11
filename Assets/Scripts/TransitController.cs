using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitController : MonoBehaviour
{
    [SerializeField] private ItemType _type;
    [Space]
    [SerializeField] private ItemConteinerBase _fromConteiner;
    [SerializeField] private ItemConteinerBase _toConteiner;

    private bool _canTransit;
    private Coroutine _procces;

    private void Start()
    {
        _fromConteiner.Init(TryTransit);
        _toConteiner.Init(TryTransit);
        _procces = StartCoroutine(Procces());
    }

    private IEnumerator Procces()
    {
        while (true)
        {
            if (_canTransit)
            {
                if(_fromConteiner.CanTake(_type) && _toConteiner.CanGive(_type))
                {
                    _toConteiner.GiveItem(_fromConteiner.TakeItem(_type));
                    yield return new WaitForSeconds(SystemConst.transitDelayTime);
                }
                else
                {
                    _canTransit = false;
                }
            }
            else
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }

    private void TryTransit()
    {
        _canTransit = true;
    }
}
