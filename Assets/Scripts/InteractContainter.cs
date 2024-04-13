using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractContainer : MonoBehaviour
{
    [SerializeField] private GameObject expectedObject;
    [SerializeField] private string dialogue;
    [SerializeField] private Mesh changeMesh;
    [SerializeField] private roomManager roomManager;
    [SerializeField] private Sprite expectedSprite;
    public bool completed;
    
    public string getDialogue()
    {
        return dialogue;
    }

    public GameObject getExpected()
    {
        return expectedObject;
    }

    public Sprite getExpectedSprite()
    {
        return expectedSprite;
    }

    public void changeState()
    {
        gameObject.GetComponent<MeshFilter>().mesh = changeMesh;
        completed = true;
        roomManager.submitObjective(gameObject);
    }

}
