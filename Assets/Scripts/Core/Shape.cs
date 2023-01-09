using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    public bool isCanRotate = true;

    void Move(Vector3 moveDir)
    {
        transform.position += moveDir;
    }

    public void MoveLeft()
    {
        Move(Vector3.left);
    }

    public void MoveRight()
    {
        Move(Vector3.right);
    }

    public void MoveUp()
    {
        Move(Vector3.up);
    }

    public void MoveDown()
    {
        Move(Vector3.down);
    }

    public void RotateRight()
    {
        transform.Rotate(0, 0, -90);
    }

    public void RotateLeft()
    {
        transform.Rotate(0, 0, 90);
    }
}
