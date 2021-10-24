//Project made on 10/20/2021
//Actually started making on 10/21/2021 

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine;
using TMPro;

public class Main : MonoBehaviour
{
	public GameObject snakeHead;
	public GameObject snakeBody;

	public GameObject deathUI;
	public GameObject pausedUI;
	public TMP_Text deathUIScoreText;

	public Tilemap groundTilemap;
	public Tilemap appleTilemap;

	public Tile borderTile;
	public Tile appleTile;

	public Transform pos1;
	public Transform pos2;

	public List<GameObject> snakeBodies = new List<GameObject>();

	public float moveRate = 0.1f;

	const int startSize = 5;

	int snakeSize = startSize;
	bool paused = false;

	GameObject player;
	Vector3 movePos = Vector3.up;

	void Awake()
	{
		player = gameObject;
		spawnApple();

		InvokeRepeating("moveSnake", 0, moveRate);
	}

	void Update()
	{
		float xAxisInput = Input.GetAxisRaw("Horizontal");
		float yAxisInput = Input.GetAxisRaw("Vertical");

		if(xAxisInput != 0 ^ yAxisInput != 0)
		{
			movePos.x = xAxisInput;
			movePos.y = yAxisInput;
		}

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			togglePause();
		}
	}

	public void replay()
	{
		print("Reset");
		Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void togglePause()
	{
		if(paused == true)
		{
			print("Resumed");
			Time.timeScale = 1;
			pausedUI.SetActive(false);
			paused = false;
		}
		else if(paused == false)
		{
			print("Paused");
			Time.timeScale = 0;
			pausedUI.SetActive(true);

			paused = true;
		}
	}

	void youDied()
	{
		print("You Died!");
		Time.timeScale = 0;

		deathUIScoreText.text = "Score: " + (snakeSize - startSize);

		deathUI.SetActive(true);

		AudioManager.play("GameOver");
	}

	void spawnApple()
	{
		int randX = (int)Random.Range(pos1.position.x, pos2.position.x);
		int randY = (int)Random.Range(pos1.position.y, pos2.position.y);

		appleTilemap.SetTile(new Vector3Int(randX, randY, 0), appleTile);
	}

	void moveSnake()
	{
		Vector3Int snakeHeadPos = Vector3Int.RoundToInt(snakeHead.transform.position);

		int snakeBodyLength = snakeBodies.Count;

		for(int i = snakeBodyLength - 1; i >= 0; i--)
		{
			if(i == 0)
				snakeBodies[i].transform.position = snakeHead.transform.position;
			else
				snakeBodies[i].transform.position = snakeBodies[i - 1].transform.position;
		}

		snakeHead.transform.position += movePos;

		//update position
		snakeHeadPos = Vector3Int.RoundToInt(snakeHead.transform.position);


		//check if the snake is touching a snake body piece
		for(int i = 0; i < snakeBodyLength; i++)
		{
			Vector3Int snakeBodyPos = Vector3Int.RoundToInt(snakeBodies[i].transform.position);

			if(snakeHeadPos == snakeBodyPos)
				youDied();
		}

		//check if the snake is touching the border
		if(groundTilemap.GetTile(snakeHeadPos) == borderTile)
			youDied();


		if(appleTilemap.GetTile(snakeHeadPos) == appleTile)
		{
			appleTilemap.SetTile(snakeHeadPos, null);
			snakeSize++;

			//add a snake body to the end of the snake
			GameObject snakeBodyClone = Instantiate(snakeBody);
			snakeBodyClone.transform.parent = snakeBody.transform.parent;
			snakeBodyClone.name = snakeBody.name + "Clone";
			snakeBodyClone.transform.position = snakeBodies[snakeBodies.Count - 1].transform.position;

			snakeBodies.Add(snakeBodyClone);

			spawnApple();
			AudioManager.play("Eat");
		}
	}
}
