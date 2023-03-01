using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    private void Update()
    {
        transform.position += Vector3.left * Time.deltaTime * 2;
    }
}
