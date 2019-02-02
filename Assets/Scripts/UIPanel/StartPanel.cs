using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartPanel : BasePanel {
    private Button loginBtn;
    private Animator animator;

    public override void OnEnter()
    {
        base.OnEnter();
        loginBtn = transform.Find("LoginButton").GetComponent<Button>();
        animator = transform.Find("LoginButton").GetComponent<Animator>();
        loginBtn.onClick.AddListener(OnLoginBtnClick);
    }

    public void OnLoginBtnClick()
    {
        PlayButtonClickSound();
        uiMng.PushPanel(UIPanelType.Login);
    }

    public override void OnPause()
    {
        base.OnPause();
        animator.enabled = false;
        Tweener tweener =  loginBtn.transform.DOScale(0, 0.4f);
        tweener.OnComplete(() => loginBtn.gameObject.SetActive(false));
    }

    public override void OnResume()
    {
        base.OnResume();
        loginBtn.gameObject.SetActive(true);
        Tweener tweener = loginBtn.transform.DOScale(1, 0.4f);
        animator.enabled = true;

    }
}
