using UnityEngine;

public class ComputerInteract : MonoBehaviour
{
    public Office office;

    private void OnMouseDown()
    {
        office.GoToComputer();
    }
}
