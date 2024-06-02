using UnityEngine;


/// <summary>
/// An overly simple player contoller.
/// As there are not a lot of controls I used the legacy Input system as it's faster to setup and wor with.
/// In a game like SUri, I would rather go for the new input system and use the generated C# feature for most things.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour {

	public Vector2 Direction { get; private set; }

	private float speed = 7.5f;
	private new Rigidbody2D rigidbody2D;

	private void Start() {
		if (!gameObject.TryGetComponent<Rigidbody2D>(out rigidbody2D)) {
			Debug.LogError($"Player does not have {nameof(Rigidbody2D)}");
		}
	}

	private void Update() {
		// Right now there's no way to reassign hot keys to a specific weapon.
		// Rather than showing the controls for switching weapons, I just wanted to show
		// how I am managing weapons and switching them in game.
		if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) {
			Player.Instance.WeaponManager.SwitchWeapon(0);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) {
			Player.Instance.WeaponManager.SwitchWeapon(1);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) {
			Player.Instance.WeaponManager.SwitchWeapon(2);
		}
	}

	private void FixedUpdate() {
		Direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
		rigidbody2D.velocity = Direction * speed;
	}

}
