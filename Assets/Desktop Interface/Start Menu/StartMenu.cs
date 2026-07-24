using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
    
    // -- Resources & Prefabs --
    [SerializeField] private Animator animator;
    
    #endregion
    
    #region --- Properties & Variables ---
    
    // -- Public --
    public bool animPlaying;
    
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
        
    }

    private void EventUnsubscription()
    {
        
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
            OpenStartMenu();
        }
        else
        {
            CloseStartMenu();
        }
    }
    
    public void OpenStartMenu()
    {
        Debug.Log("StartMenu - OpenStartMenu");
        primaryMenu.SetActive(true);
        
        animator.Play("StartMenuOpen");
    }

    public void PrimaryMenuOpened() // once anim finished
    {
        Debug.Log("StartMenu - PrimaryMenuOpened");
        primaryMenuOpen = true;
    }

    public void CloseStartMenu()
    {
        Debug.Log("StartMenu - CloseStartMenu");
        animator.Play("StartMenuClose");
        CloseSubMenu();
    }

    public void PrimaryMenuClosed() // once anim finished
    {
        Debug.Log("StartMenu - PrimaryMenuClosed");
        primaryMenu.SetActive(false);
        primaryMenuOpen = false;
    }
    
    #endregion
    
    #region --- Sub Menu ---

    private void OpenSubMenu(SubMenu subMenu)
    {
        Debug.Log("StartMenu - OpenSubMenu");
        
        if (currentSubMenu != SubMenu.None)
        {
            CloseSubMenu();
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
        
        Debug.Log("StartMenu - Current Sub Menu: " + currentSubMenu);
    }

    private void SubMenuOpened() // once anim finished
    {
        
    }

    public void CloseSubMenu()
    {
        Debug.Log("StartMenu - CloseSubMenu");
        
        switch (currentSubMenu)
        {
            case SubMenu.None:
                break;
            case SubMenu.Shutdown:
                shutdownMenu.SetActive(false);
                
                break;
            case SubMenu.Game:
                gameMenu.SetActive(false);
                
                break;
            case SubMenu.Display:
                displayMenu.SetActive(false);
                
                break;
            case SubMenu.Audio:
                audioMenu.SetActive(false);
                
                break;
            case SubMenu.Credits:
                creditsMenu.SetActive(false);
                
                break;
        }
        
        currentSubMenu = SubMenu.None;
        
        Debug.Log("StartMenu - Current Sub Menu: " + currentSubMenu);
    }

    private void SubMenuClosed() // once anim finished.
    {
        
    }
    
    public void ShutdownButton()
    {
        Debug.Log("StartMenu - ShutdownButton");

        if (currentSubMenu != SubMenu.Shutdown)
        {
            OpenSubMenu(SubMenu.Shutdown);
        }

        else
        {
            CloseSubMenu();
        }
    }
    
    public void GameButton()
    {
        Debug.Log("StartMenu - GameButton");
        
        if (currentSubMenu != SubMenu.Game)
        {
            OpenSubMenu(SubMenu.Game);
        }

        else
        {
            CloseSubMenu();
        }
    }

    public void DisplayButton()
    {
        Debug.Log("StartMenu - DisplayButton");
        
        if (currentSubMenu != SubMenu.Display)
        {
            OpenSubMenu(SubMenu.Display);
        }

        else
        {
            CloseSubMenu();
        }
    }

    public void AudioButton()
    {
        Debug.Log("StartMenu - AudioButton");
        
        if (currentSubMenu != SubMenu.Audio)
        {
            OpenSubMenu(SubMenu.Audio);
        }

        else
        {
            CloseSubMenu();
        }
    }
    
    public void CreditsButton()
    {
        Debug.Log("StartMenu - CreditsButton");
        
        if (currentSubMenu != SubMenu.Credits)
        {
            OpenSubMenu(SubMenu.Credits);
        }

        else
        {
            CloseSubMenu();
        }
    }
    
    #endregion
    
    #region --- Animation ---
    
    #endregion
}