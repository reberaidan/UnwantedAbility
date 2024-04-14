using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Runtime.InteropServices;

public class PlayerController : MonoBehaviour
{
	//input fields
	private IsometricActionAsset actionAsset;
	private InputAction move;

	//movement fields
	private Rigidbody rb;
	[SerializeField] private Transform interactTrigger;
	[SerializeField] private float movementForce = 1f;
	[SerializeField] private float maxSpeed = 5f;
	private Vector3 forceDirection = Vector3.zero;
	private Vector3 direction = Vector3.right;
	private Ray ray;
	[SerializeField] float maxInteractionDistance;
	public LayerMask layersToHit;
	[SerializeField] TextMeshProUGUI descriptionBox;
	[SerializeField] public Animator descBoxAnim;
	private bool inDialogue = false;
	[SerializeField] InventoryOverlay inventoryUI;
	[SerializeField] GameObject pauseMenu;
	[SerializeField] roomManager roomManager;

	private List<GameObject> inventory = new List<GameObject> ();

	[SerializeField] Camera playerCamera;
	private bool pickup;
	private bool containerDeposit;
	private Animator playerAnimator;

	public bool paused = false;

	private void Awake()
	{
		rb = this.GetComponent<Rigidbody>();
		playerAnimator = this.GetComponent<Animator> ();
		actionAsset = new IsometricActionAsset();
	}

	private void OnEnable()
	{
		actionAsset.Player.Interact.started += DoSomething;
		actionAsset.Player.Pause.started += DoPause;
		move = actionAsset.Player.Move;
		actionAsset.Player.Enable();
	}


	private void OnDisable()
	{
		actionAsset.Player.Interact.started -= DoSomething;
		actionAsset.Player.Pause.started -= DoPause;
		actionAsset.Player.Disable();
	}

	private void DoPause(InputAction.CallbackContext context)
	{
		if (paused)
		{
			pauseMenu.SetActive(!paused);
			paused = !paused;
			actionAsset.Player.Move.Enable();
			actionAsset.Player.Interact.Enable();
		}
		else
		{
			pauseMenu.SetActive(!paused);
			paused = !paused;
			actionAsset.Player.Move.Disable();
			actionAsset.Player.Interact.Disable();
		}
	}

	public void Unpause()
	{
		pauseMenu.SetActive(!paused);
		paused = !paused;
		actionAsset.Player.Move.Enable();
		actionAsset.Player.Interact.Enable();
	}

