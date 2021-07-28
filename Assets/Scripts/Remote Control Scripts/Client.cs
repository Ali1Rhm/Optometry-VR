using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System;

public class Client : MonoBehaviour
{
    [SerializeField] private Toggle[] eToggles;  
    [SerializeField] private LocalhostData data;

    public void Start()
    {
        eToggles[0].isOn = true;
    }

    public void SetToggles(Toggle currentToggle)
    {
        foreach (Toggle eToggle in eToggles)
        {
            if (eToggle != currentToggle)
                eToggle.isOn = false;
        }
    }

    public void SendRequest(string message)
    {
        Connect(data.IP, data.Port, message);
    }

    private static void Connect(String server, Int32 port, String message)
    {
        try
        {
            // Create a TcpClient.
            TcpClient _client = new TcpClient(server, port);

            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] _data = System.Text.Encoding.ASCII.GetBytes(message);

            // Get a client stream for reading and writing.
            NetworkStream _stream = _client.GetStream();

            // Send the message to the connected TcpServer.
            _stream.Write(_data, 0, _data.Length);

            Debug.Log("Sent: " + message);

            // Buffer to store the response bytes.
            _data = new Byte[256];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = _stream.Read(_data, 0, _data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(_data, 0, bytes);
            Console.WriteLine("Received: {0}", responseData);

            // Close everything.
            _stream.Close();
            _client.Close();
        }
        catch (ArgumentNullException e)
        {
            Debug.Log("ArgumentNullException: " + e);
        }
        catch (SocketException e)
        {
            Debug.Log("SocketException: " + e);
        }
    }
}
