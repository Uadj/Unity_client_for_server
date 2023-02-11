using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;

public class ChatUI : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextMeshProUGUI chatOutput;
    public Scrollbar scrollbar;


    private string message;
    private byte[] buffer;
    private string response;
    private int bytesReceived;
    // Create a TCP/IP socket
    Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    // Connect to the chat server
    private static IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
    private IPEndPoint endPoint = new IPEndPoint(ipAddress, 11000);
    private void UpdateChatOutput(string message)
    {
        chatOutput.text += message + "\n";
        scrollbar.value = 0;
    }
    private void Update()
    {
       
    }
    IEnumerator getMessage()
    {
        
        message = inputField.text;
        while (message != null)
        {
            // Send the message to the chat server
            buffer = Encoding.UTF8.GetBytes(message);
            clientSocket.Send(buffer);

            yield return null;
        }
            

        // Receive the response from the chat server
        buffer = new byte[1024];
        bytesReceived = clientSocket.Receive(buffer);

        // Get the response as a string
        response = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
        UpdateChatOutput(response);
        Debug.Log(response);

        yield return new WaitForSeconds(1);
    }
    private void Start()
    {
        clientSocket.Connect(endPoint);
        UpdateChatOutput("Connected to chat server.");
        StartCoroutine("getMessage", 1);    
    }
}