using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class UINetworkRoomDialog : UIDialog
{
    public InputField inputFieldName;


    protected override void OnClickButton(ref Button btn)
    {
        base.OnClickButton(ref btn);
        if (btn.name.Equals("BtnPlay"))
        {
            string nickName = inputFieldName.text;
            PhotonNetwork.NickName = nickName;
            NetworkRoomManager.Instance.Connect();
        }
    }
}
