using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private void Start()
    {
        StudioManager.Instance.CreateNewProject();

        //Debug.Log("GameManager - Start - New Studio Created");
    }
}