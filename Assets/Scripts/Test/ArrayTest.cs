using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayTest : MonoBehaviour
{
    int[,] array = new int[,]
    {
        {1,2,3 },
        {4,5,6 },
        {7,8,9 }
    };

    string[,,] array3D = new string[,,]
    {
        {
            {"000", "001","002" },
            {"010","011","012" },
            {"020","021","022" }
        },
        {
            {"100", "001","102" },
            {"110","111","112" },
            {"120","121","122" }
        },

    };
    private void Start()
    {
        int cnt = 2;
        //foreach( int a in array)
        //{
        //    Debug.Log(a);
        //}

        for(int i = 0; i < array.GetLength(0); i++)
        {
            for(int j = 0; j < array.GetLength(1); j++)
            {
                //Debug.Log(array[i, j]);
        
                //if (array[i, j] % 2 == 0)
                //    Debug.Log(array[i, j]);
        
                //if (i == j)
                //    Debug.Log(array[i, j]);

                //숙제 
                if(i + cnt == j)
                {
                    Debug.Log(array[i, j]);
                    cnt-=2;
                }
            }
        }

        Debug.Log(array3D[1, 1, 1]);

        int amountDimensions = array3D.Rank;
        Debug.Log(string.Format("이 배열의 차원은 : {0}", amountDimensions));

    }
}
