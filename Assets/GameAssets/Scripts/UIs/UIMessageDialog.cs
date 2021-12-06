using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMessageDialogData : UIPageData
{
    public Action callbackAction;
}

public class UIMessageDialog : UIDialog
{
    [SerializeField] private TMP_Text tmpTitle;
    [SerializeField] private TMP_Text tmpDes;
    [SerializeField] private Button btnPrimary;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Popuplate(UIPageData data)
    {
        base.Popuplate(data);
    }

    protected override void CloseMe()
    {
        base.CloseMe();
        UIManager.Instance.HideUIScreen<UIMessageDialog>();
    }

    protected override void OnClickButton(ref Button btn)
    {
        base.OnClickButton(ref btn);
        string name = btn.name;
        if (string.Equals(name,"BtnPrimary"))
        {
            CloseMe();
        }
    }
}
