using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;


/// <summary>
/// Spawns enemies according to the SpawnConfig. Uses Unity's Object Pooling methods to
/// pool them. For my solution for pooling, see `LootDropPool.cs`.
/// </summary>
public class EnemySpawner : MonoBehaviour {

	[SerializeField]
	private new Camera camera;

	[SerializeField, Tooltip("Distance from the edges of the screen where the enemies are spawned.")]
	private float spawnBuffer = 5;

	[SerializeField]
	private List<SpawnConfig> spawnConfigs;

	private Dictionary<SpawnConfig, ObjectPool<Enemy>> enemyObjectPool;
	private Rect spawnBound;


#if UNITY_EDITOR

	// Just trying to showcase some very simple editor only helper things that we can do.
	// I obviously didn't have the time or any features to make a full editor for,
	// so made these instead.

	/// <summary>
	/// Draws a green rectangle in the "Scene" window to show where the enemies will spawn.
	/// Devs & designers can easily tweak the values and see it live without having to ever run the game.
	/// It will throw an error if camera is not assigned. This is intentional.
	/// </summary>
	private void OnDrawGizmos() {
		CalculateSpawnBound();
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(
			spawnBound.center,
			spawnBound.size
		);
	}

	/// <summary>
	/// A very simple validation method. Just to show what all we can do with them.
	/// </summary>
	private void OnValidate() {
		if (!camera) {
			Debug.LogWarning($"Camera not assigned to {gameObject.name} ({nameof(EnemySpawner)})!");
		}
	}

#endif

	private void Start() {
		enemyObjectPool = new();
		foreach (var spawnConfig in spawnConfigs) {
			enemyObjectPool[spawnConfig] = new ObjectPool<Enemy>(
				createFunc: () => CreateEnemy(spawnConfig),
				actionOnGet: x => GetEnemy(x, spawnConfig),
				actionOnRelease: x => x.gameObject.SetActive(false),
				collectionCheck: false,
				defaultCapacity: 10,
				maxSize: 250
			);

			StartCoroutine(SpawnEnemy(spawnConfig));
		}
	}

	private void Update() {
		CalculateSpawnBound();
	}

	#region Object Pool Helper Methods

	private Enemy CreateEnemy(SpawnConfig spawnConfig) {
		var enemy = Instantiate(spawnConfig.SpawnPrefab, transform);
		enemy.gameObject.SetActive(false);
		enemy.Died += () => enemyObjectPool[spawnConfig].Release(enemy);
		return enemy;
	}

	private void GetEnemy(Enemy enemy, SpawnConfig spawnConfig) {
		enemy.transform.position = GetRandomSpawnPosition();
		enemy.gameObject.SetActive(true);
		enemy.Initialize(spawnConfig.EnemyConfig, Player.Instance ? Player.Instance.transform : null);
	}

	#endregion


	private IEnumerator SpawnEnemy(SpawnConfig spawnConfig) {
		var spawnFrequencies = spawnConfig.SpawnIntervals
			.OrderBy(x => x.PlayerLevel)
			.ToList();

		Debug.Log("Waiting for level requirements");
		// We should actually use the `Player.LeveledUp` event, but doing it just the show different ways to do things
		yield return new WaitUntil(() => Player.Instance.CurrentLevel >= spawnFrequencies[0].PlayerLevel);
		var interval = spawnFrequencies[0].SpawnInterval;
		Player.Instance.XpChanged += XpChanged;

		float lastSpawnTime = 0f;
		Debug.Log("Waiting for delay");
		yield return new WaitForSeconds(spawnConfig.SpawnDelay);

		while (Player.Instance && Player.Instance.Health > 0) {
			var enemy = enemyObjectPool[spawnConfig].Get();
			lastSpawnTime = Time.time;

			// It's possible the Player levels up and the `interval` changes while we are waiting.
			// That's why I am not using `WaitForSeconds` here.
			yield return new WaitWhile(() => lastSpawnTime + interval >= Time.time);
		}

		Debug.Log("Player dead. Stop Spawning!?");


		void XpChanged(float oldXp, float newXp) {
			var playerLevel = Player.Instance.CurrentLevel;
			foreach (var spawnFrequency in spawnFrequencies) {
				if (spawnFrequency.PlayerLevel <= playerLevel)
					interval = spawnFrequency.SpawnInterval;
				else
					break;
			}
		}
	}

	private Vector2 GetRandomSpawnPosition() {
		if (Random.value < 0.5f) {
			return new Vector2(
				Random.value < 0.5f ? spawnBound.xMin : spawnBound.xMax,
				Random.Range(spawnBound.yMin, spawnBound.yMax)
			);
		}
		else {
			return new Vector2(
				Random.Range(spawnBound.xMin, spawnBound.xMax),
				Random.value < 0.5f ? spawnBound.yMin : spawnBound.yMax
			);
		}
	}

	private void CalculateSpawnBound() {
		spawnBound.min = (Vector2)camera.ViewportToWorldPoint(Vector3.zero) - spawnBuffer * Vector2.one;
		spawnBound.max = (Vector2)camera.ViewportToWorldPoint(Vector3.one) + spawnBuffer * Vector2.one;
	}

	private void OnDestroy() {
		StopAllCoroutines();
	}

}
