using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [Header("Camera Positions")]
    public Transform deskView;
    public Transform computerView;

    [Header("Settings")]
    public float moveSpeed = 3f;

    [Header("UI")]
    public GameObject desktopUI;

    private bool atComputer = false;
    private bool moving = false;

    private void Start()
    {
        // Start at the desk view
        transform.position = deskView.position;
        transform.rotation = deskView.rotation;

        // Hide desktop UI
        if (desktopUI != null)
            desktopUI.SetActive(false);
    }

    public void GoToComputer()
    {
        if (moving || atComputer)
            return;

        atComputer = true;

        if (desktopUI != null)
            desktopUI.SetActive(true);

        StartCoroutine(MoveCamera(computerView));
    }

    public void LeaveComputer()
    {
        if (moving || !atComputer)
            return;

        atComputer = false;

        if (desktopUI != null)
            desktopUI.SetActive(false);

        StartCoroutine(MoveCamera(deskView));
    }

    IEnumerator MoveCamera(Transform target)
    {
        moving = true;

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

        moving = false;
    }

    private void Update()
    {
        if (atComputer && !moving && Input.GetKeyDown(KeyCode.Escape))
        {
            LeaveComputer();
        }
    }
}