	private void DoSomething(InputAction.CallbackContext context)
	{
		
		if (!inDialogue && !pickup && !containerDeposit && !paused )
		{
			ray = new Ray(interactTrigger.position, direction);
			Debug.DrawRay(interactTrigger.position, direction * maxInteractionDistance, Color.green, 60);
			if (Physics.Raycast(ray, out RaycastHit hit, maxInteractionDistance, layersToHit))
			{
				//generic item description
				if (hit.collider.gameObject.CompareTag("item"))
				{
					var hitScript = hit.collider.gameObject.GetComponent<itemDescription>();
					descriptionBox.text = hitScript.getDescription();
					descBoxAnim.SetTrigger("TriggerText");
					descBoxAnim.SetBool("Dialogue", true);
					inDialogue = true;
					stopMovement();
				}
				// if item needs to be picked up.
				else if (hit.collider.gameObject.CompareTag("pickup"))
				{
					var hitScript = hit.collider.gameObject.GetComponent<itemDescription>();
					descriptionBox.text = hitScript.getDescription();
					descBoxAnim.SetTrigger("TriggerText");
					descBoxAnim.SetBool("Dialogue", true);
					descBoxAnim.SetBool("PickUp", true);
					inDialogue = true;
					pickup = true;
					stopMovement();
					inventory.Add(hit.collider.gameObject);
					inventoryUI.addToInventory(hitScript.getInventoryImage());
					if (hitScript.getObjective())
					{
						roomManager.submitPickupObjective(hit.collider.gameObject);
					}
					hit.collider.gameObject.SetActive(false);
				}
				else if (hit.collider.gameObject.CompareTag("container"))
				{
					var hitScript = hit.collider.gameObject.GetComponent<InteractContainer>();
					descriptionBox.text = hitScript.getDialogue();
					descBoxAnim.SetTrigger("TriggerText");
					descBoxAnim.SetBool("Dialogue", true);
					inDialogue = true;
					stopMovement();
					var deposits = hitScript.getExpected(inventory);
					//var UIdeposits = hitScript.getExpectedSprite(inventoryUI.getInventory());
					foreach (var depositsItem in deposits)
					{
						if (inventory.Contains(depositsItem))
						{
							inventory.Remove(depositsItem);
							descBoxAnim.SetBool("DepositItem", true);
							containerDeposit = true;
							hitScript.changeState();
							inventoryUI.removeInventory(depositsItem.GetComponent<SpriteRenderer>().sprite);
						}

					}
					print("items deposited");
				}
				else if (hit.collider.gameObject.CompareTag("door"))
				{
					print("interacting with door");
					var hitScript = hit.collider.gameObject.GetComponent<Door>();
					if (!hitScript.canLeave())
					{
						descriptionBox.text = hitScript.text;
						descBoxAnim.SetTrigger("TriggerText");
						descBoxAnim.SetBool("Dialogue", true);
						inDialogue = true;
						stopMovement();
					}
					else { roomManager.startOutro(); }
				}
			}
		}
		else
		{
			
			if(!paused)
			{
				print("trying to get out of dialogues");
				if (inDialogue)
				{
					descBoxAnim.SetBool("Dialogue", false);
					inDialogue = false;
					if (!pickup && !containerDeposit)
					{
						startMovement();
					}
					if (roomManager.outroStarted)
					{
						roomManager.outroDialogue();
					}
				}
				else if(pickup){
					descBoxAnim.SetBool("PickUp", false);
					pickup = false;
					if(!containerDeposit && !inDialogue)
					{
						startMovement();
					}
				}
				else if (containerDeposit)
				{
					descBoxAnim.SetBool("DepositItem", false);
					containerDeposit = false;
					if(!pickup && !inDialogue)
					{
						startMovement();
					}
				}

			}

		}

	}

	public void putInDialogue()
	{
		inDialogue = true;
		actionAsset.Player.Move.Disable();
	}

	private void FixedUpdate()
	{
		forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
		forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;

		rb.AddForce(forceDirection, ForceMode.Impulse);
		forceDirection = Vector3.zero;

		Vector3 horizontalVelocity = rb.velocity;
		horizontalVelocity.y = 0;
		if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
		{
			rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
		}
		LookAt();
	}

	private void LookAt()
	{

		if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
		{
			bool horizontal = false;
			if (Math.Abs(move.ReadValue<Vector2>().y) < Math.Abs(move.ReadValue<Vector2>().x))
			{
				horizontal = true;
			}
			if (horizontal)
			{

				if (move.ReadValue<Vector2>().x < 0)
				{
					direction = Vector3.left;
					if (!inDialogue)
					{
						playerAnimator.SetTrigger("Left");
					}
				}
				else
				{
					direction = Vector3.right;
					if (!inDialogue)
					{
						playerAnimator.SetTrigger("Right");
					}
				}
			}
			else
			{
				if (move.ReadValue<Vector2>().y < 0)
				{
					//different because of world space direction
					direction = Vector3.back;
					if (!inDialogue) { 
						playerAnimator.SetTrigger("Front");
					}
				}
				else
				{
					//different because of world space direction
					direction = Vector3.forward;
					if (!inDialogue)
					{
						playerAnimator.SetTrigger("Back");
					}
				}
			}
		}
		else
		{
			rb.angularVelocity = Vector3.zero;
			playerAnimator.SetTrigger("Idle");
		}
	}

	private Vector3 GetCameraForward(Camera playerCamera)
	{
		Vector3 forward = playerCamera.transform.forward;
		forward.y = 0;
		return forward.normalized;
	}

	private Vector3 GetCameraRight(Camera playerCamera)
	{
		Vector3 right = playerCamera.transform.right;
		right.y = 0;
		return right.normalized;
	}

	private void stopMovement()
	{
		rb.constraints = RigidbodyConstraints.FreezeAll;
	}

	private void startMovement()
	{
		rb.constraints = RigidbodyConstraints.FreezePositionY;
		rb.freezeRotation = true;
		actionAsset.Player.Move.Enable();
	}
}
