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
}
