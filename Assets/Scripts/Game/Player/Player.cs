using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// Quite a simple and straight forward script. Hold information about player stats
/// and manages collitions (damage, pick ups, etc)
/// </summary>
[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour {

	public static Player Instance { get; private set; }

	public const int MaxLevel = 10;

	public WeaponManager WeaponManager { get; private set; }
	public float MaxHealth { get; private set; } = 100;
	public int CurrentLevel { get; private set; } = 1;

	private float _health = 100;
	public float Health {
		get => _health;
		private set {
			var oldHealth = _health;
			_health = Mathf.Clamp(value, 0, MaxHealth);
			HealthChanged?.Invoke(oldHealth, _health);
			if (_health == 0) {
				gameObject.SetActive(false);
				Died?.Invoke();
			}
		}
	}
	private float _xp = 0;
	public float XP {
		get => _xp;
		private set {
			var oldXp = _xp;
			_xp = Mathf.Clamp(value, 0f, xpLevels[MaxLevel]);

			if (oldXp == _xp)
				return;

			UpdateCurrentLevel();
			XpChanged?.Invoke(oldXp, _xp);
		}
	}

	private Dictionary<int, float> xpLevels;

	public event Action Died;
	public event Action LeveledUp;
	public event Action<float, float> XpChanged;
	public event Action<float, float> HealthChanged;


	private void Awake() {
		if (Instance != null) {
			Destroy(gameObject);
			return;
		}

		Instance = this;
	}

	private void Start() {
		InitializeXpLevels();
		WeaponManager = GetComponentInChildren<WeaponManager>();
	}

	private void InitializeXpLevels() {
		xpLevels = new();
		float totalXp = 0;
		for (int i = 1; i < MaxLevel + 1; i++) {
			totalXp += i * 100f;
			xpLevels[i] = totalXp;
		}
	}

	private void UpdateCurrentLevel() {
		var lastLevel = CurrentLevel;
		foreach (var kvp in xpLevels.OrderBy(x => x.Key)) {
			if (XP < kvp.Value) {
				CurrentLevel = kvp.Key;
				break;
			}
		}

		if (lastLevel != CurrentLevel)
			LeveledUp?.Invoke();
	}

	public void AddXP(float xp) {
		XP += xp;
	}

	public void AddHealth(float health) {
		Health += health;
	}

	public void IncreaseMaxHealth(float amount, bool healToMaxHealth = true) {
		MaxHealth += amount;
		Health = healToMaxHealth ? MaxHealth : Health;
	}

	public float GetXpToLevel(int level) {
		return xpLevels[level];
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.TryGetComponent<Enemy>(out var enemy)) {
			Health -= enemy.GetDamage();
		}
		else if(collision.TryGetComponent<WeaponPickUp>(out var weaponPickUp)) {
			WeaponManager.AddWeapon(weaponPickUp.WeaponData);
			Destroy(weaponPickUp.gameObject);
		}
	}

}
