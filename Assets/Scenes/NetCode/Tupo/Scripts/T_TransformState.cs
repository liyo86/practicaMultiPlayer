using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public struct T_TransformState : INetworkSerializable
{
    public int Tick;
    public Vector3 Position;
    public Quaternion Rotation;
    public bool HasStartedMoving;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        if (serializer.IsWriter)
        {
            var writer = serializer.GetFastBufferWriter();
            writer.WriteValueSafe(Tick);
            writer.WriteValueSafe(Position);
            writer.WriteValueSafe(Rotation);
            writer.WriteValueSafe(HasStartedMoving);
        }
        else
        {
            var reader = serializer.GetFastBufferReader();
            reader.ReadValueSafe(out Tick);
            reader.ReadValueSafe(out Position);
            reader.ReadValueSafe(out Rotation);
            reader.ReadValueSafe(out HasStartedMoving);
        }
    }
}
