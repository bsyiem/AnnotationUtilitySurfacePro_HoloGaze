using UnityEngine;
using System;
using System.Collections.Generic;

#if !UNITY_EDITOR
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
#endif

namespace Assets.Scripts.Network
{
    public class UdpSocket
    {
#if !UNITY_EDITOR
        DatagramSocket socket = new DatagramSocket();
        Windows.Networking.HostName senderIP;
        string senderPort;

        Queue<byte[]> messageQueue;

        public bool Available()
        {
            return this.messageQueue.Count > 0 ? true : false;
        }

        public UdpSocket()
        {
            this.socket = new DatagramSocket();
            this.messageQueue = new Queue<byte[]>();
        }

        public async void Listen()
        {
            Debug.Log("Listening");
            this.socket.MessageReceived += Socket_MessageReceived;
            await this.socket.BindServiceNameAsync(Settings.LISTEN_PORT);
        }

        public async void Listen(string port)
        {
            Debug.Log("Listening");
            this.socket.MessageReceived += Socket_MessageReceived;
            await this.socket.BindServiceNameAsync(port);
        }

        private void Socket_MessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
        {
            this.senderIP = args.RemoteAddress;
            this.senderPort = args.RemotePort;

            DataReader dataReader = args.GetDataReader();

            Byte[] buffer = new Byte[dataReader.UnconsumedBufferLength];
            dataReader.ReadBytes(buffer);

            this.messageQueue.Enqueue(buffer);
        }

        public async void SendMessageToPreviousSender(byte[] message)
        {
            if (this.senderIP != null && this.senderPort != null)
            {
                IOutputStream streamOut = await this.socket.GetOutputStreamAsync(this.senderIP, this.senderPort);
                DataWriter dataWriter = new DataWriter(streamOut);
                dataWriter.WriteBytes(message);
                await dataWriter.StoreAsync();
            }

        }

        public async void Send_Message(byte[] message, Windows.Networking.HostName ip, string port)
        {
            Debug.Log("Sending");
            IOutputStream streamOut = await this.socket.GetOutputStreamAsync(ip, port);
            DataWriter dataWriter = new DataWriter(streamOut);
            dataWriter.WriteBytes(message);
            await dataWriter.StoreAsync();
        }

        public byte[] Receive()
        {
            return this.messageQueue.Dequeue();
        }
#endif
    }
}

