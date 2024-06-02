using System;
using UnityEngine;

/// <summary>
/// Base class for any kind of loot that enemies drop,
/// though it's flexible enough to be used for any kind of pick up items that you want to pool automatically.
/// </summary>
[RequireComponent (typeof(Collider2D))]
public abstract class LootDrop : MonoBehaviour {

	[SerializeField, Min(0)]
	private float _amount = 1;

	public float Amount {
		get => _amount;
		set => SetAmount(value);
	}


#if UNITY_EDITOR

	private void OnValidate() {
		// I have intentionally used "<= 0" here instead of "< 0" to show how we can use validate methods to 
		// validate conditions in editor.
		if (_amount <= 0)
			Debug.LogWarning("Drop amount is <= 0, check if that's intentional. amount = " + _amount);
	}

#endif

	public virtual void Collect() {
		LootDropPool.PoolDrop(this);
	}

	protected virtual void SetAmount(float value) {
		if (value < 0)
			Debug.LogWarning("Drop amount is < 0, check if that's intentional. amount = " + value);

		_amount = value;
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.GetComponent<Player>()) {
			Collect();
		}
	}

}
