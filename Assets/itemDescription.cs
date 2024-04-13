using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemDescription : MonoBehaviour
{
	[SerializeField] private string description;

	public string getDescription()
	{
		return description;
	}
}
