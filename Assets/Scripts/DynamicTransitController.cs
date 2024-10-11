using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTransitController : MonoBehaviour
{
    [SerializeField] private ItemType _itemType;
    [Space]
    [SerializeField] private DynamicTransitElement _dynamicElementType;
    [SerializeField] private ItemConteinerBase _conteiner;

    private bool _canTransit;
    private bool _hasDynamicTransit;
    private ItemConteinerBase _dynamicConteiner;
    private Coroutine _procces;

    private void Start()
    {
        _conteiner.Init(TryTransit);
        _procces = StartCoroutine(Procces());
    }

    private IEnumerator Procces()
    {
        while (true)
        {
            if (_hasDynamicTransit && _canTransit)
            {
                switch (_dynamicElementType)
                {
                    case DynamicTransitElement.From:
                        if (_dynamicConteiner.CanTake(_itemType) && _conteiner.CanGive(_itemType))
                        {
                            _conteiner.GiveItem(_dynamicConteiner.TakeItem(_itemType));
                            yield return new WaitForSeconds(SystemConst.transitDelayTime);
                        }
                        else
                        {
                            _canTransit = false;
                        }
                    break;

                    case DynamicTransitElement.To:
                        if (_conteiner.CanTake(_itemType) && _dynamicConteiner.CanGive(_itemType))
                        {
                            _dynamicConteiner.GiveItem(_conteiner.TakeItem(_itemType));
                            yield return new WaitForSeconds(SystemConst.transitDelayTime);
                        }
                        else
                        {
                            _canTransit = false;
                        }
                        yield return new WaitForSeconds(SystemConst.transitDelayTime);
                    break;
                }
            }
            else
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ItemConteinerBase>(out ItemConteinerBase component))
        {
            _dynamicConteiner = component;
            _hasDynamicTransit = true;
            _canTransit = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<ItemConteinerBase>(out ItemConteinerBase component))
        {
            _dynamicConteiner = null;
            _hasDynamicTransit = false;
        }
    }

    private void TryTransit() 
    {
        _canTransit = true;
    }

    private enum DynamicTransitElement
    {
        From,
        To
    }
}
