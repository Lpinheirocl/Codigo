/*
 * Copyright (c) 2017 Razeware LLC
 */

using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour
{
    //Manager
    public GlobalStateManager globalManager;
	public GlobalStateManager Scena;

	//Parâmetros do jogador
	[Range (1, 2)] //Habilita um slider no editor
    public int playerNumber = 1;
	//Indica qual player você é: p1 ou p2
    public float moveSpeed = 5f;
    public bool canDropBombs = true;
	//O jogador pode soltar uma bomba?
    public bool canMove = true;
	//O jogador pode mover?
    public bool dead = false;
	//O jogador esta morto?

    //private int bombs = 2;
	//Quantidades de bombas que o jogador pode soltar
	//Aumenta quando a bomba explode

    //Prefabs
    public GameObject bombPrefab;

	//Componentes em Cache
    private Rigidbody rigidBody;
    private Transform myTransform;
    private Animator animator;

	// Use isso para inicializar
    void Start ()
    {
		//Componentes em cache para uma melhor performance e menos digitação
        rigidBody = GetComponent<Rigidbody> ();
		///Registrando em cache um Rigidbody.
		///Neste caso, como não há especificação de um GameObject antes de "GetComponent", isso quer dizer que
		///será registrado o rigidbody do objeto que tiver esse Script anexado.
		///É a mesma coisa que escrever "PegarComponente RigidDoby do objeto que carregar esse script"
        myTransform = transform;
        animator = myTransform.Find ("PlayerModel").GetComponent<Animator> ();
    }

	// Update é chamado uma vez por frame
    void Update ()
    {
		UpdateMovement ();///A cada frame do jogo, o script chama a função "UpdateMovement"
    }

    private void UpdateMovement ()
    {
        animator.SetBool ("Walking", false); //Resets a animação de andar para idle

        if (!canMove)
		{ //Retorna se o jogador não consegue se mover.
            return;
        }

		//Dependendo do numero de jogadores use diferentes inputs para movimentação
		if (playerNumber == 1)
			///Esse script está anexado ao objeto do jogador 1 e do jogador 2
			///É necessário definir no Inspector o valor da variável "playerNumber"
			///Como foi escrito lá em cima "public int playerNumber = 1", o padrão é 1 mas é público e pode ser alterado para 2
			///Então se for 1, o script vai executar a função "UpdatePlayer1Movement" e vai usar WASD para movimentação
			///Se for definido para 2, o script ai executar a função "UpdatePlayer2Movemente" e vai usar as setas para se movimentar
        {
            UpdatePlayer1Movement ();
        } else
        {
            UpdatePlayer2Movement ();
        }
    }

	/// <resumo>
	/// Atualiza movimentação do Player 1 e faz a rotação com WASD e joga a bomba com espaço
	/// </resumo>
    private void UpdatePlayer1Movement ()
    {
        if (Input.GetKey (KeyCode.W))
		{ //Cima
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, moveSpeed);
			myTransform.rotation = Quaternion.Euler (0, 0, 0);
			//Quaternion é a função de rotação, ela é calculada em graus e não é uma linha reta
			//É como o "new Vector3" para movimentação, mas em graus e é usado para girar
			//Honestamente é um dos maiores mistérios do C# e da Unity, ninguém sabe como funciona exatamente ahuahuahauha
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.A))
		{ //esquerda
            rigidBody.velocity = new Vector3 (-moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 270, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.S))
		{ //baixo
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, -moveSpeed);
            myTransform.rotation = Quaternion.Euler (0, 180, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.D))
		{ //direita
            rigidBody.velocity = new Vector3 (moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 90, 0);
            animator.SetBool ("Walking", true);
        }

        if (canDropBombs && Input.GetKeyDown (KeyCode.Space))
		{ //Jogar bomba
            DropBomb ();
        }
    }

	/// <resumo>
	///Atualiza movimentação do Player 2 e faz a rotação com setas e joga a bomba com enter ou return
	/// </resumo>
    private void UpdatePlayer2Movement ()
    {
        if (Input.GetKey (KeyCode.UpArrow))
        { //Cima
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, moveSpeed);
            myTransform.rotation = Quaternion.Euler (0, 0, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.LeftArrow))
        { //esquerda
            rigidBody.velocity = new Vector3 (-moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 270, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.DownArrow))
        { //baixo
            rigidBody.velocity = new Vector3 (rigidBody.velocity.x, rigidBody.velocity.y, -moveSpeed);
            myTransform.rotation = Quaternion.Euler (0, 180, 0);
            animator.SetBool ("Walking", true);
        }

        if (Input.GetKey (KeyCode.RightArrow))
        { //direita
            rigidBody.velocity = new Vector3 (moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            myTransform.rotation = Quaternion.Euler (0, 90, 0);
            animator.SetBool ("Walking", true);
        }

        if (canDropBombs && (Input.GetKeyDown (KeyCode.KeypadEnter) || Input.GetKeyDown (KeyCode.Return)))
		{ //para o jogador 2 faz com que as teclas de nueros joguem as bombas. 
			//Sem um NumPack você não consegue jogar bombas
            DropBomb ();
        }
    }

	/// <resumo>
	/// Joga a bomba abaixo do jogador
	/// </resumo>
    private void DropBomb ()
    {
        if (bombPrefab)
		{ //Checa se a bomba prefab é atribuida primeiro
            //Cria uma nova bomba e atribui ela ao tile
            Instantiate (bombPrefab,
                new Vector3 (Mathf.RoundToInt (myTransform.position.x), bombPrefab.transform.position.y, Mathf.RoundToInt (myTransform.position.z)),
                bombPrefab.transform.rotation);
        }
    }

	public void OnTriggerEnter (Collider other) ///Ao colidir com outro Collider, o outro objeto é armazenado na memória com o ID "other"
    {
        if (!dead && other.CompareTag ("Explosion"))
        { //vivo & atingido pela explosão
            Debug.Log ("P" + playerNumber + " hit by explosion!");

            dead = true;
            globalManager.PlayerDied (playerNumber); //Notifica a variavel global que o jogador morreu.
            Destroy (gameObject);
        }
    }
}
