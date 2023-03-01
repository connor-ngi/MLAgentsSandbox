using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubes : MonoBehaviour
{
    public int currentRed;

    private void Start()
    {
        StartCoroutine(Randomizer());
    }

    private IEnumerator Randomizer()
    {
        while(true)
        {
            int random = Random.Range(0, 9);

            for(int i = 0; i < 9; i++)
            {
                Renderer rend = transform.GetChild(i).GetComponent<Renderer>();
                rend.material.color = random == i ? Color.red : Color.white;
            }

            currentRed = random;

            yield return new WaitForSeconds(Random.Range(0, 4));
        }
    }
}
