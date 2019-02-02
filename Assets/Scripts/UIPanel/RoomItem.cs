using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour {

    public Text Username;
    public Text TotalCount;
    public Text WinCount;
    public Button JoinBtn;
    private int Id;
    private RoomListPanel roomListPanel;

	void Start () {
        if (JoinBtn != null)
        {
            JoinBtn.onClick.AddListener(OnJoinBtnClick);
        }

	}

	
    private void OnJoinBtnClick()
    {
        roomListPanel.OnJoinBtnClick(Id);
    }


    public void SetRoomInfo(int id ,string username, int totalCount, int winCount,RoomListPanel roomListPanel)
    {
        Id = id;
        Username.text = username;
        TotalCount.text = "总场数\n"+totalCount.ToString();
        WinCount.text = "胜场数\n"+winCount.ToString();
        this.roomListPanel = roomListPanel;
    }
    public void SetRoomInfo(int id,string username, string totalCount, string winCount,RoomListPanel roomListPanel)
    {
        Id = id;
        Username.text = username;
        TotalCount.text = "总场数\n" + totalCount;
        WinCount.text = "胜场数\n"+ winCount;
        this.roomListPanel = roomListPanel;
    }

    public void DestroySelf()
    {
        GameObject.Destroy(this.gameObject);
    }
}
