using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DesktopSceneLoader : MonoBehaviour
{
    [SerializeField] private string desktopSceneName = "DesktopInterface";

    [SerializeField] private Camera officeCamera;

    [Header("Office UI")]
    [SerializeField] private GameObject officeUI;


    private Camera desktopCamera;

    private bool isLoaded;



    public IEnumerator ShowDesktop()
    {
        if (isLoaded)
            yield break;


        // Hide office UI
        if(officeUI != null)
        {
            officeUI.SetActive(false);
        }


        AsyncOperation load = SceneManager.LoadSceneAsync(
            desktopSceneName,
            LoadSceneMode.Additive
        );


        while (!load.isDone)
            yield return null;



        GameObject cameraObject = GameObject.Find("DesktopCamera");


        if(cameraObject != null)
        {
            desktopCamera = cameraObject.GetComponent<Camera>();
        }


        officeCamera.enabled = false;

        desktopCamera.enabled = true;


        isLoaded = true;
    }



    public IEnumerator HideDesktop()
    {
        if (!isLoaded)
            yield break;


        if(desktopCamera != null)
        {
            desktopCamera.enabled = false;
        }


        officeCamera.enabled = true;


        AsyncOperation unload =
            SceneManager.UnloadSceneAsync(desktopSceneName);


        while (!unload.isDone)
            yield return null;


        // Show office UI again
        if(officeUI != null)
        {
            officeUI.SetActive(true);
        }


        isLoaded = false;
    }
}