using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIController : MonoBehaviour
{
    [SerializeField]
    private Button buttonStartServer;
    [SerializeField]
    private Button buttonShutDownServer;
    [SerializeField]
    private Button buttonConnectClientPanel;
    [SerializeField]
    private Button buttonDisconnectClient;
    [SerializeField]
    private Button buttonSendMessage;

    [SerializeField]
    private TMP_InputField inputField;
    [SerializeField]
    private TextField textField;
    [SerializeField]
    private Server server;
    [SerializeField]
    private Client client;
    
    [Header("ConnectPanel")]
    [SerializeField] private GameObject connectPanel;
    [SerializeField] private Button buttonConnectClient;
    [SerializeField] private Button buttonClosePanel;
    [SerializeField] private TMP_InputField textFieldLogin;


    private void Start()
    {
        buttonStartServer.onClick.AddListener(() => StartServer());
        buttonShutDownServer.onClick.AddListener(() => ShutDownServer());
        buttonConnectClientPanel.onClick.AddListener(() => OpenConnectPanel());
        buttonConnectClient.onClick.AddListener(() => ConnectClient());
        buttonClosePanel.onClick.AddListener(() => CloseConnectPanel());
        buttonDisconnectClient.onClick.AddListener(() => Disconnect());
        buttonSendMessage.onClick.AddListener(() => SendMessage());
        client.onMessageReceive += ReceiveMessage;
    }

    private void StartServer()
    {
        server.StartServer();
    }

    private void ShutDownServer()
    {
        server.ShutDownServer();
    }

    private void OpenConnectPanel()
    {
        connectPanel.SetActive(true);
    }

    private void ConnectClient()
    {
        if (textFieldLogin.text == "")
            return;

        client.Connect(textFieldLogin.text);
        connectPanel.SetActive(false);
    }

    private void CloseConnectPanel()
    {
        connectPanel.SetActive(false);
    }

    private void Disconnect()
    {
        client.Disconnect();
    }
    private void SendMessage()
    {
        client.SendMessage(inputField.text);
        inputField.text = "";
    }
    public void ReceiveMessage(object message)
    {
        textField.ReceiveMessage(message);
    }
}
