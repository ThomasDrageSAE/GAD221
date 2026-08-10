using UnityEngine;
using System.Collections;

public class Office : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Transform officeCamera;

    [Header("Camera Positions")]
    [SerializeField] private Transform deskView;
    [SerializeField] private Transform computerView;

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("Desktop")]
    [SerializeField] private string desktopSceneName = "DesktopInterface";

    [Header("Optional")]
    [SerializeField] private GameObject officeCanvas;

    private bool moving = false;

    private void Start()
    {
        // Show office UI
        if (officeCanvas != null)
        {
            officeCanvas.SetActive(true);
        }

        // If coming from desktop scene do zoom out.
        if (GameSceneLoader.Instance.previousGameScene == desktopSceneName)
        {
            officeCamera.position = computerView.position;
            officeCamera.rotation = computerView.rotation;

            StartCoroutine(LeaveComputer());
        }

        // If not set to deskView location.
        else
        {
            officeCamera.position = deskView.position;
            officeCamera.rotation = deskView.rotation;
        }
    }

    public void GoToComputer()
    {
        if (moving)
        {
            return;
        }

        StartCoroutine(OpenComputer());
    }

    private IEnumerator OpenComputer()
    {
        moving = true;

        // Move camera to monitor
        yield return StartCoroutine(MoveCamera(computerView));

        // Hide office UI
        if (officeCanvas != null)
        {
            officeCanvas.SetActive(false);
        }

        // Load the desktop scene
        GameSceneLoader.Instance.LoadScene(desktopSceneName);
    }

    private IEnumerator LeaveComputer()
    {
        moving = true;

        yield return null;
        
        // Move camera away from monitor
        yield return StartCoroutine(MoveCamera(deskView));

        moving = false;
    }

    private IEnumerator MoveCamera(Transform target)
    {
        yield return null;
        
        while (Vector3.Distance(officeCamera.position, target.position) > 0.01f || Quaternion.Angle(officeCamera.rotation, target.rotation) > 0.5f)
        {
            officeCamera.position = Vector3.Lerp(officeCamera.position, target.position,Time.deltaTime * moveSpeed);

            officeCamera.rotation = Quaternion.Slerp(officeCamera.rotation, target.rotation,Time.deltaTime * moveSpeed);

            yield return null;
        }

        officeCamera.position = target.position;
        officeCamera.rotation = target.rotation;
    }
}