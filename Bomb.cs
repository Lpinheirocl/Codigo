
/*
 * Copyright (c) 2017 Razeware LLC
 */


using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class Bomb : MonoBehaviour
{
	public AudioClip explosionSound;
	public GameObject explosionPrefab;
	public LayerMask levelMask;
	// Essa Mascara garante que que os rays verifiquem os espaços vazios para atigirem os blocos.
	private bool exploded = false;

	// Use para inicialização
	void Start ()
	{
		Invoke ("Explode", 3f); //Chama Explode em 3 segundos
	}

	void Explode ()
	{
		//Som da explosão
		AudioSource.PlayClipAtPoint (explosionSound, transform.position);

		//Cria a primeira explosão na posição da bomba
		Instantiate (explosionPrefab, transform.position, Quaternion.identity);

		//Para cada direção cria uma sequencia de explosões.
		StartCoroutine (CreateExplosions (Vector3.forward));
		StartCoroutine (CreateExplosions (Vector3.right));
		StartCoroutine (CreateExplosions (Vector3.back));
		StartCoroutine (CreateExplosions (Vector3.left));

		GetComponent<MeshRenderer> ().enabled = false; //Desabilita mesh
		exploded = true; 
		transform.Find ("Collider").gameObject.SetActive (false); //Desabilita o collider
		Destroy (gameObject, .3f); //Destroi a bomba atual em 0.3 segundos depois de todas as subrotinas terem finalizado.
	}

	public void OnTriggerEnter (Collider other)
	{
		if (!exploded && other.CompareTag ("Explosion"))
		{ //Se não explodiu ainda a a bomba é atingida por uma explosão...
			CancelInvoke ("Explode"); //Cancela a explosão e talvez a bomba explosa 2 vezes
			Explode (); //Explosão
		}
	}

	private IEnumerator CreateExplosions (Vector3 direction)
	{
		for (int i = 1; i < 3; i++)
		{ //O 3 determina o quão longe o raycast vai checar, nesse caso 3 tiles.
			RaycastHit hit; //Guarda toda a informação sobre o acerto do raycast

			Physics.Raycast (transform.position + new Vector3 (0, .5f, 0), direction, out hit, i, levelMask); //Raycast na direção expecifica na distancia i, por que a mascara da layer só vai atingir os blocos, não os jogadores ou bombas.

			if (!hit.collider)
			{ // Espaço livre, faz uma nova explosão.
				Instantiate (explosionPrefab, transform.position + (i * direction), explosionPrefab.transform.rotation);
			} else
			{ // Ao acertar um bloco para de spamar naquela direção
				break;
			}

			yield return new WaitForSeconds (.05f); //Espera 50 milesegundos antes de checar a próxima localização
		}

	}
}
