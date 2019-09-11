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
                String resp = SendMessageGetResponseInner("< open can0 >");
                Debug.Print(resp);
                if (resp.Equals(ERROR_STR)) {
                    isConnected = false;
                }
                else if (resp.Equals("< hi >< ok >"))
                {
                    SocketCanInitialised = true;
                    isConnected = true;
                }   
            }
            StartReceiver(); // Ghetto af
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

                // Create a socket connection, completely seperate to the other one

                /*
                 * 
                 *   client = new TcpClient {
                        ReceiveTimeout = 500
                    };
                    client.ConnectAsync(Ip, Port).Wait(500);

                  Use this connection to < hi > and switch to raw mode

                Down this connection, now comes everthing 
                */

                NetworkStream stream = null;

                Byte[] data = System.Text.Encoding.ASCII.GetBytes("< open can0 >< rawmode >" + "\r\n");
                if (receiver == null || receiver.Connected == false) {
                    receiver = new TcpClient {
                        ReceiveTimeout = 500
                    };
                    receiver.Connect(Ip, Port);
                }

                // if (client == null || client.Connected == false) return (ERROR_STR);
                stream = receiver.GetStream();
                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);
                // Receive the TcpServer.response.
                // Buffer to store the response bytes.
                data = new Byte[256];
                // String to store the response ASCII representation.
                String responseData = String.Empty;

                int delayed = 0;
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

                if (responseData != String.Empty) {
                    responseData = responseData.Replace("\r\n", string.Empty);
                    responseData = responseData.Replace("\r", string.Empty);
                }

                if (responseData == "< hi >< ok >< ok >") { // MIGHT NEED TO REMOVE < hi > 
                    Debug.WriteLine("Connected");
                }

                //if (!SendMessageGetResponseInner("< open can0 >").Equals("< hi >< ok >"))
                //    return (ERROR_STR);
                //    SocketCanInitialised = true;
                //}

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

        private void CanReceiverLoop() {
            while (this.isConnected) {
                try {

                    Byte[] data = new Byte[256];
                    // String to store the response ASCII representation.
                    String rawResponseData = String.Empty;
                    int delayed = 0;
                    // Read the first batch of the TcpServer response bytes.
                    while (delayed < 1000) {
                        char finalChar = ' ';

                        if (rawResponseData != null && rawResponseData != String.Empty)
                            finalChar = rawResponseData[rawResponseData.Length - 1];

                        if (finalChar != '\r') {
                            Int32 bytes = 0;
                            try {
                                bytes = receiver.GetStream().Read(data, 0, data.Length);
                                rawResponseData += System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                            } catch {
                                // Read error, lets leave the loop
                                break;
                            }
                        } else break;
                        Thread.Sleep(10);
                        delayed += 10;
                    }

                    // remove all newlines, returns, and > characters
                    if (rawResponseData != String.Empty) {
                        rawResponseData = rawResponseData.Replace("\r\n", string.Empty);
                        rawResponseData = rawResponseData.Replace("\r", string.Empty);
                        rawResponseData = rawResponseData.Replace(">", string.Empty);
                    }


                    /* read whatever is coming down that connection
                     * 
                     * Make a canpacket from it
                     * 
                     * 

                    */

                    /* if (CheckIfTritiumDatagram(data)) {
                         SplitCanPackets(data, sourceAddress, port);
                     } */

                    String[] packets = rawResponseData.Split('<');

                    // convert each string to a canpacket and invoke callback
                    foreach (String s in packets) {
                        
                        //try {
                            CanPacket p = rawInputToCan(s);
                            Debug.Print("she good?");
                            ReceivedCanPacketCallBack?.Invoke(p);
                            Debug.Print("She Good cuz");
                        /*}
                        catch {
                            Debug.Print(s);
                            Debug.Print("Failed callback for some reason");
                        } */
                    }
                    // Parse the content that you get Here

                    //CanPacket canPacket = new CanPacket((uint)302);

                    // Parse the string that comes back

                    /*

                    canPacket.SetByte(0, 0);
                    canPacket.SetByte(1, 0);

                    ReceivedCanPacketCallBack?.Invoke(canPacket);*/

                } catch {
                    Debug.Print("Cole stinks");
                    Disconnect();
                }
            }
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
            if (SendMessageGetResponse(canPacketToSocketCan(canPacket)).Equals(ERROR_STR)) return 0;
            return 1;
        }

        // convert canpacket to socketcand command given socket and mode
        public String canPacketToSocketCan(CanPacket input, String mode = "send")
        {
            StringBuilder str = new StringBuilder();
            str.Append("< " + mode + " ");

            str.Append(input.CanIdBase10.ToString() + " ");
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
            Debug.Print(vals.ToString());
            String rawCanId = vals[1] ;
            String rawBytesString = vals[3]; // populate with the frame's bytestring
            CanPacket output = new CanPacket(rawBytesString);
            output.CanId = Convert.ToUInt32(rawCanId, 16);

            

            return output;
        }
    }
}