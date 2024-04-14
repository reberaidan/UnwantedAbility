using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public void playGame()
    {
        StartCoroutine("loadLevel");
    }

    private IEnumerator loadLevel()
    {
        animator.SetTrigger("fade");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
    }

    public void exitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
