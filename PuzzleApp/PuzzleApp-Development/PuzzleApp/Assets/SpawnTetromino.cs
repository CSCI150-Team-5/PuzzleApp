using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTetromino : MonoBehaviour
{
    public GameObject[] Tetrominoes;
    
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        int i = Random.Range(0, Tetrominoes.Length);
        Instantiate(Tetrominoes[i], transform.position, Quaternion.identity);
    }
}
