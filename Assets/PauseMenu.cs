using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private bool startState;

	private void Start()
	{
		gameObject.SetActive(startState);
	}
	// Start is called before the first frame update
	public void Resume()
    {
        playerController.Unpause();
        gameObject.SetActive(false);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

	public void Quit()
	{
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
