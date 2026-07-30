using UnityEngine;
using UnityEngine.SceneManagement;

public class DesktopCameraController : MonoBehaviour
{
    public Camera officeCamera;

    private Camera desktopCamera;


    public void EnterDesktop()
    {
        StartCoroutine(LoadDesktop());
    }


    private System.Collections.IEnumerator LoadDesktop()
    {
        AsyncOperation load =
            SceneManager.LoadSceneAsync(
                "DesktopInterface",
                LoadSceneMode.Additive
            );


        while (!load.isDone)
        {
            yield return null;
        }


        // Find camera from loaded scene
        desktopCamera =
            GameObject.FindGameObjectWithTag("DesktopCamera")
                .GetComponent<Camera>();


        // Disable office camera
        officeCamera.enabled = false;


        // Enable desktop camera
        desktopCamera.enabled = true;


        Debug.Log("Desktop camera activated");
    }



    public void ExitDesktop()
    {
        if(desktopCamera != null)
        {
            desktopCamera.enabled = false;
        }


        officeCamera.enabled = true;


        SceneManager.UnloadSceneAsync(
            "DesktopInterface"
        );
    }
}