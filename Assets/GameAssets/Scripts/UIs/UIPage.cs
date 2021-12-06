using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
public class UIPage : MonoBehaviour
{
    public UIScreenType uiScreenType;
    public bool needDestroy;
    public List<Button> listButtons;

    private RectTransform rect;
    protected virtual void Awake()
    {
        rect = GetComponent<RectTransform>();
        
    }

    private void Start()
    {
        if (listButtons != null && listButtons.Count > 0)
        {
            for (int i = 0; i < listButtons.Count; i++)
            {
                Button btn = listButtons[i];
                btn.onClick.AddListener(() => OnClickButton(ref btn));
            }
        }
    }

    protected virtual void OnEnable()
    {
        ResetSize();
    }

    public virtual void Popuplate(UIPageData data)
    {
        gameObject.SetActive(true);
    }
    
    [Button]
    public void GetButtons()
    {
        listButtons.Clear();
        Button[] arrButtons = transform.GetComponentsInChildren<Button>();
        for (int i = 0; i < arrButtons.Length; i++)
        {
            Button btn = arrButtons[i];
            if (btn != null)
            {
                listButtons.Add(btn);
            }
        }
    }

    protected virtual void CloseMe()
    {
    }

    protected virtual void OnClickButton(ref Button btn)
    {
        Debug.Log("OnClickButton");
    }

    public void ResetSize()
    {
        if (rect != null)
        {
            rect.sizeDelta = Vector2.zero;
            rect.anchoredPosition3D = Vector3.zero;
        }
    }

    public virtual void Hide()
    {
        if (needDestroy)
        {
            DestroyImmediate(this.gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    protected virtual void OnDestroy()
    {
        for (int i = 0; i < listButtons.Count; i++)
        {
            listButtons[i].onClick.RemoveAllListeners();
        }
    }
}
