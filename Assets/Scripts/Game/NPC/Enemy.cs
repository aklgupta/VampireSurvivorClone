using System;
using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour {

	private float Damage => enemyConfig.Damage;

	private EnemyConfig enemyConfig;
	private Transform target;
	private float speed;
	private float currentHealth;

	// I would prefer a state machine or separate "Effect" classes, but as a last minute addition,
	// implemented quick dirty. Ideally "Enemy" class should not be responsible for handling the 
	// frozen state. Or any other, as we could end up with several dozen states in a real game.
	private bool frozen;
	private float freezeTimeLeft;

	public event Action Died;

	public void Initialize(EnemyConfig config, Transform target) {
		enemyConfig = config;
		currentHealth = config.Health;
		speed = enemyConfig.Speed;
		this.target = target;
	}

	void Update() {
		Move();
	}

	private void Move() {
		if (target == null)
			return;

		transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
	}

	public float GetDamage() => Damage;

	private void Die() {
		Died?.Invoke();

		// It's possible to drop multiple items. However, they are spawned at same location. COvering each other.
		// Ideally, they would either spread, or circle around the center or something.
		foreach (var dropData in enemyConfig.Drops) {
			if (UnityEngine.Random.value < dropData.Probability) {
				var drop = LootDropPool.CreateDropAt(dropData.DropPrefab, transform.position);
				drop.Amount = dropData.Amount;
			}
		}
		StopAllCoroutines();
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (currentHealth <= 0)
			return;

		if (collision.GetComponent<Player>()) {
			currentHealth = 0;
		}
		else if (collision.TryGetComponent<WeaponProjectile>(out var projectile)) {
			currentHealth -= projectile.Damage;
		}

		if (currentHealth <= 0)
			Die();
	}

	public void ApplyFreeze(float freezeTime) {
		if (!gameObject.activeInHierarchy)
			return;

		if (frozen) {
			// If already frozen, only increase the time.
			// Currently we ahve only one freeze effect, but it's possible that there are more than one way to freeze
			// the enemy in a real game, each freezing for a different amount of time. So, we don't simply
			// `freezeTimeLeft = freezeTime`
			// beacuse we don't want to undo the effect of a larger freeze effect by a smaller one.
			freezeTimeLeft = Mathf.Max(freezeTime, freezeTimeLeft);
		}
		else {
			StartCoroutine(PlayFreezeEffect(freezeTime));
		}
	}

	private IEnumerator PlayFreezeEffect(float forSeconds) {
		// As stated above, I included the freeze logic and made `Enemy` responsible for it,
		// just because it's a test and it's a last minute addition.

		freezeTimeLeft = forSeconds;
		var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		Color originalColor = Color.white;

		Debug.Log($"enemyConfig null? {enemyConfig == null}");
		Debug.Log($"enemyConfig = {enemyConfig}");

		speed = 0;
		if (spriteRenderer) {
			// Just a dirty way for the test.
			originalColor = spriteRenderer.color;
			spriteRenderer.color = Color.blue;
		}
		frozen = true;
		Died += RemoveFreezeBuff;

		while (freezeTimeLeft > 0) {
			yield return null;
			freezeTimeLeft -= Time.deltaTime;
		}
		RemoveFreezeBuff();


		void RemoveFreezeBuff() {
			speed = enemyConfig.Speed;
			if (spriteRenderer) {
				spriteRenderer.color = originalColor;
			}
			frozen = false;

			Died -= RemoveFreezeBuff;
		}
	}

	private void OnDisable() {
		StopAllCoroutines();
	}

	private void OnDestroy() {
		StopAllCoroutines();
	}

}
