using UnityEngine;

[CreateAssetMenu]
public class WeaponData : ScriptableObject {

	[field: SerializeField]
	public string WeaponName { get; private set; } // Used in the HUD

	[field: SerializeField]
	public WeaponProjectile ProjectilePrefab { get; private set; }

	[field: SerializeField]
	public float Damage { get; private set; }

	[field: SerializeField]
	public float FireInterval { get; private set; }

	[field: SerializeField]
	public float Range { get; private set; }

	[field: SerializeField]
	public float ProjectileSpeed { get; private set; }

}
