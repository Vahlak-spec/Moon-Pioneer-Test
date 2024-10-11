using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MessageController : MonoBehaviour
{
    public static MessageController Instance => _instance;
    private static MessageController _instance;

    [SerializeField] private Message[] _messages;
    [Space]
    [SerializeField] private float _messageActiveTime;
    [SerializeField] private float _messageSummonPeriodicity;

    private Coroutine _procces;

    private void Start()
    {
        _instance = this;
        _procces = StartCoroutine(Procces());
    }

    public void SummonMessage(string text)
    {
        foreach (var item in _messages)
        {
            if (item.IsEngaged(text)) return;
        }

        foreach (var item in _messages)
        {
            if (item.Summon(text, _messageActiveTime, _messageSummonPeriodicity)) return;
        }
    }

    private IEnumerator Procces()
    {
        while (true)
        {
            foreach (var item in _messages) item.OnFixed();

            yield return new WaitForFixedUpdate();
        }
    }

    [System.Serializable]
    private class Message
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private GameObject _gameObject;

        float _tempActiveTime;
        float _tempEngagedTime;

        private bool _canUse;

        public bool IsEngaged(string text)
        {
            return text == _text.text && !_canUse;
        }

        public bool Summon(string text, float activeTime, float engagedTime)
        {
            if (!_canUse) return false;

            _text.text = text;
            _tempActiveTime = activeTime;
            _tempEngagedTime = engagedTime;
            _canUse = false;
            _gameObject.SetActive(true);

            _gameObject.transform.SetSiblingIndex(_gameObject.transform.parent.childCount - 1);

            return true;
        }
        public void OnFixed()
        {
            _tempActiveTime -= Time.fixedDeltaTime;
            _tempEngagedTime -= Time.fixedDeltaTime;

            if (_tempActiveTime < 0)
            {
                _gameObject.SetActive(false);
            }
            if (_tempEngagedTime < 0)
            {
                _canUse = true;
            }
        }
    }
}
