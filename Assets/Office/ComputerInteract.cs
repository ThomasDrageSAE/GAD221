using UnityEngine;

public class ComputerInteract : MonoBehaviour
{
    public Office office;

    private void OnMouseDown()
    {
        if (StudioManager.Instance == null)
        {
            Debug.LogError("StudioManager.Instance is missing.");
            return;
        }

        if (!StudioManager.Instance.setupComplete)
        {
            Debug.Log("Complete setup before using the computer.");
            return;
        }

        if (office == null)
        {
            Debug.LogError("ComputerInteract: Office has not been assigned.");
            return;
        }

        office.GoToComputer();
    }
}