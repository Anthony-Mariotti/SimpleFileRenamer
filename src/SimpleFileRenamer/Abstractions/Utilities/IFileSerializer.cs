namespace SimpleFileRenamer.Abstractions.Utilities;

/// <summary>
/// Defines methods for serializing and deserializing configuration data.
/// </summary>
public interface IFileSerializer
{
    /// <summary>
    /// Serializes the given object to a string representation.
    /// </summary>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    /// <param name="obj">The object to serialize.</param>
    /// <returns>A string representation of the object.</returns>
    string Serialize<T>(T obj);

    /// <summary>
    /// Deserializes the given string to an object of type T.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
    /// <param name="data">The string representation of the object.</param>
    /// <returns>An object of type T.</returns>
    T? Deserialize<T>(string data);
}
