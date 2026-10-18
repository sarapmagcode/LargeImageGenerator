using System.Drawing;
using System.Drawing.Imaging;

namespace LargeImageGenerator;

public class Program
{
    public static void Main(string[] args)
    {
        // Image dimensions (8k x 8k works well; increase for bigger file)
        var width = 8_000;
        var height = 8_000;

        var outputFolder = @"C:\Users\Admin\Downloads\temp";
        string outputPath = Path.Combine(outputFolder, "large_test_image.jpg");

        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }

        using var stream = new FileStream(outputPath, FileMode.Create);

        using var bmp = new Bitmap(width, height);
        Console.WriteLine("Creating file... Please wait");

        var rand = new Random();

        // Fill with random pixels to make JPEG large
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                Color c = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                bmp.SetPixel(x, y, c);
            }
        }

        // Save with high quality
        ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);

        Encoder myEncoder = Encoder.Quality;
        var encoderParams = new EncoderParameters(1);
        var encoderParam = new EncoderParameter(myEncoder, 100L); // 100 = max quality
        encoderParams.Param[0] = encoderParam;

        bmp.Save(stream, jpgEncoder, encoderParams);

        Console.WriteLine($"Large test image saved to {outputPath}");
    }

    private static ImageCodecInfo? GetEncoder(ImageFormat format)
    {
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
        foreach (ImageCodecInfo codec in codecs)
        {
            if (codec.FormatID == format.Guid)
            {
                return codec;
            }
        }

        return null;
    }
}