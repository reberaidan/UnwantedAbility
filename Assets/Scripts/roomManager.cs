using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class roomManager : MonoBehaviour
{
	public bool roomComplete = false;
    [SerializeField] private List<GameObject> objectives;
    public Door door;

	private void Awake()
	{
        GameObject.Find("Floor").GetComponent<CreateFloor>().generateFloor();
        door = GameObject.Find("Door").GetComponent<Door>();
	}

    private void finishRoom()
    {
        //SceneManager.LoadScene(0);
    }

    public void submitObjective(GameObject objective)
    {
        if (objectives != null)
        {
            if (objective.GetComponent<InteractContainer>().completed)
            {
                objectives.Remove(objective);
            }
            if (objectives.Count == 0)
            {
                door.roomComplete = true;
            }
        }
        else if (objectives.Count == 0)
        {
            door.roomComplete = true;
        }
        if (roomComplete)
        {
            door.roomComplete = true;
        }
    }
}
