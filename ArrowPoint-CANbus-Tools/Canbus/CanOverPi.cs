﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using UVC.SocketCAN;

namespace ArrowPointCANBusTool.Canbus
{
    class CanOverPi : ICanTrafficInterface
    {

        /*
         * Can Packet structure is:
         * 
         * +-+------------------+-+----------------------+--------------+---------+----------+---------+
         * |8|56 - Bus Identifer|8|56 - Client Identifier|32 - Identifer|8 - Flags|8 - Length|64 - Data|
         * +-+------------------+-+----------------------+--------------+---------+----------+---------+
         * 
         */

        private const String DEFAULT_IPADDRESS = "127.0.0.1";
        // default daemon port for socketcand
        private const int DEFAULT_PORT = 29536;

        private Thread UdpReceiverThread;
        private UdpClient udpReceiverConnection;
        //private List<UdpClient> udpSenderConnections;
        private CanSocket sender;
        private Boolean isConnected = false;
        private IPAddress ipAddressMulticast;
        private IPEndPoint ipEndPointMulticast;
        private IPEndPoint localEndPoint;

        public string Ip { get; set; } = DEFAULT_IPADDRESS;
        public int Port { get; set; } = DEFAULT_PORT;
        public ReceivedCanPacketHandler ReceivedCanPacketCallBack { get; set; }
        public List<string> SelectedInterfaces { get; set; }

        internal void Close()
        {
            Disconnect();
        }

        // only interface is the socketcand daemon
        public Dictionary<string, string> AvailableInterfaces {
            get {
                Dictionary<string, string> availableInterfaces = null;
                availableInterfaces.Add(DEFAULT_IPADDRESS, DEFAULT_IPADDRESS + ":" + DEFAULT_PORT);
                // Find all available network interfaces
                /*
                NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

                foreach (NetworkInterface networkInterface in networkInterfaces)
                {
                    if ((!networkInterface.Supports(NetworkInterfaceComponent.IPv4)) ||
                        (networkInterface.OperationalStatus != OperationalStatus.Up))
                    {
                        continue;
                    }

                    IPInterfaceProperties adapterProperties = networkInterface.GetIPProperties();
                    UnicastIPAddressInformationCollection unicastIPAddresses = adapterProperties.UnicastAddresses;
                    IPAddress ipAddress = null;

                    foreach (UnicastIPAddressInformation unicastIPAddress in unicastIPAddresses)
                    {
                        if (unicastIPAddress.Address.AddressFamily != AddressFamily.InterNetwork)
                        {
                            continue;
                        }

                        ipAddress = unicastIPAddress.Address;
                        break;
                    }

                    if (ipAddress == null)
                    {
                        continue;
                    }

                    if (availableInterfaces == null)
                        availableInterfaces = new Dictionary<string, string>();

                    availableInterfaces.Add(ipAddress.ToString(), ipAddress.ToString() + " - " + networkInterface.Name);

                }*/

                return availableInterfaces;
            }
        }

        // currently connects to all available 
        public Boolean Connect()
        {

            // Both the sender the receiver
            ipAddressMulticast = IPAddress.Parse(this.Ip);
            ipEndPointMulticast = new IPEndPoint(this.ipAddressMulticast, this.Port);
            localEndPoint = new IPEndPoint(IPAddress.Any, this.Port);

            try
            {
                // create socketcand endpoint
                EndPoint socketcand = new IPEndPoint(ipAddressMulticast, DEFAULT_PORT);
                this.sender = new CanSocket();
                //begin connection to socketcand endpoint
                this.sender.Connect(socketcand);
                /*
                this.udpReceiverConnection = new UdpClient()
                {
                    ExclusiveAddressUse = false
                };
                udpReceiverConnection.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                udpReceiverConnection.Client.Bind(localEndPoint);

                // join multicast group on all available network interfaces
                NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

                // for each interface that is available, 
                foreach (NetworkInterface networkInterface in networkInterfaces)
                {
                    // if it doesnt support IPv4 networking or is not currently operational continue
                    if ((!networkInterface.Supports(NetworkInterfaceComponent.IPv4)) ||
                        (networkInterface.OperationalStatus != OperationalStatus.Up))
                    {
                        continue;
                    }

                    IPInterfaceProperties adapterProperties = networkInterface.GetIPProperties();
                    UnicastIPAddressInformationCollection unicastIPAddresses = adapterProperties.UnicastAddresses;
                    IPAddress ipAddress = null;

                    foreach (UnicastIPAddressInformation unicastIPAddress in unicastIPAddresses)
                    {
                        if (unicastIPAddress.Address.AddressFamily != AddressFamily.InterNetwork)
                        {
                            continue;
                        }

                        ipAddress = unicastIPAddress.Address;
                        break;
                    }

                    if (ipAddress == null)
                    {
                        continue;
                    }

                    if (SelectedInterfaces != null && !SelectedInterfaces.Contains(ipAddress.ToString()))
                    {
                        continue;
                    }

                    udpReceiverConnection.JoinMulticastGroup(ipAddressMulticast, ipAddress);

                    // Also create a client for this interface and add it to the list of interfaces
                    IPEndPoint interfaceEndPoint = new IPEndPoint(ipAddress, this.Port);

                    UdpClient sendClient = new UdpClient();
                    sendClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    sendClient.Client.MulticastLoopback = true;
                    sendClient.Client.Bind(interfaceEndPoint);
                    sendClient.JoinMulticastGroup(ipAddressMulticast);

                    if (udpSenderConnections == null) udpSenderConnections = new List<UdpClient>();
                    udpSenderConnections.Add(sendClient);
                }*/

            }
            catch
            {
                return false;
            }

            // hope that it has connected
            this.isConnected = true;

            //StartReceiver();

            return isConnected;
        }

