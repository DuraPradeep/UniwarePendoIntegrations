using System.Text;

namespace Uniware_PandoIntegration.API.Model
{
    public static class ProcessWrite
    {
        public static async Task WriteTextAsync(string filePath, string text)
        {
            byte[] encodedText = Encoding.Unicode.GetBytes(text);

            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Create, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
            };
        }
    }

    public static class ProcessRead
    {
        public static async Task<string> ReadTextAsync(string filePath)
        {
            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Open, FileAccess.Read, FileShare.Read,
                bufferSize: 4096, useAsync: true))
            {
                StringBuilder sb = new StringBuilder();

                byte[] buffer = new byte[0x1000];
                int numRead=await sourceStream.ReadAsync(buffer, 0, buffer.Length);
                //while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                //{

                    //int bytesread = sourceStream.Read(buffer, 0, buffer.Length);
                    //string text = Encoding.ASCII.GetString(buffer, 0, bytesread);
                    string text = Encoding.ASCII.GetString(buffer, 0, numRead).Trim();
                    sb.Append(text);

                //}

                return sb.ToString();
            }
        }
    }
}
