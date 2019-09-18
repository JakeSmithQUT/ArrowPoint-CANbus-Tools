using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ArrowPointCANBusTool.Canbus;

namespace ArrowPointCANBusTool.Canbus
{
    public class CanOverPi : ICanTrafficInterface
    {

        private const string ERROR_STR = "ERROR";

        private static readonly Object comms_locker = new Object();

        private TcpClient client = null;
        private TcpClient receiver = null;

        private Thread CanReceiverThread;
        private bool SocketCanInitialised = false;

        private const String DEFAULT_IPADDRESS = "10.16.16.78";
        private const int DEFAULT_PORT = 29536;

        public string Ip { get; set; } = DEFAULT_IPADDRESS;
        public int Port { get; set; } = DEFAULT_PORT;


        public ReceivedCanPacketHandler ReceivedCanPacketCallBack { get; set; }

        // Need to fix
        public Dictionary<string, string> AvailableInterfaces
        {
            get
            {
                Dictionary<string, string> interfaces = new Dictionary<string, string>
                {
                    { "Can0", "Can0" }
                };
                return interfaces;
            }
        }

        // Need to fix
        public List<string> SelectedInterfaces {
            get
            {
                List<string> selectedInterfaces = new List<string>()
                {
                    { "Can0" }
                };
                return selectedInterfaces;
            }
            set
            {

            }
        }

        private bool isConnected = false;

        public bool Connect()
        {
            isConnected = false;
            if (!SocketCanInitialised)
            {
                client = StartClient();
            }
            StartReceiver();
            return isConnected;
        }

        public bool Disconnect()
        {
            if (isConnected)
            {
                client.Close();
                isConnected = false;
            }
            StopReceiver();
            return isConnected;
        }

        public bool IsConnected()
        {
            return isConnected;
        }



        private Boolean StartReceiver() {
            try {
                NetworkStream stream = null;

                // initialise connection and get all raw packets - add filter here if required
                Byte[] data = System.Text.Encoding.ASCII.GetBytes("< open can0 >< rawmode >" + "\r\n");
                if (receiver == null || receiver.Connected == false) {
                    receiver = new TcpClient {
                        ReceiveTimeout = 500
                    };
                    receiver.Connect(Ip, Port);
                }

                stream = receiver.GetStream();
                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);
                // Receive the TcpServer.response.
                // Buffer to store the response bytes.
                data = new Byte[256];
                // String to store the response ASCII representation.
                String responseData = String.Empty;

                responseData = readResponse(stream);

                if (responseData != String.Empty) {
                    responseData = responseData.Replace("\r\n", string.Empty);
                    responseData = responseData.Replace("\r", string.Empty);
                }

                if (responseData == "< ok >< ok >") { // MIGHT NEED TO REMOVE < hi > 
                    Debug.WriteLine("Connected");
                }

                CanReceiverThread = new Thread(CanReceiverLoop);
                CanReceiverThread.Start();
            } catch {
                return false;
            }

            return true;
        }

        private void StopReceiver() {
            try {

                // Terminte the connection you made above

                CanReceiverThread.Abort();
            } catch { };
        }

