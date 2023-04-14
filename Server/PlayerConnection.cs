﻿using System.Net;
using System.Net.Sockets;
using PushCar.Common;
using PushCar.Common.Extensions;
using PushCar.Common.Utils;

namespace PushCar.Server;

public class PlayerConnection {
	public TcpClient Client { get; }
	public NetworkStream Stream { get; }
	public BinaryReader Reader { get; }
	public BinaryWriter Writer { get; }
	public IPEndPoint IP => (IPEndPoint)Client.Client.RemoteEndPoint!;

	public PlayerConnection(TcpClient client) {
		Client = client;

		Stream = Client.GetStream();
		Writer = new BinaryWriter(Stream);
		Reader = new BinaryReader(Stream);
	}

	public void SendPacket(IPacket packet) {
		if (!Stream.CanRead) return;
		if (!Stream.CanWrite) return;
		if (!Client.Connected) {
			Debug.Log($"[S -> C({ToString()})] Cannot send packet due to disconnected: {packet}");
			return;
		}
		Debug.Log($"[S -> C({ToString()})] {packet}");
		Writer.Write(packet);
	}

	public string ToString() => $"{IP.Address}:{IP.Port}";
}