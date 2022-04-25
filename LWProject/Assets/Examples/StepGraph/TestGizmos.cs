using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGizmos : MonoBehaviour
{
    public Vector3[] vector3Array;
    // Start is called before the first frame update
   
    private void OnDrawGizmos()
    {

        if (vector3Array == null || vector3Array.Length == 0)
        {

            return;
        }
        for (int i = 0; i < vector3Array.Length; i++)
        {
            Gizmos.color = Color.gray;
            Gizmos.DrawSphere(vector3Array[i], 0.1f);
            if (i + 1 < vector3Array.Length)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(vector3Array[i], vector3Array[i+1]);
            }
        }
      
    }
}
