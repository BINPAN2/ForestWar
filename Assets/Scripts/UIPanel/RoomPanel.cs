using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Common;

public class RoomPanel : BasePanel {

    private Text localPlayerUsername;
    private Text localPlayerTotalCount;
    private Text localPlayerWinCount;

    private Text enemyPlayerUsername;
    private Text enemyPlayerTotalCount;
    private Text enemyPlayerWinCount;

    private Transform startBtn;
    private Transform exitBtn;

    private Transform bluePanel;
    private Transform redPanel;

    private UserData ud;
    private UserData ud1;
    private UserData ud2;

    private QuitRoomRequest quitRoomRequest;
    private StartGameRequest startGameRequest;

    private bool isPopPanel =false;

    private void Start()
    {
        localPlayerUsername = transform.Find("BluePanel/Username").GetComponent<Text>();
        localPlayerTotalCount = transform.Find("BluePanel/TotalCount").GetComponent<Text>();
        localPlayerWinCount = transform.Find("BluePanel/WinCount").GetComponent<Text>();
        enemyPlayerUsername = transform.Find("RedPanel/Username").GetComponent<Text>();
        enemyPlayerTotalCount = transform.Find("RedPanel/TotalCount").GetComponent<Text>();
        enemyPlayerWinCount = transform.Find("RedPanel/WinCount").GetComponent<Text>();

        transform.Find("StartButton").GetComponent<Button>().onClick.AddListener(OnStartBtnClick);
        transform.Find("ExitButton").GetComponent<Button>().onClick.AddListener(OnExitBtnClick);

        startBtn = transform.Find("StartButton");
        exitBtn = transform.Find("ExitButton");
        bluePanel = transform.Find("BluePanel");
        redPanel = transform.Find("RedPanel");

        quitRoomRequest = GetComponent<QuitRoomRequest>();
        startGameRequest = GetComponent<StartGameRequest>();

    }

    private void Update()
    {
        if (ud != null)
        {
            SetLocalPlayerRes(ud.Username, ud.TotalCount.ToString(), ud.WinCount.ToString());
            ClearEnemyPlayerRes();
            ud = null;
        }
        if (ud1 !=null)
        {
            SetLocalPlayerRes(ud1.Username, ud1.TotalCount.ToString(), ud1.WinCount.ToString());
            if (ud2 !=null)
            {
                SetEnemyPlayerRes(ud2.Username, ud2.TotalCount.ToString(), ud2.WinCount.ToString());
            }
            else
            {
                ClearEnemyPlayerRes();
            }           
            ud1 = null;
            ud2 = null;
        }
        if (isPopPanel)
        {
            Tweener tweener = transform.DOScale(0, 0.5f);
            tweener.OnComplete(() => uiMng.PopPanel());
            isPopPanel = false;
        }
    }

    public void SetLocalPlayerResAsync()
    {
        ud = facade.GetUserData();
    }

    public void SetAllPlayerResAsync(UserData ud1,UserData ud2)
    {
        this.ud1 = ud1;
        this.ud2 = ud2;
    }

    public  void SetLocalPlayerRes(string username , string totalCount,string winCount)
    {
        localPlayerUsername.text = username;
        localPlayerTotalCount.text = "总场数："+totalCount;
        localPlayerWinCount.text = "胜场数："+winCount;
    }

    private void SetEnemyPlayerRes(string username ,string totalCount,string winCount)
    {
        enemyPlayerUsername.text = username;
        enemyPlayerTotalCount.text = "总场数：" + totalCount;
        enemyPlayerWinCount.text = "胜场数：" + winCount;
    }

    public void ClearEnemyPlayerRes()
    {
        enemyPlayerUsername.text = "";
        enemyPlayerTotalCount.text = "等待玩家加入...";
        enemyPlayerWinCount.text = "" ;
    }

    private void OnStartBtnClick()
    {
        PlayButtonClickSound();
        startGameRequest.SendRequest();
    }

    public void OnStartResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Failed)
        {
            uiMng.ShowMessageAsync("您不是房主，无法开始游戏！");
        }
        else
        {
            uiMng.PushPanelAsync(UIPanelType.Game);
            facade.EnterPlayingAsync();
        }
    }

    private void OnExitBtnClick()
    {
        PlayButtonClickSound();
        quitRoomRequest.SendRequest();
    }

    public  void OnExitResponse()
    {
        isPopPanel = true;
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
