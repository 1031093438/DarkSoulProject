using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmosTest : MonoBehaviour
{
    public GameObject model;
    private void OnDrawGizmos()
    {

        Gizmos.color = Color.green;
        Gizmos.DrawCube(model.transform.position + new Vector3(0,1.0f,5.0f),  new Vector3(0.5f, 0.5f, model.transform.forward.z * 5.0f));
        

    }
}
