using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Board gameBoard;
    Spawner blockSpawner;
    Shape activeShape;
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
}
