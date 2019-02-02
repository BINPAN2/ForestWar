using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Common;

public class GamePanel : BasePanel {

    private Text timer;
    private int time = -1;

    private Button successBtn;
    private Button failBtn;

    private void Start()
    {
        timer = transform.Find("Timer").GetComponent<Text>();
        successBtn = transform.Find("SuccessButton").GetComponent<Button>();
        successBtn.onClick.AddListener(OnResultClick);
        successBtn.gameObject.SetActive(false);
        failBtn = transform.Find("FailButton").GetComponent<Button>();
        failBtn.onClick.AddListener(OnResultClick); 
        failBtn.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (time > -1)
        {
            ShowTime(time);
            time = -1;
        }
    }

    public override void OnEnter()
    {
        transform.gameObject.SetActive(true);
    }

    public override void OnExit()
    {
        base.OnExit();
        transform.gameObject.SetActive(false);
        successBtn.gameObject.SetActive(false);
        failBtn.gameObject.SetActive(false);
    }


    private void OnResultClick()
    {
        uiMng.PopPanel();
        uiMng.PopPanel();
        GameFacade.Instance.GameOver();
    }

    public void ShowTimeAsync(int time)
    {
        this.time = time;
    }

    public void ShowTime(int time)
    {
        timer.gameObject.SetActive(true);
        timer.text = time.ToString();
        timer.transform.localScale = Vector3.one;
        Color tempColor = timer.color;
        tempColor.a = 1;
        timer.color = tempColor;
        timer.transform.DOScale(2, 0.3f).SetDelay(0.3f);
        timer.DOFade(0, 0.3f).SetDelay(0.3f).OnComplete(()=>timer.gameObject.SetActive(false));
        facade.PlayNormalSound(AudioManager.Sound_Alert);
    }


    public void OnGameOverResponse(ReturnCode returnCode)
    {
        Button tempBtn = null;
        switch (returnCode)
        {
            case ReturnCode.Success:
                successBtn.gameObject.SetActive(true);
                tempBtn = successBtn;
                break;
            case ReturnCode.Failed:
                failBtn.gameObject.SetActive(true);
                tempBtn = failBtn;
                break;
            default:
                break;
        }
        tempBtn.transform.localScale = Vector3.zero;
        tempBtn.transform.DOScale(1, 0.5f);
    }
}
