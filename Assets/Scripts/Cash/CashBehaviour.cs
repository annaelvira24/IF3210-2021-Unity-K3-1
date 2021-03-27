using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashBehaviour : MonoBehaviour
{
	public AudioSource collectSound;

	void OnTriggerEnter(Collider other)
	{
        collectSound.Play();
        other.GetComponent<TankCash>().cashAmount += 1;
		Destroy(gameObject);
	}

}
