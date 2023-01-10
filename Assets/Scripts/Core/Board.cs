using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    public Transform blankSprPrefab;

    public int blankSprHeight = 30;

    public int blankSprWidth = 10;

    // 헤더 부분을 미리 선언한다

    public int blankHeader = 8;

    // 게임의 블록 공간 전체를 저장하는 용도
    Transform[,] arrayGrid;

    public int completeRows = 0;

    private void Awake()
    {
        arrayGrid = new Transform[blankSprWidth, blankSprHeight];
    }

    private void Start()
    {
        DrawEmptyCells();
    }

    void DrawEmptyCells()
    {
        if (blankSprPrefab != null)
        {
            for (int y = 0; y < blankSprHeight - blankHeader; y++)
            {
                for (int x = 0; x < blankSprWidth; x++)
                {
                    Transform clone;
                    clone = Instantiate(blankSprPrefab, new Vector3(x, y, 0), Quaternion.identity) as Transform;
                    clone.name = string.Format("Board Spce(x={0},y={1})", x, y).ToString();
                    clone.transform.parent = this.transform;
                }
            }
        }
        else
        {
            Debug.LogWarning("blankSprPrefab is null");
        }
    }

    // 블록의 "한개의 조각" 이 보드안에 존재하는지? 안하는지? bool
    bool IsInBoard(int x, int y)
    {
        return (x >= 0 && x < blankSprWidth && y >= 0);
    }

    public bool IsVaildPos(Shape shape)
    {
        foreach (Transform child in shape.transform)
        {
            Vector2 pos = Vector2Int.RoundToInt(child.position);

            // 블록이 보드안에 존재하는지? 체크
            if (!IsInBoard((int)pos.x, (int)pos.y))
                return false;

            // 떨어질 공간에 이미 채워진 블록이 있는지? 체크
            if (IsArrayGrid((int)pos.x, (int)pos.y))
                return false;
        }

        return true;
    }


    ///////////// 블록위에 블록쌓기 관련 함수 /////////////////

    // 2차원배열 블록의 개별조각을 저장하는 함수
    public void StoreShapeInGrid(Shape shape)
    {
        if (shape == null)
            return;

        foreach (Transform child in shape.transform)
        {
            Vector2 pos = Vector2Int.RoundToInt(child.position);
            arrayGrid[(int)pos.x, (int)pos.y] = child;
        }
    }

    // arrayGrid의 (X, Y)공간이 비어있는지? 확인하는 함수
    bool IsArrayGrid(int x, int y)
    {
        return (arrayGrid[x, y] != null);
    }



    ///////////// 행 지우기 관련 함수들 /////////////////

    // 한줄(행)이 블록으로 가득 찼는지?
    bool IsComplete(int y)
    {
        for (int x = 0; x < blankSprWidth; ++x)
        {
            if (arrayGrid[x, y] == null)
                return false;
        }

        return true;
    }

    public bool IsGameOver(Shape shape)
    {
        foreach(Transform child in shape.transform)
        {
            if(child.transform.position.y >= blankSprHeight - blankHeader - 1)
            {
                return true;
            }
        }
        return false;
    }

    // 한줄(행)을 지운다
    void ClearRow(int y)
    {
        for (int x = 0; x < blankSprWidth; x++)
        {
            // 공간에 게임오브젝트가 존재한다면 삭제 후 공간도 초기화
            if (arrayGrid[x, y] != null)
            {
                Destroy(arrayGrid[x, y].gameObject);
            }
            arrayGrid[x, y] = null;
        }
    }

    // 한 행을 밑으로 이동시킨다
    void ShiftOneRowDown(int y)
    {
        for (int x = 0; x < blankSprWidth; ++x)
        {
            // 현재행이 비어있지 않다면?
            if (arrayGrid[x, y] != null)
            {
                // 현재행의 값을 "한칸 밑"으로 저장
                arrayGrid[x, y - 1] = arrayGrid[x, y];

                // 현재행은 초기화
                arrayGrid[x, y] = null;

                // 밑에칸의 위치값도 이동
                arrayGrid[x, y - 1].position += Vector3.down;
            }
        }
    }

    // 블록의 열의 시작점부터 한줄밑으로 이동
    void ShitfRowsDown(int startY)
    {
        for (int i = startY; i < blankSprHeight; ++i)
        {
            ShiftOneRowDown(i);
        }
    }

    // 전체 블록공간을 스캔하여 클리어!
    public void ClearAllRows()
    {
        completeRows = 0;
        for (int y = 0; y < blankSprHeight; ++y)
        {
            if (IsComplete(y))
            {
                completeRows++;
                ClearRow(y);

                ShitfRowsDown(y + 1);

                y--;
            }
        }
    }
}
