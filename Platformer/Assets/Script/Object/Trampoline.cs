using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{

	public bool customSpeed;
	public Vector2 customVelocity;
	public float multiplier;

	private bool onTop;
	public GameObject bouncer;
	private Animator animator;
	private Vector2 velocity;

	private void Start()
	{
		animator = gameObject.GetComponent<Animator>();
	}

	private void OnCollisionStay2D(Collision2D other)
	{

		if (onTop)
		{
			//animator.SetBool("isStepped", true);
			bouncer = other.gameObject;
		}

		StartCoroutine(JSqueeze(1.3f,0.4f,0.2f));
		Jump();
	}

	private void OnTriggerEnter2D()
	{
		onTop = true;
	}

	private void OnTriggerExit2D()
	{
		onTop = false;
		//animator.SetBool("isStepped", false);
	}

	private void OnTriggerStay2D()
	{
		onTop = true;
	}

	private void Jump()
	{
		if (customSpeed)
		{
			velocity = customVelocity;
		}
		else
		{
			velocity = transform.up * multiplier;
		}

		if (bouncer.GetComponent<Rigidbody2D>() != null && bouncer != null)
		bouncer.GetComponent<Rigidbody2D>().velocity = velocity;

	}

	IEnumerator JSqueeze(float xSqueeze, float ySqueeze, float seconds)
	{
		Vector3 originalSize = Vector3.one;
		Vector3 newSize = new Vector3(xSqueeze, ySqueeze, originalSize.z);
		float t = 0f;
		while (t <= 1.0)
		{
			t += Time.deltaTime / seconds;
			gameObject.transform.localScale = Vector3.Lerp(originalSize, newSize, t);
			yield return null;
		}
		t = 0f;
		while (t <= 1.0)
		{
			t += Time.deltaTime / seconds;
			gameObject.transform.localScale = Vector3.Lerp(newSize, originalSize, t);
			yield return null;
		}

	}
}

