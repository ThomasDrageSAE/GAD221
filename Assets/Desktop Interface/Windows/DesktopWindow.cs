using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DesktopWindow : UIComponent
{
    public event Action onWindowOpen;
    public event Action onWindowMinimize;
    public event Action onWindowMaximize;
    public event Action onWindowClose;
    
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void Open()
    {
        Debug.Log("Window - Open");
        onWindowOpen?.Invoke();
        //Anim
    }
    
    public void Minimize()
    {
        Debug.Log("Window - Minimize");
        onWindowMinimize?.Invoke();
    }

    public void Maximize()
    {
        Debug.Log("Window - Maximize");
        onWindowMaximize?.Invoke();
    }


    public void Close()
    {
        Debug.Log("Window - Close");
        onWindowClose?.Invoke();
        Destroy(gameObject);
    }
}
