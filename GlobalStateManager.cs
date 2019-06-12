/*
 * Copyright (c) 2017 Razeware LLC
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GlobalStateManager : MonoBehaviour
{
	public List<GameObject> Players = new List<GameObject> ();
	public string NameScene;
    private int deadPlayers = 0;
    private int deadPlayerNumber = -1;
	public GameObject Win; 
	public GameObject Win2; 
	public GameObject Draw;



    public void PlayerDied (int playerNumber)
    {
        deadPlayers++;

        if (deadPlayers == 1)
        {
            deadPlayerNumber = playerNumber;
            Invoke ("CheckPlayersDeath", .3f);
        }
    }

    void CheckPlayersDeath ()
    {
        if (deadPlayers == 1)
        { //se um jogador tiver morto, o outro automaticamente ganha

            if (deadPlayerNumber == 1)
            { //P1 morreu, P2 é o vencedor
                Debug.Log ("Player 2 é o ganhador!");
				Win2.SetActive (true);
				StartCoroutine ("LoadScene");


            } else
            { //P2 morreu, P1 é o vencedor
                Debug.Log ("Player 1 é o ganhador!");
				Win.SetActive (true);
				StartCoroutine ("LoadScene");
            }
        } else
        {  //mortes multiplas é empate
            Debug.Log ("Empate");
			Draw.SetActive (true);
			StartCoroutine ("LoadScene");
		}



	}
	IEnumerator LoadScene(){
		yield return new WaitForSeconds(3);
		SceneManager.LoadScene (NameScene);
	}
		
}
