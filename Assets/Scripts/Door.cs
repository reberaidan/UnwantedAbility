using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public bool roomComplete = false;
    [SerializeField] private int nextScene;
    public string text;
    public bool canLeave()
    {
        if (roomComplete)
        {
            SceneManager.LoadScene(nextScene);
        }
        return roomComplete;
    }
}
