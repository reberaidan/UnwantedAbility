using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemDescription : MonoBehaviour
{
	[SerializeField] private string description;
	[SerializeField] private Sprite inventoryImage;
	[SerializeField] private bool isObjective;
	[SerializeField] private bool tunnelVision;

	public string getDescription()
	{
		return description;
	}

	public Sprite getInventoryImage()
	{
		return inventoryImage;
	}

	public bool getObjective()
	{
		return isObjective;
	}

	public bool getTunnelVision()
	{
		return tunnelVision;
	}
}
