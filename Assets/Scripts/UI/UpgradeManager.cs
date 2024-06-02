using UnityEngine;

public class UpgradeManager : MonoBehaviour {

	private const float AttackSpeedBuff = 0.2f;
	private const float AttackDamageBuff = 5f;
	private const float PlayerHealthBuff = 20f;

	private Player Player => Player.Instance;

	[SerializeField]
	private UpgradeChoice choicePrefab;
	[SerializeField]
	private Transform choicePanel;


	void Start() {
		UpgradeChoice choice;

		choice = Instantiate(choicePrefab, choicePanel);
		choice.SetDescription("Increase Attack Speed");
		choice.AddListener(IncreaseAttackSpeed);

		choice = Instantiate(choicePrefab, choicePanel);
		choice.SetDescription("Increase Damage");
		choice.AddListener(IncreaseDamage);

		choice = Instantiate(choicePrefab, choicePanel);
		choice.SetDescription("Increase Max Health");
		choice.AddListener(IncreaseHealth);

		Player.LeveledUp += OnLevelUp;

		gameObject.SetActive(false);
	}

	private void OnEnable() {
		GameManager.Instance.PauseGame();
	}


	private void OnLevelUp() {
		gameObject.SetActive(true);
		Player.AddHealth(Player.MaxHealth);
	}

	private void IncreaseAttackSpeed() {
		Player.WeaponManager.AddAttackSpeedBuff(AttackSpeedBuff);
		gameObject.SetActive(false);
	}

	private void IncreaseDamage() {
		Player.WeaponManager.AddAttackDamageBuff(AttackDamageBuff);
		gameObject.SetActive(false);
	}

	private void IncreaseHealth() {
		Player.IncreaseMaxHealth(PlayerHealthBuff);
		gameObject.SetActive(false);
	}

	private void OnDisable() {
		GameManager.Instance.ResumeGame();
	}

	private void OnDestroy() {
		Player.LeveledUp -= OnLevelUp;
	}
}
