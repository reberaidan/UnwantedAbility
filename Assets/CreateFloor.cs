using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreateFloor : MonoBehaviour
{
	[SerializeField] private int width;
	[SerializeField] private int height;
	[SerializeField] private GameObject floor;
	[SerializeField] private float pixelWidth;
	[SerializeField] private float pixelHeight;
	// Start is called before the first frame update
	public void generateFloor()
	{

		//gameObject.GetComponent<SpriteRenderer>().enabled = false;
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				GameObject newFloor = Instantiate(floor, new Vector3(transform.position.x + pixelWidth * i, transform.position.y, transform.position.z - pixelHeight *j), transform.rotation, gameObject.transform);
			}
		}
	}
}
