using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Common;

public class RegisterPanel : BasePanel {
    private InputField usernameIF;
    private InputField passwordIF;
    private InputField repasswordIF;

    private Button registerBtn;
    private Button closeBtn;

    private RegisterRequest registerRequest;

    private void Start()
    {
        registerRequest = GetComponent<RegisterRequest>();

        usernameIF = transform.Find("UsernameLabel/UsernameInputField").GetComponent<InputField>();
        passwordIF = transform.Find("PasswordLabel/PasswordInputField").GetComponent<InputField>();
        repasswordIF = transform.Find("RepeatPasswordLabel/RepeatPasswordInputField").GetComponent<InputField>();

        closeBtn = transform.Find("CloseButton").GetComponent<Button>();
        closeBtn.onClick.AddListener(OncloseBtnClick);
        registerBtn = transform.Find("RegistButton").GetComponent<Button>();
        registerBtn.onClick.AddListener(OnregistBtnClick);
    }


    private void OnregistBtnClick()
    {
        PlayButtonClickSound();
        string msg = "";
        if (string.IsNullOrEmpty(usernameIF.text))
        {
            msg += "用户名不能为空 ";
        }
        if (string.IsNullOrEmpty(passwordIF.text))
        {
            msg += "密码不能为空";
        }
        if (passwordIF.text != repasswordIF.text)
        {
            msg += "密码不一致";
        }
        if (msg != "")
        {
            uiMng.ShowMessage(msg);
            return;
        }

        //向服务器端发送信息验证
        registerRequest.SendRequest(usernameIF.text, passwordIF.text);
    }

    private void OncloseBtnClick()
    {
        PlayButtonClickSound();
        Tweener tweener = transform.DOScale(0, 0.5f);
        tweener.OnComplete(() => uiMng.PopPanel());
    }

    public void OnRegisterResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            uiMng.ShowMessageAsync("注册成功！");
            Debug.Log("注册成功！");
        }
        else
        {
            uiMng.ShowMessageAsync("注册失败，用户名重复！");
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        transform.gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.5f);
    }

    public override void OnExit()
    {
        base.OnExit();
        transform.gameObject.SetActive(false);
    }

}
