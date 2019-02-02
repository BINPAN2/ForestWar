using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class GameFacade : MonoBehaviour {

    private static GameFacade _instance;
    public static GameFacade Instance
    {
        get
        {
            if (_instance==null)
            {
                GameObject obj = GameObject.Find("GameFacade");
                if (obj == null)
                {
                    return null;
                }
                _instance = obj.GetComponent<GameFacade>();
            }
            return _instance;
        }
    }

    private UIManager uiMng;
    private AudioManager audioMng;
    private CameraManager cameraMng;
    private PlayerManager playerMng;
    private RequestManager requestMng;
    private ClientManager clientMng;
    private bool isEnterPlaying = false;

    //private void Awake()
    //{
    //    if (_instance != null)
    //    {
    //        Destroy(this.gameObject);
    //    }
    //    _instance = this;
    //}

    private void Start()
    {
        InitManager();
    }

    private void Update()
    {
        UpdateManager();
        if (isEnterPlaying)
        {
            EnterPlaying();
            isEnterPlaying = false;
        }
    }

    private void OnDestroy()
    {
        DestroyManager();
    }

    public void InitManager()
    {
        uiMng = new UIManager(this);
        audioMng = new AudioManager(this);
        playerMng = new PlayerManager(this);
        requestMng = new RequestManager(this);
        cameraMng = new CameraManager(this);
        clientMng = new ClientManager(this);

        uiMng.OnInit();
        audioMng.OnInit();
        playerMng.OnInit();
        cameraMng.OnInit();
        requestMng.OnInit();
        clientMng.OnInit();

    }

    public void DestroyManager()
    {
        uiMng.OnDestroy();
        audioMng.OnDestroy();
        playerMng.OnDestroy();
        cameraMng.OnDestroy();
        requestMng.OnDestroy();
        clientMng.OnDestroy();
    }

    public void UpdateManager()
    {
        uiMng.Update();
        audioMng.Update();
        playerMng.Update();
        cameraMng.Update();
        requestMng.Update();
        clientMng.Update();
    }

    public void AddRequest(ActionCode actionCode,BaseRequest request)
    {
        requestMng.AddRequest(actionCode, request);
    }

    public void RemoveRequest(ActionCode actionCode)
    {
        requestMng.RemoveRequest(actionCode);
    }
    public void HandleResponse(ActionCode actionCode, string data)
    {
        requestMng.HandleResponse(actionCode, data);
    }

    public void ShowMessage(string msg)
    {
        uiMng.ShowMessage(msg);
    }

    public void SendRequest(RequestCode requestCode, ActionCode actionCode, string data)
    {
        clientMng.SendRequest(requestCode, actionCode, data);
    }

    public void PlayBgSound(string soundname)
    {
        audioMng.PlayBgSound(soundname);
    }

    public void PlayNormalSound(string soundname)
    {
        audioMng.PlayNormalSound(soundname);
    }

    public void SetUserData(UserData ud)
    {
        playerMng.UserData = ud;
    }
    
    public UserData GetUserData()
    {
        return playerMng.UserData;
    }

    public void SetCurrentRoleType(RoleType rt)
    {
        playerMng.SetCurrentRoleType(rt);
    }

    public GameObject GetCurrentRoleGameObject()
    {
        return playerMng.GetCurrentRoleGameObject();
    }

    public void EnterPlayingAsync()
    {
        isEnterPlaying = true;
    }

    public void EnterPlaying()
    {
        playerMng.SpawnRoles();
        cameraMng.FollowRole();
    }

    public void StarPlaying()
    {
        playerMng.AddControllScript();
        playerMng.CreateSyncRequest();
    }

    public void SendAttack(int damage)
    {
        playerMng.SendAttack(damage);
    }
    public void GameOver()
    {
        cameraMng.WalkthroughScene();
        playerMng.GameOver();
    }

    public void UpdateResult(int totalCount, int winCount)
    {
        playerMng.UpdateResult(totalCount, winCount);
    }
}
