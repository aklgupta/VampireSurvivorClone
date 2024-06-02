using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This is a helper class used by LootDrop to object pool them automatically, reducing the overhead for programmers.
/// Due to the scope of the test, it's quite simple but it can have further additions 
/// eg. automatically release pools if they are not being used.
/// 
/// Also, I have implemented pooling from scratch instead of using Unity's in built pooling feature to showcase
/// that I understand object pooling and can work even without provided solutions.
/// </summary>
public static class LootDropPool {

	private static GameObject poolHolder;
	private static Dictionary<LootDrop, Queue<LootDrop>> pool;
	private static Dictionary<LootDrop, LootDrop> active; // This can be a proper data type, but I skipped that for the test

	static LootDropPool() {
		pool = new();
		active = new();
		SceneManager.sceneUnloaded += OnSceneUnloaded;
		SceneManager.sceneLoaded += OnSceneLoaded;
		CreatePoolParent();
	}

	private static void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		CreatePoolParent();
	}

	private static void CreatePoolParent() {
		// We can make it a persistent Object, but since we are clearing all pooled objects regardless
		// when switchign scenes, I am making a new empty "container" GO every scene.
		if (poolHolder == null)
			poolHolder = new GameObject("LootDropPool");
	}

	private static void OnSceneUnloaded(Scene scene) {
		pool.Clear();
		active.Clear();
	}


	/// <summary>
	/// Create a Pool for the provided LootDrop if it doesn't exist already.
	/// </summary>
	/// <param name="original"></param>
	/// <returns>True if a new Pool was created. False if it already exists.</returns>
	private static bool CreatePool(LootDrop original) {
		if (pool.ContainsKey(original)) {
			return false;
		}

		pool[original] = new();
		return true;
	}

	public static LootDrop GetDrop(LootDrop original) {
		CreatePool(original);

		var drop = (pool[original].Count == 0 ? Object.Instantiate(original, poolHolder.transform) : pool[original].Dequeue());
		drop.gameObject.SetActive(true);
		active[drop] = original;
		return drop;
	}

	public static LootDrop CreateDropAt(LootDrop original, Vector3 position) {
		var drop = GetDrop(original);
		drop.transform.position = position;
		return drop;
	}


	/// <summary>
	/// Add back an item to the pool.
	/// </summary>
	/// <param name="drop"></param>
	/// <returns></returns>
	public static bool PoolDrop(LootDrop drop) {
		if (!active.ContainsKey(drop)) {
			Debug.LogError($"The drop {drop} was not created by the pool");
			return false;
		}

		drop.gameObject.SetActive(false);
		active.Remove(drop, out var original);
		pool[original].Enqueue(drop);

		return true;
	}

}
