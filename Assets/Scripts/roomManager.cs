using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class roomManager : MonoBehaviour
{
	public bool roomComplete = false;
    [SerializeField] private List<GameObject> objectives;
    [SerializeField] private string intro;
    [SerializeField] private Animator dialogueAnim;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private TextMeshProUGUI dialogueBox;
    [SerializeField] private List<string> outroLines;
    [SerializeField] private Animator fade;
    [SerializeField] private int nextScene;
    [SerializeField] private AudioClip finalClip;
    [SerializeField] private AudioSource audioChange;
    [SerializeField] private float outroDuration;
    public bool outroStarted = false;
    //private IsometricActionAsset inputActions;
    [SerializeField] private Door door;

	private void Awake()
	{
        GameObject.Find("StartingFloor").GetComponent<CreateFloor>().generateFloor();
		//inputActions = new IsometricActionAsset();
	}

	private void Start()
	{
        StartCoroutine("introDialogue");
	}

    private IEnumerator introDialogue()
    {
        yield return new WaitForSeconds(1);
        dialogueBox.text = intro;
        playerController.putInDialogue();
        dialogueAnim.SetTrigger("TriggerText");
        dialogueAnim.SetTrigger("Dialogue");
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
                door.roomComplete = true;
            }
        }
        else if (objectives.Count == 0)
        {
            roomComplete = true;
            door.roomComplete = true;
        }
        if (roomComplete)
        {
            roomComplete = true;
            door.roomComplete = true;
        }
        print("room is now finished");
    }
	public void submitPickupObjective(GameObject objective)
	{
		if (objectives != null)
		{
			objectives.Remove(objective);
			
            if (objectives.Count == 0)
			{
				roomComplete = true;
				door.roomComplete = true;
			}
		}
		else if (objectives.Count == 0)
		{
			roomComplete = true;
			door.roomComplete = true;
		}
		if (roomComplete)
		{
			roomComplete = true;
			door.roomComplete = true;
		}
		print("room is now finished");
	}

	/*private void OnEnable()
	{
		inputActions.Player.Interact.started += DoSomething;
		
		
	}
	private void OnDisable()
	{
		inputActions.Player.Interact.started -= DoSomething;
	}

	private void DoSomething(InputAction.CallbackContext context)
	{
        if(roomComplete)
        {

		    print("player interact to outro");
		    outroDialogue();
		    //playerController.descBoxAnim.SetBool("Dialogue", false);
		    playerController.putInDialogue();
        }
	}*/

	public void startOutro()
    {
        print("starting outru");
        outroStarted = true;
        StartCoroutine("fadeOut");
    }

    public void outroDialogue()
    {
        print("somehow getting into here");
        if(outroLines.Count > 0)
        {
            print("outro dialogue here");
		    dialogueBox.text = outroLines[0];
		    playerController.putInDialogue();
		    dialogueAnim.SetTrigger("TriggerText");
		    dialogueAnim.SetTrigger("Dialogue");
            outroLines.RemoveAt(0);

        }
        else
        {
            playerController.deactivateInteraction();
            StartCoroutine("finale");
        }
	}

    private IEnumerator fadeOut()
    {
        fade.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1.5f);
        outroDialogue();
    }

    private IEnumerator finale()
    {
        audioChange.clip = finalClip;
        audioChange.Play();
        audioChange.loop = false;
        yield return new WaitForSeconds(outroDuration);
		SceneManager.LoadScene(nextScene);
	}
}
