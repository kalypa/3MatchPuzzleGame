using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    public Transform blankSprPrefab;

    public int blankSprHeight = 30;

    public int blankSprWidth = 10;

    // ��� �κ��� �̸� �����Ѵ�

    public int blankHeader = 8;

    // ������ ��� ���� ��ü�� �����ϴ� �뵵
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

    // ����� "�Ѱ��� ����" �� ����ȿ� �����ϴ���? ���ϴ���? bool
    bool IsInBoard(int x, int y)
    {
        return (x >= 0 && x < blankSprWidth && y >= 0);
    }

    public bool IsVaildPos(Shape shape)
    {
        foreach (Transform child in shape.transform)
        {
            Vector2 pos = Vector2Int.RoundToInt(child.position);

            // ����� ����ȿ� �����ϴ���? üũ
            if (!IsInBoard((int)pos.x, (int)pos.y))
                return false;

            // ������ ������ �̹� ä���� ����� �ִ���? üũ
            if (IsArrayGrid((int)pos.x, (int)pos.y))
                return false;
        }

        return true;
    }


    ///////////// ������� ��Ͻױ� ���� �Լ� /////////////////

    // 2�����迭 ����� ���������� �����ϴ� �Լ�
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

    // arrayGrid�� (X, Y)������ ����ִ���? Ȯ���ϴ� �Լ�
    bool IsArrayGrid(int x, int y)
    {
        return (arrayGrid[x, y] != null);
    }



    ///////////// �� ����� ���� �Լ��� /////////////////

    // ����(��)�� ������� ���� á����?
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

    // ����(��)�� �����
    void ClearRow(int y)
    {
        for (int x = 0; x < blankSprWidth; x++)
        {
            // ������ ���ӿ�����Ʈ�� �����Ѵٸ� ���� �� ������ �ʱ�ȭ
            if (arrayGrid[x, y] != null)
            {
                Destroy(arrayGrid[x, y].gameObject);
            }
            arrayGrid[x, y] = null;
        }
    }

    // �� ���� ������ �̵���Ų��
    void ShiftOneRowDown(int y)
    {
        for (int x = 0; x < blankSprWidth; ++x)
        {
            // �������� ������� �ʴٸ�?
            if (arrayGrid[x, y] != null)
            {
                // �������� ���� "��ĭ ��"���� ����
                arrayGrid[x, y - 1] = arrayGrid[x, y];

                // �������� �ʱ�ȭ
                arrayGrid[x, y] = null;

                // �ؿ�ĭ�� ��ġ���� �̵�
                arrayGrid[x, y - 1].position += Vector3.down;
            }
        }
    }

    // ����� ���� ���������� ���ٹ����� �̵�
    void ShitfRowsDown(int startY)
    {
        for (int i = startY; i < blankSprHeight; ++i)
        {
            ShiftOneRowDown(i);
        }
    }

    // ��ü ��ϰ����� ��ĵ�Ͽ� Ŭ����!
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