        // used to read the response of a given network stream (assumes incoming response)
        private String readResponse(NetworkStream stream)
        {
            // Receive the TcpServer.response.
            // Buffer to store the response bytes.
            var data = new Byte[256];
            int delayed = 0;
            // String to store the response ASCII representation.
            String responseData = String.Empty;
            // Read the first batch of the TcpServer response bytes.
            while (delayed < 1000)
            {
                char finalChar = ' ';

                if (responseData != null && responseData != String.Empty)
                    finalChar = responseData[responseData.Length - 1];

                if (finalChar != '\r')
                {
                    Int32 bytes = 0;
                    try
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        responseData += System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    }
                    catch
                    {
                        // Read error, lets leave the loop
                        break;
                    }
                }
                else break;
                Thread.Sleep(10);
                delayed += 10;
            }
            return responseData;
        }

        private void CanReceiverLoop() {
            while (this.isConnected) {
                try {
                    Byte[] data = new Byte[256];
                    // String to store the response ASCII representation.
                    String rawResponseData = String.Empty;
                    NetworkStream stream = receiver.GetStream();
                    
                    rawResponseData = readResponse(stream);

                    // remove all newlines, returns, and > characters
                    if (rawResponseData != String.Empty) {
                        rawResponseData = rawResponseData.Replace("\r\n", string.Empty);
                        rawResponseData = rawResponseData.Replace("\r", string.Empty);
                        rawResponseData = rawResponseData.Replace(">", string.Empty);
                    }

                    String[] packets = rawResponseData.Trim().Split('<');

                    // convert each string to a canpacket and invoke callback
                    if (packets.Length > 1)
                    {
                        foreach (String s in packets)
                        {
                            try
                            {
                                if (!s.Equals(""))
                                {
                                    CanPacket p = rawInputToCan(s);
                                    if (p != null && p.CanId != 0)
                                    {
                                        ReceivedCanPacketCallBack?.Invoke(p);
                                    }
                                }
                            }
                            catch
                            {
                                Debug.Print("Failed callback for some reason");
                            }

                        }
                    }

                } catch {
                    Disconnect();
                }
            }
        }

        // initialise client connection for sending canpackets. goes through init sequence for client socket connection and returns TCPclient
        private TcpClient StartClient()
        {
            TcpClient clientConnection = null;
            NetworkStream sendStream = null;
            Byte[] connectionMessage = System.Text.Encoding.ASCII.GetBytes("< open can0 >");

            var ReceiveTimeout = 500;
            clientConnection = new TcpClient
            {
                ReceiveTimeout = ReceiveTimeout
            };
            clientConnection.Connect(Ip, Port);
            if (clientConnection.Connected)
            {
                sendStream = clientConnection.GetStream();
                sendStream.Write(connectionMessage, 0, connectionMessage.Length);

                String resp = String.Empty;
                connectionMessage = new Byte[256];

                int delayed = 0;

                Debug.WriteLine("ID: 2.5");

                resp = readResponse(sendStream);

                if (resp.Equals(ERROR_STR))
                {
                    isConnected = false;
                }
                else if (resp.Equals("< hi >< ok >"))
                {
                    SocketCanInitialised = true;
                    isConnected = true;
                }
                return clientConnection;
            }
            return null;
        }

        private string SendMessageGetResponseInner(String message) {

            Debug.WriteLine("ID: 2.1");

            if (Ip == null || Port == 0) return (ERROR_STR);

            Debug.WriteLine("ID: 2.2");

            lock (comms_locker) {

                Debug.WriteLine("ID: 2.3");

                NetworkStream stream = null;

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message + "\r\n");

                // Get a client stream for reading and writing, we try to reuse them so only do this if necessary
                if (client == null || client.Connected == false) {
                    client = new TcpClient {
                        ReceiveTimeout = 500
                    };
                    client.ConnectAsync(Ip, Port).Wait(500);
                }

                Debug.WriteLine("ID: 2.4");

                if (client == null || client.Connected == false) return (ERROR_STR);

                stream = this.client.GetStream();

                Debug.WriteLine("ID: 2.4.1");

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                Debug.WriteLine("ID: 2.4.2");
                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;
                
                Debug.WriteLine("ID: 2.5");

                responseData = readResponse(stream);

                Debug.WriteLine("ID: 2.6 - " + responseData);

                if (responseData != String.Empty) {
                    responseData = responseData.Replace("\r\n", string.Empty);
                    responseData = responseData.Replace("\r", string.Empty);
                }

                Debug.WriteLine("ID: 2.7");

                return responseData;
            }
        }


        public string SendMessageGetResponse(String message) {
            try {

                Debug.WriteLine("ID: 1");

                if (!SocketCanInitialised) {
                    // Actually put your interfaces here

                    if (!SendMessageGetResponseInner("< open can0 >").Equals("< hi >< ok >"))
                        return (ERROR_STR);
                    SocketCanInitialised = true;
                }

                Debug.WriteLine("ID: 2");

                string response = SendMessageGetResponseInner(message);

                Debug.WriteLine("ID: 3");

                // Try again on an error
                if (response == null || response.Equals(ERROR_STR))
                    response = SendMessageGetResponseInner(message);
                return response;
            } catch {
                return ERROR_STR;
            }
        }

        // sends canPacket message over client connection
        public void sendWithoutResponse(CanPacket canPacket)
        {
            if (isConnected)
            {
                String message = canPacketToSocketCan(canPacket);

                lock (comms_locker)
                {
                    NetworkStream stream = client.GetStream();

                    // Translate the passed message into ASCII and store it as a Byte array.
                    Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                    stream.Write(data, 0, data.Length);
                }
            }
        }


        public int SendMessage(CanPacket canPacket)
        {
            // Put the real values in here
            Debug.WriteLine("Sending message");
            if (SendMessageGetResponse(canPacketToSocketCan(canPacket)).Equals(ERROR_STR)) return 0;
            return 1;
        }

        // convert canpacket to socketcand command given socket and mode
        public String canPacketToSocketCan(CanPacket input, String mode = "send")
        {
            StringBuilder str = new StringBuilder();
            str.Append("< " + mode + " ");

            str.Append(input.CanIdAsHex.ToString().Substring(2) + " ");
            str.Append("8 ");
            str.Append(input.Byte0.ToString() + " ");
            str.Append(input.Byte1.ToString() + " ");
            str.Append(input.Byte2.ToString() + " ");
            str.Append(input.Byte3.ToString() + " ");
            str.Append(input.Byte4.ToString() + " ");
            str.Append(input.Byte5.ToString() + " ");
            str.Append(input.Byte6.ToString() + " ");
            str.Append(input.Byte7.ToString() + " ");

            str.Append(">");
            return str.ToString();
        }

        // converts string in the format "frame can_id receive_time raw_bytes" as in socketcand rawmode (with < and > removed)
        public CanPacket rawInputToCan(String input)
        {   
            input = input.Trim();
            String[] vals = input.Split(' ');
            if (vals.Length < 3) {
                return null;
            }

            String rawCanId = vals[1] ;
            String rawBytesString = vals[3]; // populate with the frame's bytestring
            CanPacket output = new CanPacket();

            output.RawBytesString = rawBytesString;
            output.CanId = Convert.ToUInt32(rawCanId, 16);

            

            return output;
        }
    }
}