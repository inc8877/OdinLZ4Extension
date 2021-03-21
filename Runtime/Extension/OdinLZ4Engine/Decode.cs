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

// ---

/*
Copyright (c) 2017 Milosz Krajewski

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

#endregion

using System;
using System.IO;
using K4os.Compression.LZ4;
using K4os.Compression.LZ4.Internal;

namespace OdinLZ4Extension.Engine
{
    internal static partial class OdinLZ4Engine
    {
        #region Decode

        internal static byte[] Decode(byte[] source)
        {
            unsafe
            {
                int sourceLength = source.Length;
                
                source.Validate(0, sourceLength);

                fixed (byte* sourceTemp = source)
                {
                    if (sourceLength <= 0) return Mem.Empty;

                    byte flags = *sourceTemp;
                    int num = (int) flags & 7;
                    if (num == 0) return UnpickleV0(flags, sourceTemp + 1, sourceLength - 1);
                    throw new InvalidDataException(string.Format("Pickle version {0} is not supported", (object) num));
                }
            }
        }

        #endregion

        #region Unpickle

        private static unsafe byte[] UnpickleV0(byte flags, byte* source, int sourceLength)
        {
            // ReSharper disable once IdentifierTypo
            var llen = (flags >> 6) & 0x03; // 2 bits
            if (llen == 3) llen = 4;

            if (sourceLength < llen)
                throw Exceptions.CorruptedPickle("Source buffer is too small.");

            var diff = (int) (
                llen == 0 ? 0 :
                llen == 1 ? *source :
                llen == 2 ? *(ushort*) source :
                llen == 4 ? *(uint*) source :
                throw Exceptions.CorruptedPickle("Unexpected length descriptor.")
            );
            source += llen;
            sourceLength -= llen;
            var targetLength = sourceLength + diff;

            var target = new byte[targetLength];
            fixed (byte* targetP = target)
            {
                if (diff == 0)
                {
                    Mem.Copy(targetP, source, targetLength);
                }
                else
                {
                    var decodedLength = LZ4Codec.Decode(
                        source, sourceLength, targetP, targetLength);
                    if (decodedLength != targetLength)
                        throw new ArgumentException(
                            $"Expected {targetLength} bytes but {decodedLength} has been decoded");
                }
            }

            return target;
        }

        #endregion
    }
}