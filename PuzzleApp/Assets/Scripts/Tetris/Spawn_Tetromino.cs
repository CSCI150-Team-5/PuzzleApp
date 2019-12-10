using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Spawn_Tetromino : MonoBehaviour
{
    [SerializeField] private Button LB;
    [SerializeField] private Button RB;
    [SerializeField] private Button Rotate;
    [SerializeField] private GameObject gameoverPanel;
    private bool active;
    private GameObject tetromino;

    public GameObject[] Tetrominoes;
    // Start is called before the first frame update
    void Start()
    {
        active = true;
        MoveGOPanel("DISMISS");
        NewTetromino();
    }

    // Update is called once per frame
    public void NewTetromino()
    {
        if (active)
        {
            tetromino = Instantiate(Tetrominoes[Random.Range(0, Tetrominoes.Length)], transform.position, Quaternion.identity);
            tetromino.tag = "tet";
            Debug.Log(tetromino.tag);
            tetromino.SendMessage("AssignSpawner", this.gameObject);
            LB.GetComponent<LeftButton>().SetTemp(tetromino);
            RB.GetComponent<RightButton>().SetTemp(tetromino);
            Rotate.GetComponent<Rotate>().SetTemp(tetromino);
        }
    }

    public void Stop()
    {
        Debug.Log("END GAME");
        active = false;
        MoveGOPanel("ACCESS");
    }

    private void MoveGOPanel(string activity)
    {
        Vector3 pos = Vector3.zero;
        if (activity == "DISMISS")
        {
            pos = new Vector3(10000, 10000, 0);
        }
        gameoverPanel.transform.localPosition = pos;
    }

    public void Replay()
    {
        Debug.Log("REPLAY");
        tetromino.GetComponent<Tetris_Block>().EmptyGrid();
        foreach (GameObject tet in GameObject.FindGameObjectsWithTag("tet"))
        {
            Destroy(tet);
        }
        MoveGOPanel("DISMISS");
        active = true;
        NewTetromino();
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}