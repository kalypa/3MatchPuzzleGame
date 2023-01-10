using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    Board gameBoard;

    // 블록 생성(스포너)
    Spawner blockSpawner;

    // 현재 활성화된 블록
    Shape activeShape;

    //고스트 블록
    Ghost ghostShape;

    ScoreManager scoreManager;
    // 블록 드랍 관련된 변수들
    [Range(0.02f, 1f)] public float dropIntervalRate = 0.8f;
    float timeToDrop = 0f;

    // 왼쪽 오른쪽 키 딜레이 처리 관련변수
    [Range(0.2f, 1f)] public float keyRepeatRateLeftRight = 0.5f;
    float timeToNextKeyLeftRight = 0f;


    // 드랍다운 관련 변수
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
        // 블록 스포너의 위치를 정수로 고정시켜주기 위함
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
        // 첫 블록을 생성
        if (activeShape == null)
        {
            activeShape = blockSpawner.SpawnShape();
        }
    }

    private void Update()
    {
        // 예외처리
        if (gameBoard == null || blockSpawner == null || activeShape == null || bGameOver)
            return;


        // 키입력처리
        PlayerInput();


        // 블록자동 드랍 타이머 증가
        timeToDrop += Time.deltaTime;

        // 왼쪽 오른쪽 키 딜레이 처리
        timeToNextKeyLeftRight += Time.deltaTime;
        if (timeToNextKeyLeftRight >= keyRepeatRateLeftRight)
            timeToNextKeyLeftRight = 0f;

        // 드랍다운 딜레이 처리
        timeToNextKeyDown += Time.deltaTime;
        if (timeToNextKeyDown >= keyRepeatRateDown)
            timeToNextKeyDown = 0f;


        // 일정시간마다 자동으로 생성되는 블록
        // 딜레이를 적용 시켜야 함(원하는 시간의 딜레이)

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
        //        // 블록은 계속 아래로...
        //        activeShape.MoveDown();

        //        // 블록이 바운더리 안에 존재하는지? 체크
        //        if (!gameBoard.IsVaildPos(activeShape))
        //        {
        //            activeShape.MoveUp();

        //            // 그리드 배열에 블록을 저장
        //            gameBoard.StoreShapeInGrid(activeShape);

        //            // 새로운 블록모양을 생성
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
        else if (Input.GetButtonDown("Rotate"))   // 회전
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
                // 블록은 계속 아래로...
                activeShape.MoveDown();

                // 블록이 바운더리 안에 존재하는지? 체크
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

        // 그리드 배열에 블록을 저장
        gameBoard.StoreShapeInGrid(activeShape);

        //고스트 블록이 있다면 제거해준다
        if(ghostShape)
        {
            ghostShape.Remove();
        }

        // 새로운 블록모양을 생성
        activeShape = blockSpawner.SpawnShape();
        timeToNextKeyLeftRight = 0f;
        timeToNextKeyDown = 0f;
        // 블록 클리어 처리
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
