using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class roomManager : MonoBehaviour
{
    public bool roomComplete = false;
    [SerializeField] private List<GameObject> objectives;
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
                roomComplete = true;
            }
        }
        else if (objectives.Count == 0)
        {
            roomComplete = true;
        }
        if (roomComplete)
        {
            finishRoom();
        }
    }
}
