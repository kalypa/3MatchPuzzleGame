using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    Shape ghostShape = null;

    bool bHitBottom = false;

    public void DrawGhost(Shape oriShape, Board gameBoard)
    {
        //생성된 고스트 오브젝트가 없다면 생성
        if(!ghostShape)
        {
            ghostShape = Instantiate(oriShape, oriShape.transform.position, oriShape.transform.rotation) as Shape;
            ghostShape.gameObject.name = "GhostShape";
            ghostShape.transform.SetParent(gameObject.transform);

            SpriteRenderer[] allSprRenders = ghostShape.GetComponentsInChildren<SpriteRenderer>();
            foreach(SpriteRenderer s in allSprRenders)
            {
                s.color = new Color(s.color.r, s.color.g, s.color.b, 0.2f);
            }
        }
        else
        {
            ghostShape.transform.position = oriShape.transform.position;
            ghostShape.transform.rotation = oriShape.transform.rotation;
        }

        bHitBottom = false;
        while(!bHitBottom)
        {
            ghostShape.MoveDown();
            if(!gameBoard.IsVaildPos(ghostShape))
            {
                ghostShape.MoveUp();
                bHitBottom = true;
            }
        }
    }

    public void Remove()
    {
        if (ghostShape)
            Destroy(ghostShape.gameObject);
    }
}
