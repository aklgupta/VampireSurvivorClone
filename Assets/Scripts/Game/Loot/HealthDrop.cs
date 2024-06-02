using UnityEngine;

public class HealthDrop : LootDrop {

	public override void Collect() {
		Player.Instance.AddHealth(Amount);
		base.Collect();
	}

}
