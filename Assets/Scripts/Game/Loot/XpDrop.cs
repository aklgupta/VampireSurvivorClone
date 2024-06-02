using UnityEngine;


public class XpDrop : LootDrop {

	private const float BaseXp = 12.5f;
	private const float BaseScale = 10f;

	protected override void SetAmount(float value) {
		base.SetAmount(value);
		transform.localScale = (1 + Mathf.InverseLerp(1, 10, Amount)) * BaseScale * Vector3.one;
	}

	public override void Collect() {
		Player.Instance.AddXP(Amount * BaseXp);
		base.Collect();
	}

}
