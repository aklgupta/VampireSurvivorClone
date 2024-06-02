using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;


public class WeaponManager : MonoBehaviour {

	[SerializeField]
	private WeaponData currentWeapon;

	[SerializeField]
	private LayerMask enemyLayers;


	private ObjectPool<WeaponProjectile> Projectiles {
		get {
			CreateWeaponProjectilePool();
			return weaponProjectiles[currentWeapon];
		}
	}

	private Dictionary<WeaponData, ObjectPool<WeaponProjectile>> weaponProjectiles = new();
	private List<WeaponData> weapons = new();

	private ContactFilter2D contactFilter;
	private List<Collider2D> colliders;

	private float AttackSpeedBuff = 0;
	private float AttackDamageBuff = 0;

	public event Action<WeaponData> WeaponSwitched;


	private void Start() {
		weapons.Add(currentWeapon);

		colliders = new List<Collider2D>();
		contactFilter = new ContactFilter2D() {
			useTriggers = true,
			useLayerMask = true,
			layerMask = enemyLayers,
		};

		AutoAttack();
	}

	private void CreateWeaponProjectilePool() {
		if (!weaponProjectiles.ContainsKey(currentWeapon)) {
			weaponProjectiles[currentWeapon] = new ObjectPool<WeaponProjectile>(
				createFunc: CreateProjectile,
				actionOnGet: GetProjectile,
				actionOnRelease: x => x.gameObject.SetActive(false),
				collectionCheck: false,
				maxSize: 200
			);
		}
	}

	private WeaponProjectile CreateProjectile() {
		var currentPool = Projectiles;

		var projectile = Instantiate(currentWeapon.ProjectilePrefab, null);
		projectile.gameObject.SetActive(false);
		projectile.Died += () => currentPool.Release(projectile);
		return projectile;
	}

	private void GetProjectile(WeaponProjectile projectile) {
		projectile.gameObject.SetActive(true);
		projectile.transform.position = transform.position;
		projectile.Initialize(currentWeapon.Damage + AttackDamageBuff, currentWeapon.ProjectileSpeed, GetClosestTarget());
	}

	private Transform GetClosestTarget() {
		Collider2D collider = null;
		float distance = float.MaxValue;
		foreach (var col in colliders) {
			var dist = Vector2.Distance(Player.Instance.transform.position, col.transform.position);
			if (dist < distance) {
				distance = dist;
				collider = col;
			}
		}
		return collider.transform;
	}

	private async void AutoAttack() {
		// This is not the kind of method I would only use asyn for, but use a coroutine here.
		// I intentionally used a async method instead what could have been a very simply coroutine
		// to show that I know how to use async methods in Unity, which are often considered tricky
		// by several Unity developrs, due to how unity works and handles GO and runs in editor.

		await Task.Delay(Mathf.RoundToInt(currentWeapon.FireInterval * 1000));

		float fireTime;
		while (Player.Instance && Player.Instance.Health > 0 && Application.isPlaying && this != null) {
			if (Physics2D.OverlapCircle(Player.Instance.transform.position, currentWeapon.Range, contactFilter, colliders) > 0) {
				Projectiles.Get();
				fireTime = Time.time;
				while (Time.time < fireTime + currentWeapon.FireInterval - AttackSpeedBuff && Application.isPlaying) {
					await Task.Delay(100);
				}
			}
			else {
				await Task.Yield();
			}
		}
	}

	public void AddAttackSpeedBuff(float buff) {
		AttackSpeedBuff -= buff;
	}

	public void AddAttackDamageBuff(float buff) {
		AttackDamageBuff += buff;
	}

	public void AddWeapon(WeaponData weapon) {
		if (!weapons.Contains(weapon))
			weapons.Add(weapon);
	}

	public bool SwitchWeapon(int index) {
		if (weapons.Count < index + 1)
			return false;

		currentWeapon = weapons[index];
		WeaponSwitched?.Invoke(currentWeapon);
		return true;
	}

	public WeaponData GetCurrentWeapon() {
		return currentWeapon;
	}

}
