using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public class UIManager : TSingleton<UIManager>
{
    [Header("Load Runtime")]
    public string uiPrefabsPathLoadRuntime;
    public List<string> listPrefabPath;
    
    [Header("Load With Scene")]
    public string uiPrefabsPathLoadWithScene;
    public List<UIPage> listUIPagePrefabInScene;

    public List<UIPage> listUIShowing;

    protected override void Awake()
    {
        base.Awake();
        listUIShowing = new List<UIPage>();

        for (int i = 0; i < listUIPagePrefabInScene.Count; i++)
        {
            GameObject obj = listUIPagePrefabInScene[i].gameObject;
            if(obj != null)
                listUIPagePrefabInScene[i].gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.N))
        {
            ShowUIScreen<UIMessageDialog>(null,UICanvasOverlay.Instance.transform);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            HideUIScreen<UIMessageDialog>();
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            ShowUIScreen<UIMessageDialog2>(null,UICanvasOverlay.Instance.transform);
        }
        
        
        //handle button back
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            HideUIScreen();
        }
    }

    public void ShowUIScreen<T>(UIPageData uiPageData,Transform trans)
    {
        for (int i = 0; i < listUIPagePrefabInScene.Count; i++)
        {
            UIPage uiPage = listUIPagePrefabInScene[i];
            if (uiPage != null)
            {
                if (uiPage.GetType() == typeof(T) && !UIScreenIsShowing<T>())
                {
                    Debug.Log("Show Screen");
                    if (trans != null)
                    {
                        Transform uiPageTrans = uiPage.transform;
                        uiPageTrans.SetParent(trans);
                        uiPageTrans.localScale = Vector3.one;
                    }
                    uiPage.Popuplate(uiPageData);
                    listUIShowing.Insert(0,uiPage);
                    return;
                }
            }
        }
    }

    public void HideUIScreen<T>()
    {
        UIPage uiPage = GetUIScreenShowing<T>();
        if (uiPage != null)
        {
            uiPage.Hide();
            Transform uiPageTrans = uiPage.transform;
            uiPageTrans.SetParent(transform);
            uiPageTrans.localScale = Vector3.one;
            listUIShowing.RemoveAt(0);
        }
    }
    
    public void HideUIScreen()
    {
        if (listUIShowing.Count == 0)
        {
            Debug.Log("Don't have any popup showing");
            return;
        }
        
        UIPage uiPage = listUIShowing.ElementAt(0);
        if (uiPage != null)
        {
            uiPage.Hide();
            Transform uiPageTrans = uiPage.transform;
            uiPageTrans.SetParent(transform);
            uiPageTrans.localScale = Vector3.one;
            listUIShowing.RemoveAt(0);
        }
    }
    
    bool UIScreenIsShowing<T>()
    {
        return listUIShowing.Exists(t => t.GetType() == typeof(T));
    }

    UIPage GetUIScreenShowing<T>()
    {
        return listUIShowing.Find(t => t.GetType() == typeof(T));
    }

    void HandleButtonBack()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
        }
    }
    

    #region Editor
    #if UNITY_EDITOR
    [Button]
    public void GetUIPrefabsLoadWithScene()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            DestroyImmediate(transform.GetChild(i).transform.gameObject);
        }
        
        if (listUIPagePrefabInScene != null && listUIPagePrefabInScene.Count > 0)
        {
            for (int i = listUIPagePrefabInScene.Count - 1; i >= 0; i--)
            {
                if(listUIPagePrefabInScene[i] != null)
                    DestroyImmediate(listUIPagePrefabInScene[i].gameObject);
            }
        }
        listUIPagePrefabInScene = new List<UIPage>();
        listUIPagePrefabInScene.Clear();
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { uiPrefabsPathLoadWithScene });
        foreach (string guid in guids)
        {
            string myObjectPath = AssetDatabase.GUIDToAssetPath(guid);
            Object[] myObjs = AssetDatabase.LoadAllAssetsAtPath(myObjectPath);
            GameObject prefab = Instantiate(UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(myObjectPath));
            if (prefab != null)
            {
                UIPage uiPage = prefab.GetComponent<UIPage>();
                if (uiPage != null)
                {
                    prefab.transform.SetParent(transform);
                    //uiPage.ResetSize();
                    //prefab.SetActive(false);
                    uiPage.GetButtons();
                    listUIPagePrefabInScene.Add(uiPage);
                    RectTransform rect = uiPage.GetComponent<RectTransform>();
                    if (rect != null)
                    {
                        rect.sizeDelta = Vector2.zero;
                        rect.anchoredPosition3D = Vector3.zero;
                    }
                }
            }
        }
    }
    
    //[Button]
    public void GetPathPrefabLoadRuntime()
    {
        listPrefabPath.Clear();
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { uiPrefabsPathLoadRuntime });
        foreach (string guid in guids)
        {
            string myObjectPath = AssetDatabase.GUIDToAssetPath(guid);
            listPrefabPath.Add(myObjectPath);
        }
    }
    #endif
    #endregion
    
    
}
