using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIPage : MonoBehaviour
{
    public UIScreenType uiScreenType;
    public Button[] listButtons;
    private void Awake()
    {
        if (listButtons != null && listButtons.Length > 0)
        {
            for (int i = 0; i < listButtons.Length; i++)
            {
                Button btn = listButtons[i];
                btn.onClick.AddListener(() => OnClickButton(ref btn));
            }
        }
    }

    private void OnEnable()
    {
        
    }

    protected void Popuplate(UIPageData data)
    {
                
    }
    
    [Button]
    public void GetButtons()
    {
        listButtons = FindObjectsOfType<Button>();
    }

    protected virtual void OnClickButton(ref Button btn)
    {
        
    }

    protected virtual void OnDestroy()
    {
        for (int i = 0; i < listButtons.Length; i++)
        {
            listButtons[i].onClick.RemoveAllListeners();
        }
    }
}
