using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public bool roomComplete = false;
    [SerializeField] private int nextScene;
    [SerializeField] private roomManager roomManager;
    public string text;
    public bool canLeave()
    {
        
        return roomComplete;
    }

    public void leave()
    {
	
	}
}
