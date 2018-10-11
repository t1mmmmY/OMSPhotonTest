﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PhotonAvatarView : Photon.MonoBehaviour, IPunObservable
{
    private OvrAvatar ovrAvatar;
    private OvrAvatarRemoteDriver remoteDriver;

    private List<byte[]> packetData;

    private int localSequence;

    public void Start()
    {
        if (photonView.isMine)
        {
            ovrAvatar = GetComponent<OvrAvatar>();
            ovrAvatar.RecordPackets = true;
            ovrAvatar.PacketRecorded += OnLocalAvatarPacketRecorded;

            packetData = new List<byte[]>();
        }
        else
        {
            remoteDriver = GetComponent<OvrAvatarRemoteDriver>();
        }
    }

    public void OnDisable()
    {
        if (photonView.isMine)
        {
            ovrAvatar.RecordPackets = false;
            ovrAvatar.PacketRecorded -= OnLocalAvatarPacketRecorded;
        }
    }

    public void OnLocalAvatarPacketRecorded(object sender, OvrAvatar.PacketEventArgs args)
    {
        if (!PhotonNetwork.inRoom || (PhotonNetwork.room.PlayerCount < 2))
        {
            return;
        }

        using (MemoryStream outputStream = new MemoryStream())
        {
            BinaryWriter writer = new BinaryWriter(outputStream);

            var size = Oculus.Avatar.CAPI.ovrAvatarPacket_GetSize(args.Packet.ovrNativePacket);
            byte[] data = new byte[size];
            Oculus.Avatar.CAPI.ovrAvatarPacket_Write(args.Packet.ovrNativePacket, size, data);

            writer.Write(localSequence++);
            writer.Write(size);
            writer.Write(data);

            packetData.Add(outputStream.ToArray());
        }
    }

    private void DeserializeAndQueuePacketData(byte[] data)
    {
        using (MemoryStream inputStream = new MemoryStream(data))
        {
            BinaryReader reader = new BinaryReader(inputStream);
            int remoteSequence = reader.ReadInt32();

            int size = reader.ReadInt32();
            byte[] sdkData = reader.ReadBytes(size);

            System.IntPtr packet = Oculus.Avatar.CAPI.ovrAvatarPacket_Read((System.UInt32)data.Length, sdkData);
            if (remoteDriver == null)
            {
                remoteDriver = GetComponent<OvrAvatarRemoteDriver>();
            }
            remoteDriver.QueuePacket(remoteSequence, new OvrAvatarPacket { ovrNativePacket = packet });
        }
    }

    #region IPunObservable implementation
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            if (packetData == null)
            {
                return;
            }
            if (packetData.Count == 0)
            {
                return;
            }

            stream.SendNext(packetData.Count);

            foreach (byte[] b in packetData)
            {
                stream.SendNext(b);
            }

            packetData.Clear();
        }

        if (stream.isReading)
        {
            int num = (int)stream.ReceiveNext();

            for (int counter = 0; counter < num; ++counter)
            {
                byte[] data = (byte[])stream.ReceiveNext();

                DeserializeAndQueuePacketData(data);
            }
        }
    }
    #endregion
}
