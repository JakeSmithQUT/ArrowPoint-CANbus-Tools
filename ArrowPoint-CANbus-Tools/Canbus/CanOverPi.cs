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

        private bool SocketCanInitialised = false;

        private const String DEFAULT_IPADDRESS = "10.16.16.78";
        private const int DEFAULT_PORT = 29536;

        public string Ip { get; set; } = DEFAULT_IPADDRESS;
        public int Port { get; set; } = DEFAULT_PORT;


        public ReceivedCanPacketHandler ReceivedCanPacketCallBack { get; set; }

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

        public List<string> SelectedInterfaces {
            get
            {
                List<string> selectedInterfaces = new List<string>()
                {
                    { "can0" }
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
                String resp = SendMessageGetResponseInner("< open can0 >");
                Debug.Print(resp);
                if (resp.Equals(ERROR_STR)) {
                    isConnected = false;
                }
                else if (resp.Equals("< hi >< ok >"))
                {
                    SocketCanInitialised = isConnected;
                    isConnected = true;
                }   
            }            
            return isConnected;
        }

        public bool Disconnect()
        {
            if (isConnected)
            {
                client.Close();
                isConnected = false;
            }
            return isConnected;
        }

        public bool IsConnected()
        {
            return isConnected;
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

                stream = client.GetStream();

                Debug.WriteLine("ID: 2.4.1");

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                Debug.WriteLine("ID: 2.4.2");
                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                int delayed = 0;

                Debug.WriteLine("ID: 2.5");

                // Read the first batch of the TcpServer response bytes.
                while (delayed < 1000) {
                    char finalChar = ' ';

                    if (responseData != null && responseData != String.Empty)
                        finalChar = responseData[responseData.Length - 1];

                    if (finalChar != '\r') {
                        Int32 bytes = 0;

                        try {
                            bytes = stream.Read(data, 0, data.Length);
                            responseData += System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                        } catch {
                            // Read error, lets leave the loop
                            break;
                        }
                    } else break;
                    Thread.Sleep(10);
                    delayed += 10;
                }

                Debug.WriteLine("ID: 2.6 - " + responseData);

                if (responseData != String.Empty) {
                    responseData = responseData.Replace("\r\n", string.Empty);
                    responseData = responseData.Replace("\r", string.Empty);
                }

                // Close everything.
                //stream?.Close();

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


        public int SendMessage(CanPacket canPacket)
        {
            // Put the real values in here
            if (SendMessageGetResponse("< send 401 8 00 00 00 00 00 00 00 00 >").Equals(ERROR_STR)) return 0;
            return 1;
        }
    }
}
