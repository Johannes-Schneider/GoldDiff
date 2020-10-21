using System;

namespace GoldDiff.Shared.Http
{
    public class DownloadProgressEventArguments
    {
        public double Progress { get; }
        
        public double AverageDownloadSpeedInMBs { get; }
        
        public TimeSpan EstimatedRemainingTime { get; }

        public DownloadProgressEventArguments(double progress, double averageDownloadSpeedInMBs, TimeSpan estimatedRemainingTime)
        {
            Progress = progress;
            AverageDownloadSpeedInMBs = averageDownloadSpeedInMBs;
            EstimatedRemainingTime = estimatedRemainingTime;
        }
    }
}