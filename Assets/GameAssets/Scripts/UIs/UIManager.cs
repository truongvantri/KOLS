using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

public class UIManager : TSingleton<UIManager>
{
    public string uiPrefabsPath;
    public List<UIPage> listUIPageStack;

    [Button]
    public void GetUIPrefabs()
    {
        //GameObject[] prefabs = Resources.LoadAll<GameObject>("Animations/Test");
        string[] listPath = AssetDatabase.FindAssets("t:Prefab");
        for (int i = 0; i < listPath.Length; i++)
        {
            string path = listPath[i];
            GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (obj != null)
            {
                UIPage uipage = obj.GetComponent<UIPage>();
                if (uipage != null)
                {
                    listUIPageStack.Add(uipage);

                }
            }
        }
    }
}
