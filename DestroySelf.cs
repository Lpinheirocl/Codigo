/*
 * Copyright (c) 2017 Razeware LLC
 */

using UnityEngine;
using System.Collections;

/// <resumo>
/// Pequeno script para destruir facilmente um objeto depois de um tempo.
/// </resumo>
public class DestroySelf : MonoBehaviour
{
    public float Delay = 3f;
	//Delay em segundos antes de destruir o gameobject

    void Start ()
    {
        Destroy (gameObject, Delay);
    }
}
