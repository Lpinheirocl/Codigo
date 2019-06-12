using UnityEngine;
using System.Collections;
using System;

public class Destrua : MonoBehaviour
{
	public bool dead = false;


	public void OnTriggerEnter (Collider other) ///Ao colidir com outro Collider, o outro objeto é armazenado na memória com o ID "other"
	{
		if (!dead && other.CompareTag ("Explosion"))
		{ //vivo & atingido pela explosão
			Debug.Log ("P hit by explosion!");

			dead = true;

			Destroy (gameObject);
		}
	}

}
