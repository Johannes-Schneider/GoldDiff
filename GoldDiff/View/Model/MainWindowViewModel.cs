using GoldDiff.Shared.LeagueOfLegends;
using GoldDiff.Shared.View.Model;

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

        private LoLVersion _leagueVersion = LoLVersion.Zero;

        public LoLVersion LeagueVersion
        {
            get => _leagueVersion;
            set => MutateVerboseIfNotNull(ref _leagueVersion, value);
        }
    }
}