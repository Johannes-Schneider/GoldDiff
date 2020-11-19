using System.Windows;
using System.Windows.Controls;
using FlatXaml.View;
using GoldDiff.GitHub.RemoteApi;
using GoldDiff.Shared;
using GoldDiff.View.Resource;

namespace GoldDiff.View.Dialog
{
    public partial class UpdateApplicationDialog : FlatDialog
    {
        private string? Message
        {
            get => GetValue(MessageProperty) as string;
            set => SetValue(MessageProperty, value);
        }

        private static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof(Message), typeof(string), typeof(UpdateApplicationDialog));

        private string? ReleaseUrl
        {
            get => GetValue(ReleaseUrlProperty) as string;
            set => SetValue(ReleaseUrlProperty, value);
        }

        private static readonly DependencyProperty ReleaseUrlProperty = DependencyProperty.Register(nameof(ReleaseUrl), typeof(string), typeof(UpdateApplicationDialog));
        
        public UpdateApplicationDialog(GitHubReleaseInfo latestRelease)
        {
            InitializeComponent();

            Message = string.Format(UpdateApplicationDialogResources.Message, latestRelease.Version);
            ReleaseUrl = latestRelease.Url;
        }
    }
}