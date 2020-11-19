using FlatXaml.Model;
using GoldDiff.Shared.LeagueOfLegends;
using GoldDiff.Shared.Utility;

namespace GoldDiff.View.Model
{
    public class MainWindowViewModel : ViewModel
    {
        private object? _content;

        public object? Content
        {
            get => _content;
            set => MutateVerbose(ref _content, value);
        }

        private StringVersion _leagueVersion = StringVersion.Zero;

        public StringVersion LeagueVersion
        {
            get => _leagueVersion;
            set => MutateVerboseIfNotNull(ref _leagueVersion, value);
        }
    }
}