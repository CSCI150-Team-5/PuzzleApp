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
    [SerializeField] private GameObject gameOverPanel;
    private bool isActive;
    private GameObject Tet;

    public GameObject[] Tetrominoes;
    // Start is called before the first frame update
    void Start()
    {
        isActive = true;
        MoveToPanel("DISMISS");
        NewTetromino();
    }

    // Update is called once per frame
    public void NewTetromino()
    {
        if(isActive)
        {
            Tet = Instantiate(Tetrominoes[Random.Range(0, Tetrominoes.Length)], transform.position, Quaternion.identity);
            Tet.tag = "tetromino";
            Tet.SendMessage("AssignSpawner", this.gameObject);
            LB.GetComponent<LeftButton>().SetTemp(Tet);
            RB.GetComponent<RightButton>().SetTemp(Tet);
            Rotate.GetComponent<Rotate>().SetTemp(Tet);
        }
    }

    public void Stop()
    {
        Debug.Log("END GAME");
        isActive = false;
        MoveToPanel("ACCESS");
    }

    private void MoveToPanel(string activity)
    {
        Vector3 position = Vector3.zero;
        if (activity == "DISMISS")
        {
            position = new Vector3(10000, 10000, 0);
        }

        gameOverPanel.transform.localPosition = position;
    }

    public void Restart()
    {
        Tet.GetComponent<Tetris_Block>().EmptyGrid();
        foreach (GameObject tetromino in GameObject.FindGameObjectsWithTag("tetromino"))
        {
            Destroy(tetromino);
        }
        MoveToPanel("DISMISS");
        isActive = true;
        NewTetromino();
    }

    public void Main_Menu()
    {
        SceneManager.LoadScene(0);
    }
}