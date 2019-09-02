using UnityEngine;
using System.Collections;
//会员资料
public class ZhuanZhangXiaJiPanel : UserSubPanelBase {

    //按钮
    public GameObject ReturnBtn;

    public UIPopupList transferMethod;
    public UIInput rec;
    public UIInput money;
    public UIInput password;

    public GameObject transferBtn;

    void Start() {
        UIEventListener.Get(ReturnBtn).onClick = OnReturn;
        UIEventListener.Get(transferBtn).onClick = OnTransfer;
    }

    // Update is called once per frame
    void Update() {
    }

    void OnReturn(GameObject go)
    {
        gameObject.SetActive(false);
    }

    void OnTransfer(GameObject go)
    {
        if (string.IsNullOrEmpty(money.value))
        {
            msgSignal.Dispatch(new MsgPara("请输入金额", 2));
            return;
        }
        if (string.IsNullOrEmpty(password.value))
        {
            msgSignal.Dispatch(new MsgPara("请输入密码", 2));
            return;
        }
        if (string.IsNullOrEmpty(rec.value))
        {
            msgSignal.Dispatch(new MsgPara("请输入接收账号或者ID", 2));
            return;
        }



        // 通过账号充值
        //通过ID充值
        bool isId = true;
        if (transferMethod.value.Equals("通过账号充值"))
        {
            isId = false;
        }

        if (isId)
        {
            if (!Util.isNumberic(rec.value))
            {
                msgSignal.Dispatch(new MsgPara("请输入正确的ID", 2));
                return;
            }

            int id = int.Parse(rec.value);

            NetworkManager.Instance.MemberTransfer(int.Parse(money.value)*100, password.value, (uint)id);
        }
        else
        {
            NetworkManager.Instance.MemberTransfer(int.Parse(money.value) * 100, password.value, 0, rec.value);
        }
        money.value = "";
        password.value = "";
        rec.value = "";

        AudioController.Instance.SoundPlay("active_item");

    }

    public void Show()
    {

        gameObject.SetActive(true);
        password.value = "";
        money.value = "";
        rec.value = "";
    }

}
