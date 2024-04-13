using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryOverlay : MonoBehaviour
{
    private List<GameObject> inventory = new List<GameObject>();
    [SerializeField] private GameObject inventoryPrefab;

    public void addToInventory(Sprite spr)
    {
        Vector3 position = new Vector3(gameObject.transform.position.x + (inventory.Count * 50),gameObject.transform.position.y,gameObject.transform.position.z);
        Quaternion rotation = Quaternion.identity;
        Transform parent = gameObject.transform;

        GameObject newChild = Instantiate(inventoryPrefab, position, rotation, parent);
        newChild.GetComponent<Image>().sprite = spr;
        inventory.Add(newChild);
    }

    public void removeInventory(Sprite spr) 
    { 
        foreach (GameObject child in inventory)
        {
            if(child.GetComponent<Image>().sprite == spr)
            {
                inventory.Remove(child);
                Destroy(child);
                break;
            }
        }

        foreach(GameObject child in inventory)
        {
            child.transform.position = new Vector3(transform.position.x + 50 * inventory.IndexOf(child),transform.position.y,transform.position.z);
        }
    }
}
