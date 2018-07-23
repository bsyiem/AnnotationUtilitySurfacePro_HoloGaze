using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Network;

#if !UNITY_EDITOR
using MsgPack;
using MsgPack.Serialization;
#endif


public class AnnotateBehaviour : MonoBehaviour {

    JsonEntity msg;

    public InputField ipAddress;
    public InputField annotateString;

#if !UNITY_EDITOR
    UdpSocket udp;
    MessagePackSerializer serializer; 
#endif

    void Start()
    {
        this.msg = new JsonEntity();
        this.msg.command = JsonEntity.CommandType.None;
#if !UNITY_EDITOR
        this.serializer = SerializationContext.Default.GetSerializer<JsonEntity>();
        this.udp = new UdpSocket();
        this.udp.Listen(Settings.LISTEN_PORT);
#endif
    }
    void Update()
    {
#if !UNITY_EDITOR
        if(this.udp.Available())
        {
            this.msg = (JsonEntity)this.serializer.UnpackSingleObject(this.udp.Receive());
        }
#endif
        if(this.msg.command == JsonEntity.CommandType.Send)
        {
            OnAnnotateClick();
            this.msg.command = JsonEntity.CommandType.None;
        }
    }
    public void OnAnnotateClick()
    {
        Debug.Log("IPAddress ="+this.ipAddress.text);
        JsonEntity msgToSend = new JsonEntity();
        msgToSend.command = JsonEntity.CommandType.Annotate;
        msgToSend.annotate = annotateString.text;
#if !UNITY_EDITOR
        byte[] msgBytes = this.serializer.PackSingleObject(msgToSend);
        this.udp.Send_Message(msgBytes,new Windows.Networking.HostName(this.ipAddress.text),Settings.PORT);
#endif
        this.annotateString.text = "";
    }
}
