using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Network;

#if !UNITY_EDITOR
using MsgPack;
using MsgPack.Serialization;
#endif


public class CancelBehaviour : MonoBehaviour
{

    public InputField ipAddress;

#if !UNITY_EDITOR
    UdpSocket udp;
    MessagePackSerializer serializer; 
#endif

    void Start()
    {
#if !UNITY_EDITOR
        this.serializer = SerializationContext.Default.GetSerializer<JsonEntity>();
        this.udp = new UdpSocket();
#endif
    }

    public void OnCancelClick()
    {
        JsonEntity msg = new JsonEntity();
        msg.command = JsonEntity.CommandType.Cancel;
        msg.annotate = "";
#if !UNITY_EDITOR
        byte[] msgBytes = this.serializer.PackSingleObject(msg);
        this.udp.Send_Message(msgBytes,new Windows.Networking.HostName(this.ipAddress.text),Settings.PORT);
#endif
    }
}