        // disconnect from all currently connected interfaces
        public Boolean Disconnect()
        {
            if (!isConnected) return false;

            // close all currently open sockets
            try
            {
                sender.Disconnect(false);
                //udpReceiverConnection.Close();
                //foreach (UdpClient client in udpSenderConnections)
                //    client.Close();
            }
            catch { }

            //StopReceiver();

            isConnected = false;

            return isConnected;
        }

        // send the canPacket over Pi socketcand interface
        public int SendMessage(CanPacket canPacket)
        {
            if (!isConnected) return -1;

            //var data = canPacket.RawBytes;

            var data = new ArraySegment<byte>(canPacket.RawBytes, 0, 8);
            var send = new List<ArraySegment<byte>>() { data };
            int resultToReturn = 0;

            // send CAN to each client -> change to interface via socketCAN 
            /*foreach (UdpClient client in udpSenderConnections)
            {
                int result = client.Send(data, data.Length, ipEndPointMulticast);
                if (result > resultToReturn)
                    resultToReturn = result;
            }*/
            // send "send" message
            resultToReturn = sender.Send(send);

            return resultToReturn;
        }

        public Boolean IsConnected()
        {
            return isConnected;
        }

        /*
        private Boolean StartReceiver()
        {
            try
            {
                UdpReceiverThread = new Thread(UdpReceiverLoop);
                UdpReceiverThread.Start();
            }
            catch
            {
                return false;
            }

            return true;
        }

        private void StopReceiver()
        {
            try
            {
                UdpReceiverThread.Abort();
            }
            catch { };
        }
        */

        private void UdpReceiverLoop()
        {
            while (this.isConnected)
            {
                try
                {
                    var ipEndPoint = new IPEndPoint(IPAddress.Any, this.Port);
                    byte[] data = udpReceiverConnection.Receive(ref ipEndPoint);
                    IPAddress sourceAddress = ipEndPoint.Address;
                    int port = ipEndPoint.Port;

                    if (CheckIfTritiumDatagram(data))
                    {
                        SplitCanPackets(data, sourceAddress, port);
                    }
                }
                catch
                {
                    Disconnect();
                }
            }
        }

        private bool CheckIfTritiumDatagram(byte[] data)
        {
            string dataString = MyExtensions.ByteArrayToText(data);

            // Some tritium Can Bridges uses Tritiub rather that Tritium
            // The latest release seems to just use Tri
            return dataString.Contains("Tri");
        }

        private void SplitCanPackets(byte[] data, IPAddress sourceIPAddress, int sourcePort)
        {
            Byte[] header = data.Take(16).ToArray();
            Byte[] body = data.Skip(16).ToArray();
            int numPackets = body.Length / 14;

            for (int i = 0; i < numPackets; i++)
            {
                CanPacket canPacket = new CanPacket(header.Concat(body.Take(14).ToArray()).ToArray())
                {
                    SourceIPAddress = sourceIPAddress,
                    SourceIPPort = sourcePort
                };

                ReceivedCanPacketCallBack?.Invoke(canPacket);

                body = body.Skip(14).ToArray();
            }

        }
    }
}
