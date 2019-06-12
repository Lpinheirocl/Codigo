/*
 * Copyright (c) 2017 Razeware LLC
 */

using UnityEngine;
using System.Collections;

/// <resumo>
/// Esse script garante que a bomba pode ser deixada aos pés do jogador sem que de bug quando o jogador anda para se afastar.
/// desabilita o trigger no collider, fazendo o objeto solid.
/// </resumo>
public class DisableTriggerOnPlayerExit : MonoBehaviour
{

    public void OnTriggerExit (Collider other)
    {
        if (other.gameObject.CompareTag ("Player"))
		{ //quando o jogador sai da area do trigger
			GetComponent<Collider> ().isTrigger = false; //desabilita o trigger
        }
    }
}
