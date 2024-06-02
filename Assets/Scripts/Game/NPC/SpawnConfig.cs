using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A Scriptable Object to store the configs about spawning enemies.
/// We can actually replace `EnemyConfig` with a generic `Config` and use this as a spawn config
/// for more than just enemies. Just showing how to makes scalable and reuseable systems.
/// </summary>
[CreateAssetMenu]
public class SpawnConfig : ScriptableObject {

	[field: SerializeField]
	public Enemy SpawnPrefab { get; private set; }

	[field: SerializeField, Range(0, 10)]
	public float SpawnDelay { get; private set; }

	[field: SerializeField]
	public EnemyConfig EnemyConfig { get; private set; }

	[SerializeField]
	private List<SpawnFrequency> spawnIntervals;


	public IReadOnlyList<SpawnFrequency> SpawnIntervals => spawnIntervals;

}


/// <summary>
/// This is slightly specific to the test, but can actually be more generic.
/// </summary>
[Serializable]
public class SpawnFrequency {

	[field: SerializeField, Range(1, Player.MaxLevel)]
	public int PlayerLevel { get; private set; }

	[field: SerializeField, Range(0.1f, 60)]
	public float SpawnInterval { get; private set; }

}