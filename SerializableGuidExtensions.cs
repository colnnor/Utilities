using System;

public static class SerializableGuidExtensions
{
    public static SerializableGuid ToSerializableGuid(this Guid guid) => new SerializableGuid(guid);

}