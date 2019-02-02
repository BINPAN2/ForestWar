using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Common;

public class LoginPanel : BasePanel {

    private Button closeBtn;
    private InputField usernameIF;
    private InputField passwordIF;
    private Button loginBtn;
    private Button registerBtn;
    private LoginRequest loginRequest;


    private void Start()
    {
        usernameIF = transform.Find("UsernameLabel/UsernameInputField").GetComponent<InputField>();
        passwordIF = transform.Find("PasswordLabel/PasswordInputField").GetComponent<InputField>();

        loginRequest = GetComponent<LoginRequest>();

        closeBtn = transform.Find("CloseButton").GetComponent<Button>();
        closeBtn.onClick.AddListener(OncloseBtnClick);
        loginBtn = transform.Find("LoginButton").GetComponent<Button>();
        loginBtn.onClick.AddListener(OnloginBtnClick);
        registerBtn = transform.Find("RegisterButton").GetComponent<Button>();
        registerBtn.onClick.AddListener(OnregisterBtnClick);
    }


    private void OncloseBtnClick()
    {
        PlayButtonClickSound();
        Tweener tweener =  transform.DOScale(0, 0.5f);
        tweener.OnComplete(() => uiMng.PopPanel());
    }

    private void OnloginBtnClick()
    {
        PlayButtonClickSound();
        string msg = "";
        if (string.IsNullOrEmpty( usernameIF.text))
        {
            msg += "用户名不能为空 ";
        }
        if (string.IsNullOrEmpty(passwordIF.text))
        {
            msg += "密码不能为空";
        }
        if(msg != "")
        {
            uiMng.ShowMessage(msg);
            return;
        }

        //向服务器端发送信息验证
        loginRequest.SendRequest(usernameIF.text, passwordIF.text);
    }

    public void OnLoginResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Success)
        {
            uiMng.PushPanelAsync(UIPanelType.RoomList);
            Debug.Log("登录成功！");
        }
        else
        {
            uiMng.ShowMessageAsync("用户名或密码错误，无法登录，请重新输入！");
        }
    }

    private void OnregisterBtnClick()
    {
        PlayButtonClickSound();
        uiMng.PushPanel(UIPanelType.Register);
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


    public override void OnPause()
    {
        base.OnPause();
        Tweener tweener = transform.DOScale(0, 0.4f);
        tweener.OnComplete(() => gameObject.SetActive(false));
    }

    public override void OnResume()
    {
        base.OnResume();
        gameObject.SetActive(true);
        transform.DOScale(1, 0.4f);

    }
}
