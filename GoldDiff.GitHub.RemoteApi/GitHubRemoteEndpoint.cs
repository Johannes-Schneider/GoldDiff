﻿using System;
using System.Threading.Tasks;
using GoldDiff.Shared.Http;

namespace GoldDiff.GitHub.RemoteApi
{
    public class GitHubRemoteEndpoint
    {
        private static TimeSpan RequestTimeout { get; } = TimeSpan.FromSeconds(2);
        private const string ReleaseUrl = @"https://api.github.com/repos/{0}/releases/latest";
        
        private static GitHubRemoteEndpoint? _instance;

        public static GitHubRemoteEndpoint Instance
        {
            get => _instance ??= new GitHubRemoteEndpoint();
        }

        private RestRequester Requester { get; }
        
        private GitHubRemoteEndpoint()
        {
            Requester = new RestRequester(RequestTimeout);
        }

        public async Task<GitHubReleaseInfo?> GetLatestReleaseAsync(string repositoryName)
        {
            return await Requester.GetAsync<GitHubReleaseInfo?>(string.Format(ReleaseUrl, repositoryName));
        }
    }
}