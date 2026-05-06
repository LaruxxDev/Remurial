using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Game/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    [System.Serializable]
    public struct Entry
    {
        public string displayName;

        public string databaseID;
        public Sprite sprite;
        public GameObject prefab;
    }

    public List<Entry> entries = new List<Entry>();

    private Dictionary<string, Entry> _lookup;

    public void Initialize()
    {
        _lookup = new Dictionary<string, Entry>();

        foreach (var e in entries)
            _lookup[e.databaseID] = e;
    }

    public bool TryGet(string id, out Entry entry)
    {
        if (_lookup == null)
            Initialize();

        return _lookup.TryGetValue(id, out entry);
    }
}
