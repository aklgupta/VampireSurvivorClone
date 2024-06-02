using UnityEngine;


public class FreezeWeaponProjectile : WeaponProjectile {

	private const float FreezeTime = 2f;

	private void Start() {
		Hit += FreezeEnemy;
	}

	private void FreezeEnemy(Collider2D enemyCollider) {
		enemyCollider.GetComponent<Enemy>().ApplyFreeze(FreezeTime);
	}

}
