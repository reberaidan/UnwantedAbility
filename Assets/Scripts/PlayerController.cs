using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

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
	[SerializeField] Animator descBoxAnim;
	private bool inDialogue = false;

	[SerializeField] Camera playerCamera;

	private void Awake()
	{
		rb = this.GetComponent<Rigidbody>();
		actionAsset = new IsometricActionAsset();
	}

	private void OnEnable()
	{
		actionAsset.Player.Interact.started += DoSomething;
		move = actionAsset.Player.Move;
		actionAsset.Player.Enable();
	}


	private void OnDisable()
	{
		actionAsset.Player.Interact.started -= DoSomething;
		actionAsset.Player.Disable();
	}
	private void DoSomething(InputAction.CallbackContext context)
	{
		if (!inDialogue)
		{
			ray = new Ray(interactTrigger.position, direction);
			Debug.DrawRay(interactTrigger.position, direction, Color.green, 60);
			if (Physics.Raycast(ray, out RaycastHit hit, maxInteractionDistance, layersToHit))
			{
				if (hit.collider.gameObject.CompareTag("item"))
				{
					descriptionBox.text = hit.collider.gameObject.GetComponent<itemDescription>().getDescription();
					descBoxAnim.SetTrigger("TriggerText");
					descBoxAnim.SetBool("Dialogue", true);
					inDialogue = true;
					stopMovement();
				}
			}
		}
		else
		{
			descBoxAnim.SetBool("Dialogue", false);
			inDialogue = false;
			startMovement();
		}

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
				}
				else
				{
					direction = Vector3.right;
				}
			}
			else
			{
				if (move.ReadValue<Vector2>().y < 0)
				{
					direction = Vector3.back;
				}
				else
				{
					direction = Vector3.forward;
				}
			}
		}
		else
		{
			rb.angularVelocity = Vector3.zero;
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
		rb.constraints = RigidbodyConstraints.None;
		rb.constraints = RigidbodyConstraints.FreezeRotation;
		rb.constraints = RigidbodyConstraints.FreezePositionY;
	}
}
