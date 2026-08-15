using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DesktopInterface : MonoBehaviour
{
    // Screen Animations
    [SerializeField] private Animator animScreenOn;
    [SerializeField] private Animator animScreenOff;
    
    [SerializeField] private Animator animSystemLoad;
    [SerializeField] private Animator animOSLogo;
    [SerializeField] private Image bgSystemLoad;

    [SerializeField] private GameObject windowLayer;
    [SerializeField] private GameObject shortcutLayer;
    
    // Applications
    // Blank
    [SerializeField] private DesktopWindow windowPrefab; 
    
    // Minesweeper
    [SerializeField] private Minesweeper minesweeperPrefab;
    Minesweeper currentMinesweeper;
    
    // Email
    [SerializeField] private EmailWindow emailWindowPrefab;
    EmailWindow currentEmailWindow;
    
    void Start()
    {
        animScreenOn.gameObject.SetActive(true);
        animSystemLoad.gameObject.SetActive(true);
        animOSLogo.gameObject.SetActive(true);
        bgSystemLoad.gameObject.SetActive(true);
        //animScreenOff.gameObject.SetActive(true);
        
        StartCoroutine(InterfaceOpen());
    }

    public IEnumerator InterfaceOpen()
    {
        yield return new WaitForSeconds(0.5f);
        ScreenOnAnim();
        StartCoroutine(RunOnAnimFinish(animScreenOn, "DesktopBoot", SystemLoad, 0.25f));
    }
    
    public IEnumerator RunOnAnimFinish(Animator animator, string stateName, Action method, float delaySeconds)
    {
        //Debug.Log(method.Method.Name + "Awaiting " + animator.name + " - " + stateName + " Finish");
        yield return null;
        
        while (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(delaySeconds);

        //Debug.Log(animator.name + " - " + stateName + " Finished");
        method.Invoke();
    }

    public void ScreenOnAnim()
    {
        //Debug.Log("Desktop Interface - ScreenOnAnim");
        animScreenOn.gameObject.SetActive(true);
        animScreenOn.Play("DesktopBoot");
    }

    public void SystemLoad()
    {
        //Debug.Log("Desktop Interface - SystemLoad");
        bgSystemLoad.gameObject.SetActive(true);
        animOSLogo.gameObject.SetActive(true);
        animOSLogo.Play("OSLogo");
        animSystemLoad.gameObject.SetActive(true);
        animSystemLoad.Play("LoadingBarFill");
        StartCoroutine(RunOnAnimFinish(animSystemLoad, "LoadingBarFill", SystemLoaded, 0.5f));
    }

    public void SystemLoaded()
    {
        //Debug.Log("Desktop Interface - SystemLoaded");
        bgSystemLoad.gameObject.SetActive(false);
        animOSLogo.gameObject.SetActive(false);
        animSystemLoad.gameObject.SetActive(false);
    }

    public static void InterfaceClose()
    {
        //Debug.Log("InterfaceClose");
        GameSceneLoader.Instance.LoadScene("Office");
    }

    public void MinesweeperShortcut()
    {
        if (currentMinesweeper == null)
        {
            currentMinesweeper = Instantiate(minesweeperPrefab, windowLayer.transform);
            Minesweeper.onMinesweeperClose.AddListener(MinesweeperClosed);
        }
    }

    public void MinesweeperClosed()
    {
        currentMinesweeper = null;
    }

    public void CreatePlaceholderWindow()
    {
        //Debug.Log("Desktop Interface - CreatePlaceholderWindow");
        DesktopWindow window = Instantiate(windowPrefab, windowLayer.transform);
    }
    
    public void EmailShortcut()
    {
        if (currentEmailWindow == null)
        {
            //currentEmailWindow = Instantiate(emailWindowPrefab, new Vector3(0,0,0), new Quaternion(),windowLayer.transform);
            currentEmailWindow = Instantiate(emailWindowPrefab, windowLayer.transform);
            currentEmailWindow.onWindowClose += EmailWindowClosed;
        }
    }

    public void EmailWindowClosed()
    {
        currentEmailWindow = null;
    }
}