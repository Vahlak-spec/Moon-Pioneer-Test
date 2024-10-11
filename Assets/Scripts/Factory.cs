using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    public static Factory Instance => instance;

    private static Factory instance;

    [SerializeField] private SpawenData[] _datas;

    private void Start()
    {
        instance = this;

        foreach(var item in _datas)
            item.Init();
    }

    public T Summon<T>(Entity prefab) 
    {
        foreach(var item in _datas)
        {
            if (item.ItFit(prefab))
                return item.Summon<T>();
        }
        return default(T);
    }

    [System.Serializable]
    private class SpawenData
    {
        [SerializeField] private Entity _prefab;
        [SerializeField] private int _num;

        private Entity[] _entities;

        public void Init() 
        {
            _entities = new Entity[_num];
            for(int i = 0; i < _num; i++)
            {
                _entities[i] = Instantiate(_prefab);
                _entities[i].Hide();
            }
        }
        public T Summon<T>()
        {
            foreach(var item in _entities)
            {
                if (item.IsHide)
                {
                    item.Summon();
                    return item.GetComponent<T>();
                }
            }
            return default(T);
        }
        public bool ItFit(Entity prefab) => _prefab == prefab;
    }
}
