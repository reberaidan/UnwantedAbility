using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractContainer : MonoBehaviour
{
    [SerializeField] private GameObject expectedObject;
    [SerializeField] private string dialogue;
    [SerializeField] private Mesh changeMesh;
    [SerializeField] private roomManager roomManager;
    public bool completed;
    
    public string getDialogue()
    {
        return dialogue;
    }

    public GameObject getExpected()
    {
        return expectedObject;
    }

    public void changeState()
    {
        gameObject.GetComponent<MeshFilter>().mesh = changeMesh;
        completed = true;
        roomManager.submitObjective(gameObject);
    }

}
