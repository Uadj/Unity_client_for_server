using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class client : MonoBehaviour
{
    public InputField inputField;
    public Text chatOutput;
    public Scrollbar scrollbar;

    private Socket _socket;
    private List<string> _messages;

    private void Start()
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _socket.Connect("127.0.0.1", 11000);
        _messages = new List<string>();
    }

    private void Update()
    {
        byte[] buffer = new byte[1024];
        int bytesReceived = _socket.Receive(buffer);
        if (bytesReceived > 0)
        {
            string message = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
            _messages.Add(message);
            UpdateChatOutput();
        }
    }

    public void OnSendButtonClicked()
    {
        string message = inputField.text;
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        _socket.Send(buffer);
        inputField.text = "";
    }

    private void UpdateChatOutput()
    {
        chatOutput.text = string.Join("\n", _messages);
        scrollbar.value = 0;
    }
}
