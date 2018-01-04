﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{

    [SerializeField]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            maximum = max;
            minimum = min;
        }
    }

    public int columns = 5;
    public int rows = 5;
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallsTiles;
    private Dictionary<Vector2, Vector2> gridPositions = new Dictionary<Vector2, Vector2>();
    private Transform boardHolder;
    private List<Vector3> gridPosition = new List<Vector3>();

    void InitialiseList()
    {
        gridPosition.Clear();

        for (int x = 1; x < columns - 1; ++x)
        {
            for (int y = 1; y < rows - 1; ++y)
            {
                gridPosition.Add(new Vector3(x, y, 0f));
            }
        }
    }

    public void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                gridPositions.Add(new Vector2(x, y), new Vector2(x, y));
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    public void AddToBoard (int horizontal, int vertical)
    {
        if (horizontal == 1)
        {
            int x = (int)Player.position.x;
            int sightX = x + 2;
            for (x += 1; x <= sightX; ++x)
            {
                int y = (int)Player.position.y;
                int sightY = y + 1;
                for (y -= 1; y <= sightY; ++y)
                {
                    AddTiles(new Vector2(x, y));
                }
            }
        }
        else if (horizontal == -1)
        {
            int x = (int)Player.position.x;
            int sightX = x - 2;
            for (x -= 1; x >= sightX; x--)
            {
                int y = (int)Player.position.y;
                int sightY = y + 1;
                for (y -= 1; y <= sightY; y++)
                {
                    AddTiles(new Vector2(x, y));
                }
            }
        }
        else if (vertical == 1)
        {
            int y = (int)Player.position.y;
            int sightY = y + 2;
            for (y += 1; y <= sightY; ++y)
            {
                int x = (int)Player.position.x;
                int sightX = x + 1;
                for (x -= 1; x <= sightX; ++x)
                {
                    AddTiles(new Vector2(x, y));
                }
            }
        }
        else if (vertical == -1)
        {
            int y = (int)Player.position.y;
            int sightY = y - 2;
            for (y -= 1; y >= sightY; --y)
            {
                int x = (int)Player.position.x;
                int sightX = x + 1;
                for (x -= 1; x <= sightX; ++x)
                {
                    AddTiles(new Vector2(x, y));
                }
            }
        }
    }

    private void AddTiles(Vector2 tileToAdd)
    {
        if (!gridPositions.ContainsKey(tileToAdd))
        {
            gridPositions.Add(tileToAdd, tileToAdd);
            GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
            GameObject instance = Instantiate(toInstantiate, new Vector3(tileToAdd.x, tileToAdd.y, 0f), Quaternion.identity) as GameObject;
            instance.transform.SetParent(boardHolder);
            if (Random.Range(0, 3) == 1)
            {
                toInstantiate = wallTiles[Random.Range(0, wallTiles.Length)];
                instance = Instantiate(toInstantiate, new Vector3(tileToAdd.x, tileToAdd.y, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPosition.Count);
        Vector3 randomPosition = gridPosition[randomIndex];
        gridPosition.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)
    {
        int objectCount = Random.Range(minimum, maximum + 1);
        for (int i = 0; i < objectCount; ++i)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        BoardSetup();
        InitialiseList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        int enemyCount = (int)Mathf.Log(level, 2f);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
