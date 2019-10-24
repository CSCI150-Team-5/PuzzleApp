using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] 
    private Snake snake;
    private Board board;
    public GameObject myMasterMenu; //Make a place for the Master Menu Prefab - Place the prefab in here in the Unity UI

    void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        GameAssets.instance.cam.orthographicSize = GameAssets.instance.gridSize.y / 2;
        GameAssets.instance.cam.transform.position = new Vector3((GameAssets.instance.gridSize.x / 2) - 0.5f, (GameAssets.instance.gridSize.y / 2) - 0.5f, -10);
        board = new Board(GameAssets.instance.gridSize);
        Instantiate(myMasterMenu, new Vector3(0, 0, 0), Quaternion.identity); //Create the menu and place it in its default position
        MasterMenuBehavior.OnClickedSettings += ClickedSettings; //Subscribe your function to be notified when setting button is clicked
        MasterMenuBehavior.OnClickedBack += ClickedBack; //Subscribe your function to be notified when back button is clicked
        myMasterMenu.GetComponent<MasterMenuBehavior>().SetTitleText("Snake");
        Debug.Log(myMasterMenu.GetComponent<MasterMenuBehavior>().GetTitleText());
        snake.Setup(board);
        board.Setup(snake);
    }

    void ClickedSettings() //Handle Settings Button Clicks Here
    {
        Debug.Log("Clicked Settings Button");
    }

    void ClickedBack() //Handle Back Button Clicks Here
    {
        Debug.Log("Clicked Back Button");
    }

}
