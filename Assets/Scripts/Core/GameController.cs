using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    Board gameBoard;

    // ��� ����(������)
    Spawner blockSpawner;

    // ���� Ȱ��ȭ�� ���
    Shape activeShape;

    //��Ʈ ���
    Ghost ghostShape;

    ScoreManager scoreManager;
    // ��� ��� ���õ� ������
    [Range(0.02f, 1f)] public float dropIntervalRate = 0.8f;
    float timeToDrop = 0f;

    // ���� ������ Ű ������ ó�� ���ú���
    [Range(0.2f, 1f)] public float keyRepeatRateLeftRight = 0.5f;
    float timeToNextKeyLeftRight = 0f;


    // ����ٿ� ���� ����
    [Range(0.02f, 1f)] public float keyRepeatRateDown = 0.5f;
    float timeToNextKeyDown = 0f;

    bool bGameOver = false;

    public GameObject gameOverPanelObj = null;

    private void Awake()
    {
        gameBoard = GameObject.FindObjectOfType<Board>();
        if (gameBoard == null)
        {
            Debug.LogWarning("gameBoard is null");
            return;
        }

        blockSpawner = GameObject.FindObjectOfType<Spawner>();
        if (blockSpawner == null)
        {
            Debug.LogWarning("blockSpawner is null");
            return;
        }
        // ��� �������� ��ġ�� ������ ���������ֱ� ����
        blockSpawner.transform.position = Vector3Int.RoundToInt(blockSpawner.transform.position);


        ghostShape = GameObject.FindObjectOfType<Ghost>();
        if (ghostShape == null)
        {
            Debug.LogWarning("ghostShape is null");
            return;
        }

        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        if (scoreManager == null)
        {
            Debug.LogWarning("scoreManager is null");
            return;
        }
    }

    private void Start()
    {
        // ù ����� ����
        if (activeShape == null)
        {
            activeShape = blockSpawner.SpawnShape();
        }
    }

    private void Update()
    {
        // ����ó��
        if (gameBoard == null || blockSpawner == null || activeShape == null || bGameOver)
            return;


        // Ű�Է�ó��
        PlayerInput();


        // ����ڵ� ��� Ÿ�̸� ����
        timeToDrop += Time.deltaTime;

        // ���� ������ Ű ������ ó��
        timeToNextKeyLeftRight += Time.deltaTime;
        if (timeToNextKeyLeftRight >= keyRepeatRateLeftRight)
            timeToNextKeyLeftRight = 0f;

        // ����ٿ� ������ ó��
        timeToNextKeyDown += Time.deltaTime;
        if (timeToNextKeyDown >= keyRepeatRateDown)
            timeToNextKeyDown = 0f;


        // �����ð����� �ڵ����� �����Ǵ� ���
        // �����̸� ���� ���Ѿ� ��(���ϴ� �ð��� ������)

        // 1. Time.time
        //if(Time.time > timeToDrop)
        //{
        //    timeToDrop = Time.time + dropIntervalRate;
        //    if (activeShape)
        //    {
        //        activeShape.MoveDown();
        //    }
        //}

        // 2. Time.deltaTime
        //timeToDrop += Time.deltaTime;
        //if (timeToDrop >= dropIntervalRate)
        //{
        //    timeToDrop = 0f;
        //    if (activeShape)
        //    {
        //        // ����� ��� �Ʒ���...
        //        activeShape.MoveDown();

        //        // ����� �ٿ���� �ȿ� �����ϴ���? üũ
        //        if (!gameBoard.IsVaildPos(activeShape))
        //        {
        //            activeShape.MoveUp();

        //            // �׸��� �迭�� ����� ����
        //            gameBoard.StoreShapeInGrid(activeShape);

        //            // ���ο� ��ϸ���� ����
        //            activeShape = blockSpawner.SpawnShape();
        //        }
        //    }
        //}

    }
    private void LateUpdate()
    {
        if(ghostShape)
        {
            ghostShape.DrawGhost(activeShape, gameBoard);
        }
    }

    void PlayerInput()
    {
        if ((Input.GetButton("MoveRight") && timeToNextKeyLeftRight == 0f) || Input.GetButtonDown("MoveRight"))
        {
            activeShape.MoveRight();
            if (!gameBoard.IsVaildPos(activeShape))
            {
                activeShape.MoveLeft();
            }
        }
        else if ((Input.GetButton("MoveLeft") && timeToNextKeyLeftRight == 0f) || Input.GetButtonDown("MoveLeft"))
        {
            activeShape.MoveLeft();
            if (!gameBoard.IsVaildPos(activeShape))
            {
                activeShape.MoveRight();
            }
        }
        else if (Input.GetButtonDown("Rotate"))   // ȸ��
        {
            activeShape.RotateRight();
            if (!gameBoard.IsVaildPos(activeShape))
            {
                activeShape.RotateLeft();
            }
        }
        else if (Input.GetButton("MoveDown") && timeToNextKeyDown == 0f || timeToDrop >= dropIntervalRate)
        {
            timeToDrop = 0f;
            if (activeShape)
            {
                // ����� ��� �Ʒ���...
                activeShape.MoveDown();

                // ����� �ٿ���� �ȿ� �����ϴ���? üũ
                if (!gameBoard.IsVaildPos(activeShape))
                {
                    if (gameBoard.IsGameOver(activeShape))
                    {
                        GameOver();
                    }
                    else
                    {
                        LandShape();
                    }
                }
            }
        }
    }

    void LandShape()
    {
        activeShape.MoveUp();

        // �׸��� �迭�� ����� ����
        gameBoard.StoreShapeInGrid(activeShape);

        //��Ʈ ����� �ִٸ� �������ش�
        if(ghostShape)
        {
            ghostShape.Remove();
        }

        // ���ο� ��ϸ���� ����
        activeShape = blockSpawner.SpawnShape();
        timeToNextKeyLeftRight = 0f;
        timeToNextKeyDown = 0f;
        // ��� Ŭ���� ó��
        gameBoard.ClearAllRows();
        if(gameBoard.completeRows > 0)
        {
            scoreManager.ScoreLines(gameBoard.completeRows);
        }
    }

    void GameOver()
    {
        activeShape.MoveUp();
        bGameOver = true;

        if(gameOverPanelObj)
        {
            gameOverPanelObj.SetActive(true);
        }
    }

    public void RestartGame()
    {
        if (gameOverPanelObj)
        {
            gameOverPanelObj.SetActive(false);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
    }
}
