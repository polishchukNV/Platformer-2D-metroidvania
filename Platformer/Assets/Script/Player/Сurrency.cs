using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Сurrency : MonoBehaviour
{
	[SerializeField] AudioSource audioSource;
	[SerializeField] float range;
	[SerializeField] float normalizedDistance;
	[SerializeField] float volume;

	private void Start()
	{
		if (audioSource == null)
		{
			audioSource = GetComponent<AudioSource>();
		}
	}

	private void OnDrawGizmosSelected()
	{
        Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, range);
	}

	private void Update()
	{
		normalizedDistance = range / (Mathf.Abs(NewPlayer.Instance.transform.position.x - transform.position.x));
		if (normalizedDistance <= 1)
		{
			audioSource.volume = normalizedDistance;
		}
	}
}
