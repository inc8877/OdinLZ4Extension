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

using System.Diagnostics;
using K4os.Compression.LZ4;
using Sirenix.Serialization;

namespace OdinLZ4Extension
{
    public static partial class OdinLZ4API
    {
        /// <summary>
        /// Some stuff for testing
        /// </summary>
        public static class UsefulStuff
        {
            /// <summary>
            /// Just calculates the compression efficiency
            /// </summary>
            /// <param name="value"></param>
            /// <param name="format"></param>
            /// <param name="ctx"></param>
            /// <param name="compressionLevel"></param>
            /// <returns></returns>
            public static string GetEfficiency(object value, DataFormat format, SerializationContext ctx = null,
                LZ4Level compressionLevel = LZ4Level.L00_FAST)
            {
                float compressedLengh = SerializeValue(value, format, ctx, compressionLevel).Length;
                float uncompressedLengh = SerializationUtility.SerializeValue(value, format, ctx).Length;
                return (100f - ((compressedLengh / uncompressedLengh) * 100f)).ToString("F3") + " %";
            }
            
            /// <summary>
            /// Counts the time spent on compression
            /// </summary>
            /// <param name="value"></param>
            /// <param name="format"></param>
            /// <param name="ctx"></param>
            /// <param name="compressionLevel"></param>
            /// <returns></returns>
            public static string CalculatingSpeed(object value, DataFormat format, SerializationContext ctx = null,
                LZ4Level compressionLevel = LZ4Level.L00_FAST)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                
                SerializeValue(value, format, ctx, compressionLevel);
                SerializationUtility.SerializeValue(value, format, ctx);
                
                stopwatch.Stop();
                return "Elapsed time : " + stopwatch.ElapsedMilliseconds + "(ms)";
            }
        }
    }
}