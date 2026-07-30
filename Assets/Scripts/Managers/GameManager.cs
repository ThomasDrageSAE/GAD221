using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        StudioManager.Instance.CreateNewProject();

        Debug.Log("New Studio Created");
    }
}