using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CreateFloor : MonoBehaviour
{
	[SerializeField] private int width;
	[SerializeField] private int height;
	[SerializeField] private GameObject floor;
	// Start is called before the first frame update
	public void generateFloor()
	{
		Sprite sprite = GetComponent<Sprite>();
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				GameObject newFloor = Instantiate(floor, new Vector3(transform.position.x + 1.28f * i, transform.position.y, transform.position.z - 1.28f *j), transform.rotation, gameObject.transform);
			}
		}
	}
}
