using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using Common;

/// <summary>
/// 用来管理服务器端Socket的链接
/// </summary>
public class ClientManager : BaseManager {

    private static string IP = "127.0.0.1";
    private static int PORT = 6688;

    private Socket clientSocket;
    private Message msg = new Message();


    public ClientManager(GameFacade facade) : base(facade) { }

    public override void OnInit()
    {
        base.OnInit();
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            clientSocket.Connect(IP, PORT);
            Start();
        }
        catch(Exception e)
        {
            Debug.LogWarning("无法连接到服务器！" + e);
        }
    }

    private void Start()
    {
        if (clientSocket== null||clientSocket.Connected == false)
        {
            return;
        }
        clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, RecieveCallback, null);
    }

    private void RecieveCallback(IAsyncResult ar)
    {
        try
        {
            int count = clientSocket.EndReceive(ar);
            Debug.Log("接收到服务器端响应");
            msg.ReadMessage(count,OnprocessDataCallback);
            Start();
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }

    }

    private void OnprocessDataCallback(ActionCode actionCode,string data)
    {
        //TODO
        facade.HandleResponse(actionCode, data);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        try
        {
            clientSocket.Close();
        }
        catch(Exception e)
        {
            Debug.LogWarning("无法关闭与服务器的连接！" + e);
        }
    }


    public void SendRequest(RequestCode requestCode, ActionCode actionCode, string data)
    {
        byte[] bytes = Message.PackData(requestCode, actionCode, data);
        clientSocket.Send(bytes);
    }
}

