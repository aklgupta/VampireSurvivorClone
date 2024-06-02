using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PlayerAnimationController : MonoBehaviour {

	[SerializeField]
	private PlayerController playerController;

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private Transform weapon;

	[SerializeField]
	private List<AnimationClip> weaponFlipClips;


	void Update() {
		animator.SetFloat("horizontalSpeed", playerController.Direction.x);
		animator.SetFloat("verticalSpeed", playerController.Direction.y);

		var currentClip = animator.GetCurrentAnimatorClipInfo(0)
			.OrderByDescending(x => x.weight)
			.First().clip;

		if (weaponFlipClips.Contains(currentClip)) {
			FlipWeapon();
		}
		else {
			UnflipWeapon();
		}
	}

	public void FlipWeapon() {
		weapon.transform.localScale = new Vector3(-1, 1, 1);
	}

	public void UnflipWeapon() {
		weapon.transform.localScale = new Vector3(1, 1, 1);
	}

}
