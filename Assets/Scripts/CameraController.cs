using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [Header("Camera Positions")]
    [SerializeField] private Transform deskView;
    [SerializeField] private Transform computerView;

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("Desktop")]
    [SerializeField] private DesktopSceneLoader sceneLoader;

    [Header("Optional")]
    [SerializeField] private GameObject officeCanvas;

    private bool atComputer = false;
    private bool moving = false;

    private void Start()
    {
        transform.position = deskView.position;
        transform.rotation = deskView.rotation;
    }

    public void GoToComputer()
    {
        if (moving || atComputer)
            return;

        StartCoroutine(OpenComputer());
    }

    public void LeaveComputer()
    {
        if (moving || !atComputer)
            return;

        StartCoroutine(CloseComputer());
    }

    private IEnumerator OpenComputer()
    {
        moving = true;
        atComputer = true;

        // Move camera to the monitor
        yield return StartCoroutine(MoveCamera(computerView));

        // Hide office UI
        if (officeCanvas != null)
            officeCanvas.SetActive(false);

        // Load the desktop scene
        if (sceneLoader != null)
            yield return StartCoroutine(sceneLoader.ShowDesktop());

        moving = false;
    }

    private IEnumerator CloseComputer()
    {
        moving = true;

        // Unload the desktop scene
        if (sceneLoader != null)
            yield return StartCoroutine(sceneLoader.HideDesktop());

        // Show office UI
        if (officeCanvas != null)
            officeCanvas.SetActive(true);

        // Move camera back
        yield return StartCoroutine(MoveCamera(deskView));

        atComputer = false;
        moving = false;
    }

    private IEnumerator MoveCamera(Transform target)
    {
        while (Vector3.Distance(transform.position, target.position) > 0.01f ||
               Quaternion.Angle(transform.rotation, target.rotation) > 0.5f)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                target.position,
                Time.deltaTime * moveSpeed);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                target.rotation,
                Time.deltaTime * moveSpeed);

            yield return null;
        }

        transform.position = target.position;
        transform.rotation = target.rotation;
    }

    private void Update()
    {
        if (atComputer && !moving && Input.GetKeyDown(KeyCode.Escape))
        {
            LeaveComputer();
        }
    }
}