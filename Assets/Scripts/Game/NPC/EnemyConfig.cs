using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Contains info about enemies that the spawner can use to spawn and initialize the enemy with.
/// Since it's stored in a Scriptable Object, it makes it easy for designers to easily tweak values
/// and test things without asking devs to update values. And swapping config in and out is easier than
/// having them on GO. Expaning it in future would be easier, eg. We could make the "Health" field a
/// range, and then each copy of the enemy would receive a random health between the range.
/// </summary>
[Serializable]
public class EnemyConfig {

	[field: SerializeField]
	public float Speed { get; private set; }

	[field: SerializeField]
	public float Health { get; private set; }

	[field: SerializeField]
	public float Damage { get; private set; }

	[SerializeField]
	private List<EnemyDropData> drops;


	public IReadOnlyList<EnemyDropData> Drops => drops;

}

/// <summary>
/// The item that an enemy may drop on death.
/// </summary>
[Serializable]
public class EnemyDropData {

	[field: SerializeField]
	public LootDrop DropPrefab { get; private set; }

	[field: SerializeField, Min(0)]
	public float Amount { get; private set; }

	[field: SerializeField, Range(0, 1)]
	public float Probability { get; private set; }

}
