using System.Collections;
using UnityEngine;


/// <summary>
/// Manages the UI for stats.
/// </summary>
public class Stats : MonoBehaviour {

	[SerializeField]
	private ProgressBar health;

	[SerializeField]
	private ProgressBar xpLevel;

	[SerializeField]
	private TMPro.TMP_Text timeLeftLabel;

	[SerializeField]
	private TMPro.TMP_Text currentWeaponLabel;

	private Player Player => Player.Instance;


	private IEnumerator Start() {
		yield return null;

		health.SetProgress(1);
		UpdateHealth(0, Player.Health);
		xpLevel.SetLabel($"XP: 0");
		UpdateXp(0, 0);

		Player.HealthChanged += UpdateHealth;
		Player.XpChanged += UpdateXp;
		Player.WeaponManager.WeaponSwitched += UpdateWeaponLabel;
		UpdateWeaponLabel(Player.WeaponManager.GetCurrentWeapon());
	}

	private void Update() {
		timeLeftLabel.text = $"Time left: {GameManager.Instance.TimeLeftToSurvive:0.0}s";
	}

	private void UpdateHealth(float _, float curHealth) {
		var healthFraction = curHealth / Player.MaxHealth;
		health.SetProgress(healthFraction);
		health.SetLabel($"Health: {healthFraction * 100:0.00}% ({curHealth}/{Player.MaxHealth})");
	}

	private void UpdateXp(float _, float curXp) {
		float lastLevelXp = 0;
		if (Player.CurrentLevel > 1) {
			lastLevelXp = Player.GetXpToLevel(Player.CurrentLevel - 1);
		}

		if (Player.CurrentLevel < Player.MaxLevel) {
			float xpFraction = (curXp - lastLevelXp) / (Player.GetXpToLevel(Player.CurrentLevel) - lastLevelXp);
			xpLevel.SetProgress(xpFraction);
			xpLevel.SetLabel($"Level: {Player.CurrentLevel} | XP to next Level: {xpFraction * 100:0.00}%");
		}
		else {
			xpLevel.SetProgress(1);
			xpLevel.SetLabel($"Level: {Player.CurrentLevel} (Max Level)");
		}
	}

	private void UpdateWeaponLabel(WeaponData weaponData) {
		// As I didn't have the time to create a proper UI, displaying the currely equipped weapon as text.
		currentWeaponLabel.text = weaponData.WeaponName;
	}

	private void OnDestroy() {
		Player.HealthChanged -= UpdateHealth;
		Player.XpChanged -= UpdateXp;
	}
}
