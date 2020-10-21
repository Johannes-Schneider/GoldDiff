using System;
using System.IO;
using System.IO.Compression;
using GoldDiff.Shared.View.Model;

namespace GoldDiff.Shared.Archive
{
    public class ZipArchive
    {
        public static void ExtractToDirectory(FileInfo? archive, DirectoryInfo? outputDirectory, ProgressViewViewModel? progress)
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

            Extract(archive, outputDirectory, progress);
        }

        private static void Extract(FileInfo source, DirectoryInfo destination, ProgressViewViewModel progress)
        {
            if (Directory.Exists(destination.FullName))
            {
                destination.Delete(true);
            }

            destination.Create();

            var archive = new System.IO.Compression.ZipArchive(File.OpenRead(source.FullName));
            var currentEntryNumber = 0;
            var totalNumberOfEntries = archive.Entries.Count;
            foreach (var entry in archive.Entries)
            {
                progress.CurrentStepProgress = currentEntryNumber++ / (double) totalNumberOfEntries;

                if (entry == null)
                {
                    continue;
                }

                var entryDestination = Path.Combine(destination.FullName, entry.FullName);
                if (Path.GetFileName(entryDestination).Length < 1)
                {
                    // entry is directory
                    if (entry.Length != 0)
                    {
                        throw new Exception($"Unexpected error while extracting a {nameof(ZipArchive)}! ({nameof(entry)}.{nameof(entry.Length)} != 0)");
                    }

                    Directory.CreateDirectory(entryDestination);
                }
                else
                {
                    // entry is file
                    Directory.CreateDirectory(Path.GetDirectoryName(entryDestination)!);
                    entry.ExtractToFile(entryDestination, true);
                }
            }
        }
    }
}