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
            Debug.LogError("���尡 ����...");
            return;
        }

        blockSpawner = GameObject.FindObjectOfType<Spawner>();
        if (blockSpawner == null)
        {
            Debug.LogError("�����ʰ� ����...");
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
