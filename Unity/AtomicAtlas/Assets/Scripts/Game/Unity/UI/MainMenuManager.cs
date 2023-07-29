using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager GlobalInstance
    {
        get;
        private set;
    }

    public event EventHandler OnClickStartEvent;
    public event EventHandler OnClickQuitEvent;

    [SerializeField]
    private GameObject mainMenu;

    void Start()
    {
        GlobalInstance = this;
    }

    void Update()
    {
        
    }

    public void SetMainMenuActive(bool active)
    {
        mainMenu.SetActive(active);
    }

    public void OnClickStart()
    {
        OnClickStartEvent?.Invoke(this, new EventArgs());
    }

    public void OnClickQuit()
    {
        OnClickQuitEvent?.Invoke(this, new EventArgs());
    }
}
