using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Board : MonoBehaviour
{
    public Transform blankSpritePrefab;

    public int blankSpriteHeight = 30;

    public int blankSpriteWidth = 10;

    public int blankHeader = 8;
    Transform[,] arrayGrid;

    private void Awake()
    {
        arrayGrid = new Transform[blankSpriteWidth, blankSpriteHeight];
    }

    private void Start()
    {
        DrawEmptyCells();
    }

    private void DrawEmptyCells()
    {
        if(blankSpritePrefab != null)
        {
            for(int y = 0; y < blankSpriteHeight - blankHeader; y++)
            {
                for(int x = 0; x < blankSpriteWidth; x++)
                {
                    Transform clone;
                    clone = Instantiate(blankSpritePrefab, new Vector3(x, y, 0), Quaternion.identity) as Transform;
                    clone.name = string.Format("Board Space(x ={0}, y ={1}", x, y).ToString();
                    clone.SetParent(this.transform);
                    

                }
            }
        }
        else
        {
            Debug.LogError("왜 BlankSprite Prefab 안 넣었어");
        }
    }

    bool IsInBoard(int x, int y)
    {
        return (x >= 0 && x < blankSpriteWidth && y >= 0);
    }

    public bool IsvailPos(Shape shape)
    {
        foreach(Transform child in shape.transform)
        {
            Vector2 pos = Vector2Int.RoundToInt(child.position);

            if (!IsInBoard((int)pos.x, (int)pos.y))
                return false;

            if(IsArrayGrid((int)pos.x, (int)pos.y))
                    return false;
        }

        return true;
    }

    public void StoreShapeInGrid(Shape shape)
    {
        if (shape == null)
            return;

        foreach(Transform child in shape.transform)
        {
            Vector2 pos = Vector2Int.RoundToInt(child.position);
            arrayGrid[(int)pos.x, (int)pos.y] = child;
        }
    }

    bool IsArrayGrid(int x, int y)
    {
        return (arrayGrid[x, y] != null);
    }
}
