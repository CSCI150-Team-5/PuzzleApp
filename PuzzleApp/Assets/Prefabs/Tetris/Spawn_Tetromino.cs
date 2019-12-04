using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawn_Tetromino : MonoBehaviour
{
    [SerializeField] private Button LB;
    [SerializeField] private Button RB;
    [SerializeField] private Button Rotate;

    public GameObject[] Tetrominoes;
    // Start is called before the first frame update
    void Start()
    {
        NewTetromino();
    }

    // Update is called once per frame
    public void NewTetromino()
    {
        GameObject temp = Instantiate(Tetrominoes[Random.Range(0, Tetrominoes.Length)], transform.position, Quaternion.identity);
        LB.GetComponent<LeftButton>().SetTemp(temp);
        RB.GetComponent<RightButton>().SetTemp(temp);
        Rotate.GetComponent<Rotate>().SetTemp(temp);
    }
}