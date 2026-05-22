using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyDatabase : ScriptableObject // Base de datos de las plantas existentes.
{
    public List<EnemyData> enemyData;
}

[Serializable]
public class EnemyData // Variables que posee cada planta.
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public int ID { get; private set; }
    [field: SerializeField] public GameObject Prefab { get; private set; }
}