using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using FlatXaml.Model;

namespace GoldDiff.Shared.Archive
{
    // Taken from https://gist.github.com/ForeverZer0/a2cd292bd2f3b5e114956c00bb6e872b
    public class TarGZipArchive
    {
        private static int ChunkSize { get; } = 4096;

        public static void ExtractToDirectory(FileInfo? archive, DirectoryInfo? outputDirectory, Progression? progress)
        {
            if (archive == null)
            {
                throw new ArgumentNullException(nameof(archive));
            }
            
            if (outputDirectory == null)
            {
                throw new ArgumentNullException(nameof(outputDirectory));
            }

            if (progress == null)
            {
                throw new ArgumentNullException(nameof(progress));
            }

            if (!File.Exists(archive.FullName))
            {
                throw new Exception($"Unable to find {nameof(archive)} ({archive.FullName})!");
            }

            if (!Directory.Exists(outputDirectory.FullName))
            {
                outputDirectory.Create();
            }
            
            using var stream = File.OpenRead(archive.FullName);
            Extract(stream, outputDirectory, progress);

            progress.CurrentStepProgress = 1.0d;
        }

        private static void Extract(Stream stream, DirectoryInfo outputDirectory, Progression progress)
        {
            // A GZipStream is not seekable, so copy it first to a MemoryStream
            using var gzip = new GZipStream(stream, CompressionMode.Decompress);
            using var memStr = new MemoryStream();
            
            int read;
            var buffer = new byte[ChunkSize];
            do
            {
                read = gzip.Read(buffer, 0, ChunkSize);
                memStr.Write(buffer, 0, read);

                progress.CurrentStepProgress = (double) stream.Position / stream.Length * 0.5d; // multiply by 0.5 because we also need to extract the tar archive afterwards

            } while (read == ChunkSize);

            memStr.Seek(0, SeekOrigin.Begin);
            ExtractTar(memStr, outputDirectory, progress);
        }

        private static void ExtractTar(Stream stream, DirectoryInfo outputDirectory, Progression progress)
        {
            var fileBuffer = new byte[ChunkSize];
            var buffer = new byte[100];
            while (true)
            {
                stream.Read(buffer, 0, 100);
                var name = Encoding.ASCII.GetString(buffer).Trim('\0');
                
                if (string.IsNullOrWhiteSpace(name))
                {
                    break;
                }

                stream.Seek(24, SeekOrigin.Current);
                stream.Read(buffer, 0, 12);
                var size = Convert.ToInt64(Encoding.UTF8.GetString(buffer, 0, 12).Trim('\0').Trim(), 8);
                stream.Seek(376L, SeekOrigin.Current);

                var output = new FileInfo(Path.Combine(outputDirectory.FullName, name));
                if (output.Directory?.Exists == false)
                {
                    output.Directory?.Create();
                }

                if (!name.Equals("./", StringComparison.InvariantCulture) && size > 0)
                {
                    using var str = File.Open(output.FullName, FileMode.OpenOrCreate, FileAccess.Write);
                    var read = 0;
                    while (read < size)
                    {
                        var nextChunkSize = (int) Math.Min(fileBuffer.Length, size - read);
                        stream.Read(fileBuffer, 0, nextChunkSize);
                        str.Write(fileBuffer, 0, nextChunkSize);

                        read += nextChunkSize;
                        progress.CurrentStepProgress = 0.5d + (double) stream.Position / stream.Length * 0.5d;
                    }
                }

                var pos = stream.Position;

                var offset = 512 - (pos % 512);
                if (offset == 512)
                {
                    offset = 0;
                }

                stream.Seek(offset, SeekOrigin.Current);
                progress.CurrentStepProgress = 0.5d + (double) stream.Position / stream.Length * 0.5d;
            }
        }
    }
}