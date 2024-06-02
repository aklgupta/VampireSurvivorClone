using System;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
[DisallowMultipleComponent]
public class WeaponProjectile : MonoBehaviour {

	private const float TimeToLive = 10f;

	public float Damage { get; private set; }

	private float speed;
	private Transform target;
	private Vector2 direction;
	private float initTime;
	private new Rigidbody2D rigidbody2D;

	public event Action Died;
	public event Action<Collider2D> Hit;

	public void Initialize(float damage, float speed, Transform target) {
		Damage = damage;
		this.speed = speed;
		this.target = target;

		initTime = Time.time;
		if (!rigidbody2D && !TryGetComponent(out rigidbody2D))
			Debug.LogError($"No {nameof(Rigidbody2D)} on {gameObject.name}");
	}

	public void Initialize(float damage, float speed, Vector2 direction) {
		Damage = damage;
		this.speed = speed;
		this.direction = direction;

		initTime = Time.time;
		if (!rigidbody2D && !TryGetComponent(out rigidbody2D))
			Debug.LogError($"No {nameof(Rigidbody2D)} on {gameObject.name}");
	}

	void Update() {
		if (target && target.gameObject.activeInHierarchy) {
			direction = (target.position - transform.position).normalized;
		}
		else {
			target = null;
		}

		rigidbody2D.velocity = direction * speed;

		if (Time.time > initTime + TimeToLive) {
			Died?.Invoke();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.GetComponent<Enemy>()) {
			Hit?.Invoke(collision);
			Died?.Invoke();
		}
	}

}
