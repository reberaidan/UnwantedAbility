using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InteractContainer : MonoBehaviour
{
    [SerializeField] private List<GameObject> expectedObject;
    [SerializeField] private string dialogue;
    [SerializeField] private string stashText;
    [SerializeField] private Mesh changeMesh;
    [SerializeField] private roomManager roomManager;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    private List<Sprite> expectedSprite = new List<Sprite>();
    public bool completed;

	private void Start()
	{
		foreach (GameObject obj in expectedObject)
        {
            expectedSprite.Add(obj.GetComponent<SpriteRenderer>().sprite);
        }
	}

	public string getDialogue()
    {
        return dialogue;
    }

    public List<GameObject> getExpected(List<GameObject> inventory)
    {
        var list = new List<GameObject>(); 
        foreach (var item in inventory)
        {
            if (expectedObject.Contains(item))
            {
                list.Add(item);
            }
        }
        
        expectedObject = expectedObject.Except(list).ToList();
        changePickupText(list);
        return list;
    }

    public List<Sprite> getExpectedSprite(List<GameObject> inventory)
    {
		var list = new List<Sprite>();
		foreach (GameObject item in inventory)
		{
			if (expectedSprite.Contains(item.GetComponent<SpriteRenderer>().sprite))
			{
				list.Add(item.GetComponent<SpriteRenderer>().sprite);
			}

		}

	    expectedSprite = expectedSprite.Except(list).ToList();

		return list;
	}

    private void changePickupText(List<GameObject> list)
    {
        string changeText = stashText;
        

        foreach (var item in list)
        {
            changeText += item.name;
            if(item != list[list.Count - 1])
            {
                changeText += ", ";
            }
        }
        textMeshProUGUI.text = changeText;
    }
/*
    public void removeExpected(List<GameObject> list)
    {
		foreach (var item in expectedObject)
		{
			if (list.Contains(item))
			{
				expectedObject.Remove(item);
			}
		}
	}

    public void removeExpectedSprite(List<GameObject> list)
    {
		foreach (var item in expectedSprite)
		{
			if (list.Contains(item))
			{
				expectedSprite.Remove(item);
			}
		}
	}*/

    public void changeState()
    {
        if(expectedObject.Count == 0)
        {
            print("changing state");
            completed = true;
            roomManager.submitObjective(gameObject);
        }
    }

}
