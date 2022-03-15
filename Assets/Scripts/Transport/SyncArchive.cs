using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.Networking;
//using UnityEngine.Networking.Types;
//using UnityEngine.Networking.Match;
//using System.Collections.Generic;

public class SyncArchive : MonoBehaviour
{
//    public MainWindow m_MainWindow;
//    public InputField m_InputField;
//    public Text m_StateDisplay;
//    public GameObject m_CreateButton;
//    public GameObject m_JoinButton;
//    public GameObject m_ShutdownButton;
//    public GameObject m_SyncButton;

//    private const int VERSION_ID = 2;

//    // Do not remove or modify the exist protocols.
//    private enum Protocol
//    {
//        VersionCheckNtf = 10001,
//        SyncArchive1 = 20001,
//        SyncArchive2 = 20002,
//        SyncArchive3 = 20003,
//    }

//    private enum SyncState
//    {
//        CheckVersion,
//        Sync
//    }

//    private Canvas m_Canvas;

//    private SyncState m_SyncState = SyncState.CheckVersion;

//    private bool m_IsDirty;

//    // Matchmaker related
//    bool m_MatchCreated;
//    bool m_MatchJoined;
//    MatchInfo m_MatchInfo;
//    NetworkMatch m_NetworkMatch;

//    // Connection/communication related
//    int m_HostId = -1;
//    // On the server there will be multiple connections, on the client this will only contain one ID
//    List<int> m_ConnectionIds = new List<int>();
//    byte[] m_ReceiveBuffer;
//    NetworkWriter m_Writer;
//    NetworkReader m_Reader;
//    bool m_ConnectionEstablished;

//    const int k_ServerPort = 25000;
//    const int k_MaxMessageSize = 65535;

//    void Awake()
//    {
//        m_NetworkMatch = gameObject.AddComponent<NetworkMatch>();
//    }

//    void Start()
//    {
//        m_InputField.text = PlayerPrefs.GetString("BabyName", string.Empty);
//        m_Canvas = GetComponent<Canvas>();
//        m_CreateButton.SetActive(true);
//        m_JoinButton.SetActive(true);
//        m_ShutdownButton.SetActive(false);
//        m_SyncButton.SetActive(m_SyncState == SyncState.Sync);

//        m_ReceiveBuffer = new byte[k_MaxMessageSize];
//        m_Writer = new NetworkWriter();
//    }

//    void OnApplicationQuit()
//    {
//        NetworkTransport.Shutdown();
//    }

//    public void OnCreateButtonClick()
//    {
//        string strBabyName = m_InputField.text;
//        m_NetworkMatch.CreateMatch(strBabyName, 4, true, "", "", "", 0, 0, OnMatchCreate);
//        PlayerPrefs.SetString("BabyName", strBabyName);
//        m_StateDisplay.text = "开始连接中转服务器";
//    }

//    public void OnJoinButtonClick()
//    {
//        string strBabyName = m_InputField.text;
//        m_NetworkMatch.ListMatches(0, 1, m_InputField.text, true, 0, 0, (success, info, matches) =>
//        {
//            if (success && matches.Count > 0)
//            {
//                m_NetworkMatch.JoinMatch(matches[0].networkId, "", "", "", 0, 0, OnMatchJoined);
//            }
//            else
//            {
//                m_StateDisplay.text = "没有找到与宝贝名字相同的连接";
//            }
//        });
//        PlayerPrefs.SetString("BabyName", strBabyName);
//        m_StateDisplay.text = "正在连接中";
//    }

//    public void OnShutdownButtonClick()
//    {
//        m_StateDisplay.text = "已断开与服务器的连接";
//        m_NetworkMatch.DropConnection(m_MatchInfo.networkId, m_MatchInfo.nodeId, 0, OnConnectionDropped);
//        m_CreateButton.SetActive(true);
//        m_JoinButton.SetActive(true);
//        m_ShutdownButton.SetActive(false);
//    }

//    public void OnSyncButtonClick()
//    {
//        if (m_ConnectionEstablished)
//        {
//            m_Writer.SeekZero();
//            m_Writer.Write((int)Protocol.SyncArchive1);
//            CustomStarUsage.m_Instance.Searialize(m_Writer);
//            byte error;
//            for (int i = 0; i < m_ConnectionIds.Count; ++i)
//            {
//                NetworkTransport.Send(m_HostId, m_ConnectionIds[i], 0, m_Writer.AsArray(), m_Writer.Position, out error);
//                if ((NetworkError)error != NetworkError.Ok)
//                {
//                    Debug.LogError("Failed to send message: " + (NetworkError)error);
//                }
//            }
//            m_Writer.SeekZero();
//            m_Writer.Write((int)Protocol.SyncArchive2);
//            Days.m_Instance.Searialize(m_Writer);
//            for (int i = 0; i < m_ConnectionIds.Count; ++i)
//            {
//                NetworkTransport.Send(m_HostId, m_ConnectionIds[i], 0, m_Writer.AsArray(), m_Writer.Position, out error);
//                if ((NetworkError)error != NetworkError.Ok)
//                {
//                    Debug.LogError("Failed to send message: " + (NetworkError)error);
//                }
//            }
//            m_Writer.SeekZero();
//            m_Writer.Write((int)Protocol.SyncArchive3);
//            UseStarHistory.m_Instance.Searialize(m_Writer);
//            for (int i = 0; i < m_ConnectionIds.Count; ++i)
//            {
//                NetworkTransport.Send(m_HostId, m_ConnectionIds[i], 0, m_Writer.AsArray(), m_Writer.Position, out error);
//                if ((NetworkError)error != NetworkError.Ok)
//                {
//                    Debug.LogError("Failed to send message: " + (NetworkError)error);
//                }
//            }
//        }
//    }

//    public void OnCloseButtonClick()
//    {
//        if (m_IsDirty)
//        {
//            m_MainWindow.OnSync();
//            m_IsDirty = false;
//        }
//        m_Canvas.enabled = false;
//    }

//    public void OnConnectionDropped(bool success, string extendedInfo)
//    {
//        m_StateDisplay.text = "服务器已断开连接";
//        NetworkTransport.Shutdown();
//        m_HostId = -1;
//        m_MatchInfo = null;
//        m_MatchCreated = false;
//        m_MatchJoined = false;
//        m_ConnectionEstablished = false;
//    }

//    public virtual void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
//    {
//        if (success)
//        {
//            m_StateDisplay.text = "已开始匹配";
//            Utility.SetAccessTokenForNetwork(matchInfo.networkId, matchInfo.accessToken);

//            m_MatchCreated = true;
//            m_MatchInfo = matchInfo;

//            StartServer(matchInfo.address, matchInfo.port, matchInfo.networkId, matchInfo.nodeId);

//            OnConnection();
//        }
//        else
//        {
//            m_StateDisplay.text = "匹配失败：" + extendedInfo;
//        }
//    }

//    public virtual void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
//    {
//        if (success)
//        {
//            Utility.SetAccessTokenForNetwork(matchInfo.networkId, matchInfo.accessToken);

//            m_MatchJoined = true;
//            m_MatchInfo = matchInfo;

//            m_StateDisplay.text = "已与目标建立连接：" + matchInfo.address + " Port:" + matchInfo.port + " NetworKID: " + matchInfo.networkId + " NodeID: " + matchInfo.nodeId;
//            ConnectThroughRelay(matchInfo.address, matchInfo.port, matchInfo.networkId, matchInfo.nodeId);

//            OnConnection();
//        }
//        else
//        {
//            m_StateDisplay.text = "服务器连接失败：" + extendedInfo;
//        }
//    }

//    void SetupHost(bool isServer)
//    {
//        m_StateDisplay.text = "正在初始化网络连接";
//        NetworkTransport.Init();
//        var config = new ConnectionConfig();
//        config.AddChannel(QosType.Reliable);
//        config.AddChannel(QosType.Unreliable);
//        var topology = new HostTopology(config, 4);
//        if (isServer)
//            m_HostId = NetworkTransport.AddHost(topology, k_ServerPort);
//        else
//            m_HostId = NetworkTransport.AddHost(topology);
//    }

//    void StartServer(string relayIp, int relayPort, NetworkID networkId, NodeID nodeId)
//    {
//        SetupHost(true);

//        byte error;
//        NetworkTransport.ConnectAsNetworkHost(m_HostId, relayIp, relayPort, networkId, Utility.GetSourceID(), nodeId, out error);
//    }

//    void ConnectThroughRelay(string relayIp, int relayPort, NetworkID networkId, NodeID nodeId)
//    {
//        SetupHost(false);

//        byte error;
//        NetworkTransport.ConnectToNetworkPeer(m_HostId, relayIp, relayPort, 0, 0, networkId, Utility.GetSourceID(), nodeId, out error);
//    }

//    void OnConnection()
//    {
//        if (m_MatchCreated || m_MatchJoined)
//        {
//            m_CreateButton.SetActive(false);
//            m_JoinButton.SetActive(false);
//            m_ShutdownButton.SetActive(true);

//            m_SyncState = SyncState.CheckVersion;
//        }
//    }

//    void Update()
//    {
//        if (m_HostId == -1)
//            return;

//        var networkEvent = NetworkEventType.Nothing;
//        int connectionId;
//        int channelId;
//        int receivedSize;
//        byte error;

//        networkEvent = NetworkTransport.ReceiveRelayEventFromHost(m_HostId, out error);
//        if (networkEvent == NetworkEventType.ConnectEvent)
//        {
//            m_StateDisplay.text = "已连接中转服务器";
//        }
//        if (networkEvent == NetworkEventType.DisconnectEvent)
//        {
//            m_StateDisplay.text = "与中转服务器断开连接";
//            m_SyncButton.SetActive(false);
//        }

//        do
//        {
//            networkEvent = NetworkTransport.ReceiveFromHost(m_HostId, out connectionId, out channelId, m_ReceiveBuffer, m_ReceiveBuffer.Length, out receivedSize, out error);
//            if ((NetworkError)error != NetworkError.Ok)
//            {
//                m_StateDisplay.text = "接收网络数据时发生错误：" + (NetworkError)error;
//            }

//            switch (networkEvent)
//            {
//                case NetworkEventType.ConnectEvent:
//                    {
//                        m_StateDisplay.text = "已通过中转服务器建立连接：" + connectionId + " ChannelID:" + channelId;
//                        if (m_ConnectionEstablished = true)
//                        {
//                            m_ConnectionIds.Add(connectionId);
//                            m_Writer.SeekZero();
//                            m_Writer.Write((int)Protocol.VersionCheckNtf);
//                            m_Writer.Write(VERSION_ID);
//                            for (int i = 0; i < m_ConnectionIds.Count; ++i)
//                            {
//                                NetworkTransport.Send(m_HostId, m_ConnectionIds[i], 0, m_Writer.AsArray(), m_Writer.Position, out error);
//                                if ((NetworkError)error != NetworkError.Ok)
//                                {
//                                    Debug.LogError("Failed to send message: " + (NetworkError)error);
//                                }
//                            }
//                        }
//                        break;
//                    }
//                case NetworkEventType.DataEvent:
//                    {
//                        m_StateDisplay.text = "收到一个协议包:" + connectionId + " ChannelID: " + channelId + " Received Size: " + receivedSize;
//                        m_Reader = new NetworkReader(m_ReceiveBuffer);
//                        ProcessProtocol();
//                        break;
//                    }
//                case NetworkEventType.DisconnectEvent:
//                    {
//                        m_StateDisplay.text = "与中转服务器断开连接：" + connectionId;
//                        m_CreateButton.SetActive(true);
//                        m_JoinButton.SetActive(true);
//                        m_ShutdownButton.SetActive(false);
//                        m_SyncButton.SetActive(false);
//                        break;
//                    }
//                case NetworkEventType.Nothing:
//                    break;
//            }
//        } while (networkEvent != NetworkEventType.Nothing);
//    }

//    void ProcessProtocol()
//    {
//        int nProtocol = m_Reader.ReadInt32();
//        switch(nProtocol)
//        {
//            case (int)Protocol.VersionCheckNtf:
//                int nVersionId = m_Reader.ReadInt32();
//                m_SyncState = nVersionId == VERSION_ID ? SyncState.Sync : SyncState.CheckVersion;
//                bool canSync = m_SyncState == SyncState.Sync;
//                m_SyncButton.SetActive(canSync);
//                if (canSync)
//                {
//                    m_StateDisplay.text = "版本验证通过，可以同步存档";
//                }
//                else
//                {
//                    m_StateDisplay.text = "和对方的版本不同，无法同步存档";
//                }
//                break;
//            case (int)Protocol.SyncArchive1:
//                CustomStarUsage.m_Instance.Desearialize(m_Reader);
//                m_IsDirty = true;
//                break;
//            case (int)Protocol.SyncArchive2:
//                Days.m_Instance.Desearialize(m_Reader);
//                m_IsDirty = true;
//                break;
//            case (int)Protocol.SyncArchive3:
//                UseStarHistory.m_Instance.Desearialize(m_Reader);
//                m_IsDirty = true;
//                break;
//            default:
//                m_StateDisplay.text = "不能识别的网络消息：" + nProtocol;
//                break;
//        }
//    }
}
