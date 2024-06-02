using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Similar to Loot/Drop items.
/// Picking these up will add a new weapon.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class WeaponPickUp : MonoBehaviour {

	public const float RotationSpeed = 50f;

	[field: SerializeField]
	public WeaponData WeaponData { get; private set; }


	private void Update() {
		transform.Rotate(Vector3.forward, RotationSpeed * Time.deltaTime);
	}

}
