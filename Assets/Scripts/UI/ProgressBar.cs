using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

	[SerializeField]
	private Image foregroundImage;

	[SerializeField]
	private TMPro.TMP_Text progressLabel;

	public void SetLabel(string text) {
		progressLabel.text = text;
	}

	public void SetProgress(float progress) {
		foregroundImage.fillAmount = progress;
	}

}
