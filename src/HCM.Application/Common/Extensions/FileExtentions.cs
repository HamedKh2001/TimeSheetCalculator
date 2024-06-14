using System.IO;
using System.Text;

namespace HCM.Application.Common.Extensions;

public static class FileExtentions
{
    public static MemoryStream GenerateCsvMemoryStream(this string input)
    {
        // Create a MemoryStream to store CSV content
        MemoryStream memoryStream = new MemoryStream();

        // Write the input string to the MemoryStream
        byte[] bytes = Encoding.UTF8.GetBytes(input);
        memoryStream.Write(bytes, 0, bytes.Length);

        // Reset the MemoryStream position to the beginning
        memoryStream.Position = 0;

        return memoryStream;
    }
}