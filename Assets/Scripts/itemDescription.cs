using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemDescription : MonoBehaviour
{
	[SerializeField] private string description;
	[SerializeField] private Sprite inventoryImage;

	public string getDescription()
	{
		return description;
	}

	public Sprite getInventoryImage()
	{
		return inventoryImage;
	}
}
