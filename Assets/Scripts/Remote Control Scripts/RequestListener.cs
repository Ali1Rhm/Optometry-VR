using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Threading.Tasks;
using NaughtyAttributes;

public class RequestListener : MonoBehaviour
{
    OptometryPhaseManager m_optometryPhaseManager;
    EyesManager m_eyeManager;
    TcpListener m_server = null;

    [SerializeField] private bool getIPAutomaric;
    [HideIf("getIPAutomaric")]
    [SerializeField] private string iP;
    [SerializeField] private Int32 port;

    private void Awake()
    {
        // Reference to OptometryPhaseManager script
        m_optometryPhaseManager = GameObject.FindObjectOfType<OptometryPhaseManager>();
        m_eyeManager = GameObject.FindObjectOfType<EyesManager>();

        // Get Local IP if getIPAutomaric is checked
        if(getIPAutomaric)
            iP = GetIP.LocalIPAddress();
    }

    async void Start()
    {
        try
        {
            await ListenForClients();
        }
        catch (SocketException e)
        {
            Debug.Log("SocketException: " + e);
        }
        finally
        {
            // Stop listening for new clients.
            m_server.Stop();
        }
    }

    async Task ListenForClients()
    {
        // Set the TcpListener on port 13000.
        IPAddress _localAddr = IPAddress.Parse(iP);

        // TcpListener server = new TcpListener(port);
        m_server = new TcpListener(_localAddr, port);

        // Start listening for client requests.
        m_server.Start();

        // Buffer for reading data
        Byte[] _bytes = new Byte[256];
        String _data = null;

        // Enter the listening loop.
        while (true)
        {
            Debug.Log("Waiting for a connection...");

            // Perform a blocking call to accept requests.
            TcpClient _client = await m_server.AcceptTcpClientAsync();
            Debug.Log("Connected!");

            _data = null;

            // Get a stream object for reading and writing
            NetworkStream _stream = _client.GetStream();

            int i;

            // Loop to receive all the data sent by the client.
            while ((i = _stream.Read(_bytes, 0, _bytes.Length)) != 0)
            {
                // Translate data bytes to a ASCII string.
                _data = System.Text.Encoding.ASCII.GetString(_bytes, 0, i);
                Debug.Log("Received: " + _data);
                HandleRequest(_data);

                // Process the data sent by the client.
                _data = _data.ToUpper();

                byte[] msg = System.Text.Encoding.ASCII.GetBytes(_data);

                // Send back a response.
                _stream.Write(msg, 0, msg.Length);
            }

            // Shutdown and end connection
            _client.Close();
        }
    }

    // Handle the client request based on the message
    private void HandleRequest(string message)
    {
        switch (message)
        {
            case "ToggleFirst":
                m_optometryPhaseManager.ToggleFirstPhase();
                break;
            case "ToggleSecond":
                m_optometryPhaseManager.ToggleSecondPhase();
                break;
            case "ToggleRight":
                m_eyeManager.ToggleRightEye();
                break;
            case "ToggleLeft":
                m_eyeManager.ToggleLeftEye();
                break;
            default:
                return;
        }
    }
}
