using UnityEngine;


/// <summary>
/// Collecting it releases several low damage magic orbs.
/// </summary>
public class MagicBomb : LootDrop {

	private const float ProjectileDamage = 5f;
	private const float ProjectileSpeed = 50f;

	[SerializeField]
	private WeaponProjectile projectilePrefab;

	public override void Collect() {
		base.Collect();

		Amount = Mathf.CeilToInt(Amount);
		var randomRotation = Random.value * 360f * Mathf.Deg2Rad;

		// Emit Amount number of orbs in a circle, starting at a random angle.
		for (int i = 0; i < Amount; i++) {
			var dir = new Vector2(
				Mathf.Sin(Mathf.Deg2Rad * i * 360f / Amount + randomRotation),
				Mathf.Cos(Mathf.Deg2Rad * i * 360f / Amount + randomRotation)
			);

			// I am not using a object pool here due to time constraints and low probabilty of this loot item
			var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity, null);
			projectile.Initialize(ProjectileDamage, ProjectileSpeed, dir);
			projectile.Died += () => Destroy(projectile.gameObject);
		}
	}

}
