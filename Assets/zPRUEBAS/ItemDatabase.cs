using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Game/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<ItemDefinition> entries = new List<ItemDefinition>();

    private Dictionary<int, ItemDefinition> lookup;

    public void Initialize()
    {
        lookup = new Dictionary<int, ItemDefinition>();

        foreach (var def in entries)
        {
            if (def == null)
                continue;

            if (!lookup.ContainsKey(def.ID))
                lookup[def.ID] = def;
        }
    }

    public bool TryGet(int ID, out ItemDefinition definition)
    {
        if (lookup == null)
            Initialize();

        return lookup.TryGetValue(ID, out definition);
    }
}
