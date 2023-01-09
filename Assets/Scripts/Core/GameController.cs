using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Board gameBoard;

    Spawner blockSpawner;

    Shape activeShape;

    [Range(0.02f, 1f)] public float dropIntervalRate = 0.8f;
    float timeToDrop = 0f;

    private void Awake()
    {
        gameBoard = GameObject.FindObjectOfType<Board>();
        if(gameBoard == null)
        {
            Debug.LogError("보드가 없어...");
            return;
        }

        blockSpawner = GameObject.FindObjectOfType<Spawner>();
        if (blockSpawner == null)
        {
            Debug.LogError("스포너가 없어...");
            return;
        }
    }

    private void Start()
    {
        if(activeShape == null)
        {
            activeShape = blockSpawner.SpawnShape();
        }
    }

    private void Update()
    {
        //if(Time.time > timeToDrop)
        //{
        //    timeToDrop = Time.time + dropIntervalRate;
        //    if (activeShape)
        //    {
        //        activeShape.MoveDown();
        //    }
        //}
        timeToDrop += Time.deltaTime;
        if (timeToDrop >= dropIntervalRate)
        {
            timeToDrop = 0f;
            if (activeShape)
            {
                activeShape.MoveDown();
                if(!gameBoard.IsvailPos(activeShape))
                {
                    activeShape.MoveUp();
                    gameBoard.StoreShapeInGrid(activeShape);
                    activeShape = blockSpawner.SpawnShape();
                }
            }
        }

    }
}
