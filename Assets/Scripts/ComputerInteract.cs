using UnityEngine;

public class ComputerInteract : MonoBehaviour
{
    public CameraController cameraController;

    private void OnMouseDown()
    {
        cameraController.GoToComputer();
    }
}