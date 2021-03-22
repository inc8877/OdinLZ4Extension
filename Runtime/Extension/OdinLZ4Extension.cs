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

using K4os.Compression.LZ4;
using OdinLZ4Extension.Engine;
using Sirenix.Serialization;

namespace OdinLZ4Extension
{
    public static partial class OdinLZ4API
    {
        #region Serialize

        /// <summary>
        /// Short version of <see cref="SerializeValue(object, DataFormat, SerializationContext, OdinLZ4Level)"/>
        /// for fast data serialization in binary format
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] LazySerialization(object value) => SerializeValue(value, DataFormat.Binary);
        
        public static byte[] SerializeValue(object value, DataFormat format, SerializationContext ctx = null,
            OdinLZ4Level level = OdinLZ4Level.FAST) =>
            OdinLZ4Engine.Encode(SerializationUtility.SerializeValue(value, format, ctx), (LZ4Level)level);

        #endregion

        #region Deserialize

        /// <summary>
        /// Short version of <see cref="DeserializeValue{TResult}(byte[], DataFormat, DeserializationContext)"/>
        /// for fast data deserialization in binary format
        /// </summary>
        /// <param name="bytes"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        public static TResult LazyDeserialization<TResult>(byte[] bytes) => DeserializeValue<TResult>(bytes, DataFormat.Binary);
        
        public static TResult DeserializeValue<TResult>(byte[] bytes, DataFormat format, DeserializationContext ctx = null) =>
            SerializationUtility.DeserializeValue<TResult>(OdinLZ4Engine.Decode(bytes), format, ctx);
    

        #endregion
    }
}
