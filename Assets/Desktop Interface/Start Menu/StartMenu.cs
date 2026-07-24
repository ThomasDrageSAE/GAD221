using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StartMenu : UIComponent
{
    #region --- Inspector References ---
    
    // -- In Scene --
    [SerializeField] public GameObject primaryMenu;
    
    [SerializeField] public GameObject shutdownMenu;
    [SerializeField] public GameObject gameMenu;
    [SerializeField] public GameObject displayMenu;
    [SerializeField] public GameObject audioMenu;
    [SerializeField] public GameObject creditsMenu;
    
    [SerializeField] private Animator primaryMenuAnimator;
    [SerializeField] private Animator subMenuAnimator;
    
    // -- Resources & Prefabs --
    
    #endregion
    
    #region --- Properties & Variables ---
    
    // -- Public --
    
    // -- Private --
    private bool primaryMenuOpen;
    private bool subMenuOpen;

    private enum SubMenu
    {
        None, Shutdown, Game, Display, Audio, Credits
    }

    private SubMenu currentSubMenu;
    
    #endregion
    
    #region --- Events ---
    
    public UnityEvent onMenuOpen = new UnityEvent();
    public UnityEvent onMenuClose = new UnityEvent();
    public UnityEvent onSubMenuOpen = new UnityEvent();
    public UnityEvent onSubMenuClose = new UnityEvent();
    
    private void EventSubscription()
    {
        SubMenus.subMenusOpened += SubMenuOpened;
        SubMenus.subMenusClosed += SubMenuClosed;
    }

    private void EventUnsubscription()
    {
        SubMenus.subMenusOpened -= SubMenuOpened;
        SubMenus.subMenusClosed -= SubMenuClosed;
    }

    #endregion

    #region --- Initialization & Termination ---

    private void Start()
    {
        EventSubscription();

        primaryMenu.gameObject.SetActive(false);
        primaryMenuOpen = false;
        currentSubMenu = SubMenu.None;
        shutdownMenu.SetActive(false);
        gameMenu.SetActive(false);
        displayMenu.SetActive(false);
        audioMenu.SetActive(false);
        creditsMenu.SetActive(false);
    }

    private void OnDestroy()
    {
        EventUnsubscription();
        
        
    }

    #endregion
    
    #region --- Primary Menu ---

    public void MenuButtonClicked()
    {
        Debug.Log("StartMenu - MenuButtonClicked");

        if (!primaryMenuOpen)
        {
            StartCoroutine(OpenStartMenu());
        }
        else
        {
            StartCoroutine(CloseStartMenu());
        }
    }

    public void Open()
    {
        StartCoroutine(OpenStartMenu());
    }
    
    public IEnumerator OpenStartMenu()
    {
        Debug.Log("StartMenu - OpenStartMenu");
        primaryMenu.SetActive(true);
        
        primaryMenuAnimator.Play("StartMenuOpen");
        DisableInteraction();
        yield return new WaitUntil(() => primaryMenuOpen);
        EnableInteraction();
    }

    public void PrimaryMenuOpened() // once anim finished
    {
        Debug.Log("StartMenu - PrimaryMenuOpened");
        primaryMenuOpen = true;
    }

    public void Close()
    {
        StartCoroutine(CloseStartMenu());
    }

    public IEnumerator CloseStartMenu()
    {
        Debug.Log("StartMenu - CloseStartMenu");
        
        yield return StartCoroutine(CloseSubMenu());
        
        primaryMenuAnimator.Play("StartMenuClose");
        DisableInteraction();
        yield return new WaitUntil(() => !primaryMenuOpen);
        EnableInteraction();
    }

    public void PrimaryMenuClosed() // once anim finished
    {
        Debug.Log("StartMenu - PrimaryMenuClosed");
        primaryMenu.SetActive(false);
        primaryMenuOpen = false;
    }
    
    #endregion
    
    #region --- Sub Menu ---

    private IEnumerator OpenSubMenu(SubMenu subMenu)
    {
        Debug.Log("StartMenu - OpenSubMenu");
        
        if (currentSubMenu != SubMenu.None)
        {
            yield return StartCoroutine(CloseSubMenu());
        }
        
        currentSubMenu = subMenu;
        
        switch (currentSubMenu)
        {
            case SubMenu.None:
                gameMenu.SetActive(true);
                break;
            case SubMenu.Shutdown:
                shutdownMenu.SetActive(true);
                break;
            case SubMenu.Game:
                gameMenu.SetActive(true);
                break;
            case SubMenu.Display:
                displayMenu.SetActive(true);
                break;
            case SubMenu.Audio:
                audioMenu.SetActive(true);
                break;
            case SubMenu.Credits:
                creditsMenu.SetActive(true);
                break;
        }
        
        subMenuAnimator.Play("SubMenusOpen");
        DisableInteraction();
        yield return new WaitUntil(() => subMenuOpen);
        EnableInteraction();
    }

    private void SubMenuOpened() // once anim finished
    {
        Debug.Log("StartMenu - SubMenuOpened");
        subMenuOpen = true;
        Debug.Log("StartMenu - Current Sub Menu: " + currentSubMenu);
    }

    public IEnumerator CloseSubMenu()
    {
        Debug.Log("StartMenu - CloseSubMenu");
        
        subMenuAnimator.Play("SubMenusClose");
        DisableInteraction();
        yield return new WaitUntil (() => !subMenuOpen);
        EnableInteraction();
    }

    private void SubMenuClosed() // once anim finished.
    {
        Debug.Log("StartMenu - SubMenuClosed");
        
        shutdownMenu.SetActive(false);
        gameMenu.SetActive(false);
        displayMenu.SetActive(false);
        audioMenu.SetActive(false);
        creditsMenu.SetActive(false);
        
        currentSubMenu = SubMenu.None;
        subMenuOpen = false;
    }
    
    public void ShutdownButton()
    {
        Debug.Log("StartMenu - ShutdownButton");

        if (currentSubMenu != SubMenu.Shutdown)
        {
            StartCoroutine(OpenSubMenu(SubMenu.Shutdown));
        }

        else
        {
            StartCoroutine(CloseSubMenu());
        }
    }
    
    public void GameButton()
    {
        Debug.Log("StartMenu - GameButton");
        
        if (currentSubMenu != SubMenu.Game)
        {
            StartCoroutine(OpenSubMenu(SubMenu.Game));
        }

        else
        {
            StartCoroutine(CloseSubMenu());
        }
    }

    public void DisplayButton()
    {
        Debug.Log("StartMenu - DisplayButton");
        
        if (currentSubMenu != SubMenu.Display)
        {
            StartCoroutine(OpenSubMenu(SubMenu.Display));
        }

        else
        {
            StartCoroutine(CloseSubMenu());
        }
    }

    public void AudioButton()
    {
        Debug.Log("StartMenu - AudioButton");
        
        if (currentSubMenu != SubMenu.Audio)
        {
            StartCoroutine(OpenSubMenu(SubMenu.Audio));
        }

        else
        {
            StartCoroutine(CloseSubMenu());
        }
    }
    
    public void CreditsButton()
    {
        Debug.Log("StartMenu - CreditsButton");
        
        if (currentSubMenu != SubMenu.Credits)
        {
            StartCoroutine(OpenSubMenu(SubMenu.Credits));
        }

        else
        {
            StartCoroutine(CloseSubMenu());
        }
    }
    
    #endregion
    
    #region --- Interaction ---

    public void EnableInteraction()
    {
        UIComponent[] uiComponents = GetComponentsInChildren<UIComponent>();

        foreach (UIComponent uiComponent in uiComponents)
        {
            uiComponent.ResumeInteraction();
        }
        
        Debug.Log("StartMenu - EnableInteraction");
    }

    public void DisableInteraction()
    {
        UIComponent[] uiComponents = GetComponentsInChildren<UIComponent>();

        foreach (UIComponent uiComponent in uiComponents)
        {
            uiComponent.PauseInteraction();
        }
        
        Debug.Log("StartMenu - DisableInteraction");
    }
    #endregion
}