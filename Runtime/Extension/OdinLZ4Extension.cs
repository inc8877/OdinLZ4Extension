#region LICENSE

/*
Copyright 2021 Volodymyr Bozhko

Contact: inc8877@gmail.com

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

#endregion

using System.Collections.Generic;
using K4os.Compression.LZ4;
using OdinLZ4Extension.Engine;
using Sirenix.Serialization;

namespace OdinLZ4Extension
{
    public static partial class OdinLZ4API
    {
        #region Serialize
        
        /// <summary>
        /// Serialize type and compress it via LZ4
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <param name="format">The format of serialization</param>
        /// <param name="ctx">The context of a given serialization session</param>
        /// <param name="level">Level of compression</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static byte[] SerializeValue<T>(T value, DataFormat format, SerializationContext ctx = null,
            OdinLZ4Level level = OdinLZ4Level.FAST) =>
            OdinLZ4Engine.Encode(SerializationUtility.SerializeValue<T>(value, format, ctx), (LZ4Level)level);
        
        /// <summary>
        /// Serialize type and compress it via LZ4. Fills a <paramref name="unityObjects"/> list with the Unity objects
        /// which were referenced during serialization.
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <param name="format">The format of serialization</param>
        /// <param name="unityObjects">A list of the Unity objects which were referenced during serialization</param>
        /// <param name="ctx">The context of a given serialization session</param>
        /// <param name="level">Level of compression</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static byte[] SerializeValue<T>(T value, DataFormat format, out List<UnityEngine.Object> unityObjects,
            SerializationContext ctx = null, OdinLZ4Level level = OdinLZ4Level.FAST) =>
            OdinLZ4Engine.Encode(SerializationUtility.SerializeValue<T>(value, format, out unityObjects, ctx), (LZ4Level)level);
        
        /// <summary>
        /// Short version of <see cref="SerializeValue{T}(T, DataFormat, SerializationContext, OdinLZ4Level)"/>
        /// for fast data serialization in binary format
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <param name="level">Level of compression</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static byte[] LazySerialization<T>(T value, OdinLZ4Level level = OdinLZ4Level.FAST) =>
            SerializeValue<T>(value, DataFormat.Binary, level: level);
        
        /// <summary>
        /// Short version of <see cref="SerializeValue{T}(T, DataFormat, out List{UnityEngine.Object}, SerializationContext, OdinLZ4Level)"/>
        /// for fast data serialization in binary format. Fills a <paramref name="unityObjects"/> list with the Unity objects
        /// which were referenced during serialization.
        /// </summary>
        /// <param name="value">The value to serialize</param>
        /// <param name="unityObjects">A list of the Unity objects which were referenced during serialization</param>
        /// <param name="level">Level of compression</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static byte[] LazySerialization<T>(T value, out List<UnityEngine.Object> unityObjects,
            OdinLZ4Level level = OdinLZ4Level.FAST) => SerializeValue<T>(value, DataFormat.Binary, out unityObjects, level: level);
        
        #endregion

        #region Deserialize

        /// <summary>
        /// Decompresses and deserializes a value of a given type from the given byte array in the given format
        /// </summary>
        /// <param name="bytes">The bytes to deserialize from</param>
        /// <param name="format">The format of deserialization</param>
        /// <param name="ctx">The context of a given deserialization session</param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static TResult DeserializeValue<TResult>(byte[] bytes, DataFormat format, DeserializationContext ctx = null) =>
            SerializationUtility.DeserializeValue<TResult>(OdinLZ4Engine.Decode(bytes), format, ctx);

        /// <summary>
        /// Decompresses and deserializes a value of a given type from the given byte array in the given format,
        /// using the given list of Unity objects for external index reference resolution
        /// </summary>
        /// <param name="bytes">The bytes to deserialize from</param>
        /// <param name="format">The format of deserialization</param>
        /// <param name="referencedUnityObjects">The list of Unity objects to use for external index reference resolution</param>
        /// <param name="ctx">The context of a given deserialization session</param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static TResult DeserializeValue<TResult>(byte[] bytes, DataFormat format,
            List<UnityEngine.Object> referencedUnityObjects, DeserializationContext ctx = null) =>
            SerializationUtility.DeserializeValue<TResult>(OdinLZ4Engine.Decode(bytes), format, referencedUnityObjects, ctx);
        
        /// <summary>
        /// Short version of <see cref="DeserializeValue{TResult}(byte[], DataFormat, DeserializationContext)"/>
        /// for fast data deserialization from binary format
        /// </summary>
        /// <param name="bytes">The bytes to deserialize from</param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static TResult LazyDeserialization<TResult>(byte[] bytes) => DeserializeValue<TResult>(bytes, DataFormat.Binary);
        
        /// <summary>
        /// Short version of <see cref="DeserializeValue{TResult}(byte[], DataFormat, List{UnityEngine.Object}, DeserializationContext)"/>
        /// for fast data deserialization from binary format, using the given list of Unity objects for external index reference resolution
        /// </summary>
        /// <param name="bytes">The bytes to deserialize from</param>
        /// <param name="referencedUnityObjects">The list of Unity objects to use for external index reference resolution</param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static TResult LazyDeserialization<TResult>(byte[] bytes, List<UnityEngine.Object> referencedUnityObjects) =>
            DeserializeValue<TResult>(bytes, DataFormat.Binary, referencedUnityObjects);

        #endregion
    }
}
