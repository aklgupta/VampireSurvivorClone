using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour {

	public void Play() {
		SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
	}

	public void Quit() {
#if UNITY_EDITOR
		UnityEditor.EditorApplication.ExitPlaymode();
#else
		Application.Quit();
#endif
	}

}
