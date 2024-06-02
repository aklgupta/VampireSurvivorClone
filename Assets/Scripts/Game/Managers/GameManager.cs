using System;
using UnityEngine;


/// <summary>
/// A very simple Game Manager to check win condition and used by other scripts to puase/resume the game.
/// Ideally I would prefer multiple managers for different aspects, but as the game is very simple,
/// I went for just a single manager.
/// </summary>
public class GameManager : MonoBehaviour {

	public const float TimeToSurvive = 300f;

	public static GameManager Instance { get; private set; }
	public float TimeLeftToSurvive { get; private set; } = TimeToSurvive;

	private float normalTimeScale; // To restore the time scale before pause, as it might not always be 1

	public event Action GameWon;


	private void Awake() {
		if (Instance != null) {
			Destroy(gameObject);
			return;
		}
		TimeLeftToSurvive = TimeToSurvive;
		Instance = this;
		normalTimeScale = Time.timeScale;
	}

	private void Update() {
		TimeLeftToSurvive -= Time.deltaTime;
		if (TimeLeftToSurvive < 0) {
			TimeLeftToSurvive = 0;
			GameWon?.Invoke();
		}
	}

	public void PauseGame() {
		if (Time.timeScale == 0)
			return;

		normalTimeScale = Time.timeScale;
		Time.timeScale = 0;
	}

	public void ResumeGame() {
		Time.timeScale = normalTimeScale;
	}

}
