/*
 * This file contains code that is used in the past but not now.
 * It is saved only for historic purposes.
 */

#region Marshal.Copy (Negative)
/*
 public static TimeSpan Negative(ImageData data, BackgroundWorker backgroundWorker) {
            int i = 0;
            int j = 0;
            int index = 0;

            // Lock the bitmap's bits.  
            BitmapData bmpData = data.M_bitmap.LockBits(new Rectangle(0, 0, data.M_width, data.M_height), ImageLockMode.ReadWrite, data.M_bitmap.PixelFormat);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap.
            int bytes = Math.Abs(bmpData.Stride)*data.M_bitmap.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            Stopwatch watch = Stopwatch.StartNew();

            #region Algorithm
            for (i = 0; i < data.M_width; i++) {
                backgroundWorker.ReportProgress(Convert.ToInt32(((double)i/data.M_width)*100));
                for (j = 0; j < data.M_height; j++) {
                    index = (j*bmpData.Stride) + (i*3);
                    rgbValues[index + 2] = (byte)(255 - rgbValues[index + 2]); // R
                    rgbValues[index + 1] = (byte)(255 - rgbValues[index + 1]); // G
                    rgbValues[index] = (byte)(255 - rgbValues[index]); // B
                }
            }
            #endregion

            watch.Stop();
            TimeSpan elapsedTime = watch.Elapsed;

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, bytes);

            // Unlock the bits.
            data.M_bitmap.UnlockBits(bmpData);

            return elapsedTime;
}
            */
#endregion