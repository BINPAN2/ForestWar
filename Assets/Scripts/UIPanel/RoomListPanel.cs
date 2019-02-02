using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Common;

public class RoomListPanel : BasePanel {

    private VerticalLayoutGroup roomLayout;
    private GameObject roomItemPrefab;
    private List<UserData> udList = null;
    private ListRoomRequest listRoomRequest;
    private CreateRoomRequest createRoomRequest;
    private JoinRoomRequest joinRoomRequest;

    private UserData ud1 = null;
    private UserData ud2 = null;
    

    private void Start()
    {
        transform.Find("RoomList/CloseButton").GetComponent<Button>().onClick.AddListener(OncloseBtnClick);
        transform.Find("RoomList/CreateRoomButton").GetComponent<Button>().onClick.AddListener(OnCreateRoomBtnClick);
        transform.Find("RoomList/RefreshButton").GetComponent<Button>().onClick.AddListener(OnRefreshBtnClick);
        roomLayout = transform.Find("RoomList/ScrollRect/Layout").GetComponent<VerticalLayoutGroup>();
        roomItemPrefab = Resources.Load("UIPanel/Room") as GameObject;
        listRoomRequest = GetComponent<ListRoomRequest>();
        createRoomRequest = GetComponent<CreateRoomRequest>();
        joinRoomRequest = GetComponent<JoinRoomRequest>();
    }

    private void OncloseBtnClick()
    {
        PlayButtonClickSound();
        Tweener tweener = transform.DOScale(0, 0.5f);
        tweener.OnComplete(() => uiMng.PopPanel());
    }

    private void OnCreateRoomBtnClick()
    {
        PlayButtonClickSound();
        BasePanel panel = uiMng.PushPanel(UIPanelType.Room);
        createRoomRequest.SetRoomPanel(panel);
        createRoomRequest.SendRequest();
    }

    private void OnRefreshBtnClick()
    {
        PlayButtonClickSound();
        listRoomRequest.SendRequest();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        SetBattleRes();
        transform.gameObject.SetActive(true);
        transform.localScale = Vector3.zero;
        Tweener tweener = transform.DOScale(1, 0.5f);
        if (listRoomRequest == null)
        {
            listRoomRequest = GetComponent<ListRoomRequest>();
        }
        tweener.OnComplete(() => listRoomRequest.SendRequest());
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
        Tweener tweener= transform.DOScale(1, 0.4f);
        tweener.OnComplete(() => listRoomRequest.SendRequest());
    }

    private void Update()
    {
        if (udList != null)
        {
            LoadRoomItem(udList);
            udList = null;
        }
        if (ud1 !=null &&ud2 != null)
        {
            BasePanel panel = uiMng.PushPanel(UIPanelType.Room);
            (panel as RoomPanel).SetAllPlayerResAsync(ud1, ud2);
            ud1 = null;
            ud2 = null;
        }
    }

    public void OnUpdateResultResponse(int totalCount,int winCount)
    {
        facade.UpdateResult(totalCount, winCount);
        SetBattleRes();
    }

    private void SetBattleRes()
    {
        transform.Find("BattleRes/Username").GetComponent<Text>().text = facade.GetUserData().Username;
        transform.Find("BattleRes/TotalCount").GetComponent<Text>().text = "总场数："+facade.GetUserData().TotalCount.ToString();
        transform.Find("BattleRes/WinCount").GetComponent<Text>().text ="胜场数："+ facade.GetUserData().WinCount.ToString();
    }

    public void LoadRoomItemAsync(List<UserData> udList)
    {
        this.udList = udList;
    }

    private void LoadRoomItem(List<UserData> udList)
    {
        RoomItem[] riArray = GetComponentsInChildren<RoomItem>();
        foreach (RoomItem ri in riArray)
        {
            ri.DestroySelf();
        }
        int count = udList.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject roomItem = GameObject.Instantiate(roomItemPrefab);
            roomItem.transform.SetParent(roomLayout.transform);
            roomItem.GetComponent<RoomItem>().SetRoomInfo(udList[i].Id,udList[i].Username, udList[i].TotalCount, udList[i].WinCount,this);
        }
        int roomCount = GetComponentsInChildren<RoomItem>().Length;
        Vector2 size = roomLayout.GetComponent<RectTransform>().sizeDelta;
        roomLayout.GetComponent<RectTransform>().sizeDelta = new Vector2(size.x, roomCount * (roomItemPrefab.GetComponent<RectTransform>().sizeDelta.y + roomLayout.spacing));
    }

    public  void OnJoinBtnClick(int id)
    {
        joinRoomRequest.SendRequest(id);
    }

    public void OnJoinResponse(ReturnCode returnCode,UserData ud1 , UserData ud2)
    {
        switch (returnCode)
        {
            case ReturnCode.Success:
                this.ud1 = ud1;
                this.ud2 = ud2;
                break;
            case ReturnCode.Failed:
                uiMng.ShowMessageAsync("房间已满，无法加入");
                break;
            case ReturnCode.NotFound:
                uiMng.ShowMessageAsync("房间被销毁无法加入");
                break;
            default:
                break;
        }
    }
}
