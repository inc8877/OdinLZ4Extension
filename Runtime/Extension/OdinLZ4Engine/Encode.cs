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

using K4os.Compression.LZ4;
using K4os.Compression.LZ4.Internal;

namespace OdinLZ4Extension.Engine
{
    internal static partial class OdinLZ4Engine
    {
        #region Encode
        
        internal static byte[] Encode(byte[] source, LZ4Level level = LZ4Level.L00_FAST)
        {
            unsafe
            {
                int sourceLength = source.Length;
                
                source.Validate(0, sourceLength);

                fixed (byte* sourceTemp = source)
                {
                    if (sourceLength <= 0) return Mem.Empty;
                    int targetLength1 = sourceLength - 1;
                    byte* target = (byte*) Mem.Alloc(sourceLength);
                    
                    try
                    {
                        int targetLength2 = LZ4Codec.Encode(
                            sourceTemp, sourceLength,
                            target, targetLength1,
                            level);
                        
                        return targetLength2 <= 0
                            ? PickleV0(sourceTemp, sourceLength, sourceLength)
                            : PickleV0(target, targetLength2, sourceLength);
                    }
                    finally
                    {
                        Mem.Free((void*) target);
                    }
                }
            }
            
        }

        #endregion

        #region Pickle

        private static unsafe byte[] PickleV0(byte* target, int targetLength, int sourceLength)
        {
            var diff = sourceLength - targetLength;
            var llen = diff == 0 ? 0 : diff < 0x100 ? 1 : diff < 0x10000 ? 2 : 4;
            var result = new byte[targetLength + 1 + llen];

            fixed (byte* resultP = result)
            {
                var llenFlags = llen == 4 ? 3 : llen; // 2 bits
                var flags = (byte) ((llenFlags << 6) | Constants.CurrentVersion);
                Mem.Poke1(resultP + 0, flags);
                if (llen == 1) Mem.Poke1(resultP + 1, (byte) diff);
                else if (llen == 2) Mem.Poke2(resultP + 1, (ushort) diff);
                else if (llen == 4) Mem.Poke4(resultP + 1, (uint) diff);
                Mem.Move(resultP + llen + 1, target, targetLength);
            }

            return result;
        }

        #endregion
    }
}