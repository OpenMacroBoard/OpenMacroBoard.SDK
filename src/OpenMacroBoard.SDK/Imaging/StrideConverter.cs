using System;

namespace OpenMacroBoard.SDK.Imaging
{
    internal class StrideConverter
    {
        private readonly int bytesPerPixel;

        public StrideConverter(int bytesPerPixel)
        {
            this.bytesPerPixel = bytesPerPixel;
        }

        public void Transform(IntPtr sourceData, int sourceStart, IntPtr targetData, int targetStart)
        {
            unsafe
            {
                var src = (byte*)sourceData;
                var tar = (byte*)targetData;

                for (int i = 0; i < bytesPerPixel; i++)
                    tar[targetStart + i] = src[sourceStart + i];
            }
        }
    }
}
