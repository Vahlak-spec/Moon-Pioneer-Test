using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Entity
{
    public ItemType Type => _type;

    [SerializeField] private ItemType _type;

    private Coroutine _coroutine;
    private float _tpf;

    private void Start()
    {
        _tpf = 1f / SystemConst.itemTransitionTime * Time.fixedDeltaTime;
    }

    public override void Summon()
    {
        base.Summon();
        gameObject.SetActive(true);
    }
    public override void Hide()
    {
        base.Hide();
        gameObject.SetActive(false);
    }

    public void MakeTransition(Vector3 newPosition, Vector3 newRotation, Transform newParent = null, Action onComplete = null)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(Transition(newPosition, newRotation, newParent));
    }

    public IEnumerator Transition(Vector3 newLocalPosition, Vector3 newEulerAngle, Transform newParent, Action onComplete = null)
    {
        float t = 0;

        transform.parent = newParent;
        Vector3 oldPosition = transform.localPosition;
        Vector3 oldRotation = transform.eulerAngles;

        while (t < 1)
        {
            t += _tpf;
            transform.localPosition = Vector3.Lerp(oldPosition, newLocalPosition, t);
            transform.localEulerAngles = Vector3.Lerp(oldRotation, newEulerAngle, t);
            yield return new WaitForFixedUpdate();
        }
        onComplete?.Invoke();
    }
}

public enum ItemType
{
    green,
    red,
    perple
}
