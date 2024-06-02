using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class UpgradeChoice : MonoBehaviour {

	[SerializeField]
	private TMPro.TMP_Text description;

	private Button button;

	private void Awake() {
		button = GetComponent<Button>();
	}

	public void SetDescription(string text) {
		description.text = text;
	}

	public void AddListener(UnityAction action) {
		button.onClick.AddListener(action);
	}

	public void RemoveListener(UnityAction action) {
		button.onClick.RemoveListener(action);
	}

}
