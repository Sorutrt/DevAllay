using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace DevAllay.Features.WorldManager;

public static class IconUpdater
{
    private const int TargetWidth = 800;
    private const int TargetHeight = 450;

    public static bool UpdateWorldIcon(string worldFolderPath, string sourceImagePath)
    {
        try
        {
            string targetPath = Path.Combine(worldFolderPath, "world_icon.jpeg");

            using (var sourceImage = Image.FromFile(sourceImagePath))
            {
                using (var resizedImage = ResizeAndCrop(sourceImage, TargetWidth, TargetHeight))
                {
                    // Save as JPEG
                    var jpegEncoder = GetEncoder(ImageFormat.Jpeg);
                    var encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 90L);
                    
                    resizedImage.Save(targetPath, jpegEncoder, encoderParameters);
                }
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    private static Bitmap ResizeAndCrop(Image source, int targetWidth, int targetHeight)
    {
        double sourceRatio = (double)source.Width / source.Height;
        double targetRatio = (double)targetWidth / targetHeight;

        int cropWidth, cropHeight, cropX, cropY;

        if (sourceRatio > targetRatio)
        {
            // Source is wider - crop width
            cropHeight = source.Height;
            cropWidth = (int)(cropHeight * targetRatio);
            cropX = (source.Width - cropWidth) / 2;
            cropY = 0;
        }
        else
        {
            // Source is taller - crop height
            cropWidth = source.Width;
            cropHeight = (int)(cropWidth / targetRatio);
            cropX = 0;
            cropY = (source.Height - cropHeight) / 2;
        }

        var bitmap = new Bitmap(targetWidth, targetHeight);
        using (var graphics = Graphics.FromImage(bitmap))
        {
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.CompositingQuality = CompositingQuality.HighQuality;

            var destRect = new Rectangle(0, 0, targetWidth, targetHeight);
            var srcRect = new Rectangle(cropX, cropY, cropWidth, cropHeight);

            graphics.DrawImage(source, destRect, srcRect, GraphicsUnit.Pixel);
        }

        return bitmap;
    }

    private static ImageCodecInfo GetEncoder(ImageFormat format)
    {
        var codecs = ImageCodecInfo.GetImageEncoders();
        foreach (var codec in codecs)
        {
            if (codec.FormatID == format.Guid)
            {
                return codec;
            }
        }
        return codecs[0];
    }
}
