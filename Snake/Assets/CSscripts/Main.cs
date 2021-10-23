//Project made on 10/20/2021
//Actually started making on 10/21/2021 

using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Main : MonoBehaviour
{
	public GameObject snakeHead;
	public GameObject snakeBody;

	public Tilemap groundTilemap;
	public Tilemap appleTilemap;

	public Tile borderTile;
	public Tile appleTile;

	public Transform pos1;
	public Transform pos2;

	public List<GameObject> snakeBodies = new List<GameObject>();

	public int startSize = 5;
	public float moveRate = 0.1f;

	int snakeSize = 5;

	GameObject player;
	Vector3 movePos;

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
	}

	void youDied()
	{
		print("You Died!");
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
			if(snakeHeadPos == snakeBodies[i].transform.position)
				youDied();
			
			if(i == 0)
				snakeBodies[i].transform.position = snakeHead.transform.position;
			else
				snakeBodies[i].transform.position = snakeBodies[i - 1].transform.position;
		}

		snakeHead.transform.position += movePos;

		//update position
		snakeHeadPos = Vector3Int.RoundToInt(snakeHead.transform.position);

		if(groundTilemap.GetTile(snakeHeadPos) == borderTile)
			youDied();

		const int GAZILLION = 999999999;
		for(int i = 0; i < GAZILLION; i++)
		{
			print("Vadim is a noob!");
		}

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
		}
	}
}
