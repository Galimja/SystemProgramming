using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class Server : MonoBehaviour
{
    private const int MAX_CONNECTION = 10;
    private int port = 5805;
    
    private int hostID;
    private int reliableChannel;
    
    private bool isStarted = false;
    private byte error;
    
    List<int> connectionIDs = new List<int>();

    private Dictionary<int, string> users = new Dictionary<int, string>();

    public void StartServer()
    {
        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();
        reliableChannel = cc.AddChannel(QosType.Reliable);
        HostTopology topology = new HostTopology(cc, MAX_CONNECTION);
        hostID = NetworkTransport.AddHost(topology, port);
        isStarted = true;
    }


    void Update()
    {
        if (!isStarted) return;

        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        
        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out
            channelId, recBuffer, bufferSize, out dataSize, out error);
        
        while (recData != NetworkEventType.Nothing)
        {
            switch (recData)
            {
                case NetworkEventType.Nothing:
                    break;
                case NetworkEventType.ConnectEvent:
                    connectionIDs.Add(connectionId);
                    users.Add(connectionId, "");
                    Debug.Log($"Player {connectionId} has connected.");
                    break;
                case NetworkEventType.DataEvent:
                    string message = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                    if (users[connectionId] == "")
                    {
                        users[connectionId] = message;
                        SendMessageToAll($"{users[connectionId]} has connected.");
                        Debug.Log($"{users[connectionId]} has connected.");
                        break;
                    } 
                    SendMessageToAll($"{users[connectionId]}: {message}");
                    Debug.Log($"{users[connectionId]}: {message}");
                    break;
                case NetworkEventType.DisconnectEvent:
                    connectionIDs.Remove(connectionId);
                    SendMessageToAll($"{users[connectionId]} has disconnected.");
                    Debug.Log($"{users[connectionId]} has disconnected.");
                    users.Remove(connectionId);
                    break;
                case NetworkEventType.BroadcastEvent:
                    break;
            }
            recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer,
                bufferSize, out dataSize, out error);
        }
    }

    public void SendMessageToAll(string message)
    {
        for (int i = 0; i < connectionIDs.Count; i++)
        {
            SendMessage(message, connectionIDs[i]);
        }
    }

    public void SendMessage(string message, int connectionID)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(message);
        NetworkTransport.Send(hostID, connectionID, reliableChannel, buffer, message.Length * sizeof(char), out error);
        if ((NetworkError)error != NetworkError.Ok) Debug.Log((NetworkError)error);
    }

    public void ShutDownServer()
    {
        if (!isStarted) return;
        NetworkTransport.RemoveHost(hostID);
        NetworkTransport.Shutdown();
        isStarted = false;
    }
}
