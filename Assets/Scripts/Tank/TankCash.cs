using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankCash : MonoBehaviour
{
	public GameObject cashText;
	public int cashAmount;

	void OnTriggerEnter(Collider other)
	{
		cashText.GetComponent<Text>().text = ": " + cashAmount;
	}

}

