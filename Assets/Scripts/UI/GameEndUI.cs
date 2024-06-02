using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndUI : MonoBehaviour {

	[SerializeField]
	private TMPro.TMP_Text resultText;

	private void Start() {
		gameObject.SetActive(false);
		Player.Instance.Died += ShowRetryPanel;
		GameManager.Instance.GameWon += ShowGameWonPanel;
	}

	private void OnEnable() {
		GameManager.Instance.PauseGame();
	}

	private void ShowRetryPanel() {
		resultText.text = "You Lost!";
		gameObject.SetActive(true);
	}

	private void ShowGameWonPanel() {
		resultText.text = "You Won!";
		gameObject.SetActive(true);
	}

	public void Restart() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void Quite() {
		SceneManager.LoadScene(0);
	}

	private void OnDisable() {
		GameManager.Instance.ResumeGame();
	}

	private void OnDestroy() {
		Player.Instance.Died -= ShowRetryPanel;
		GameManager.Instance.GameWon -= ShowGameWonPanel;
	}

}
