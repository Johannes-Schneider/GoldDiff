using FlatXaml.Model;
using GoldDiff.LeagueOfLegends.Game;

namespace GoldDiff.View.Model
{
    public abstract class AbstractWindowViewModel : ViewModel
    {
        private LoLGame? _game;

        public LoLGame? Game
        {
            get => _game;
            set => MutateVerboseIfNotNull(ref _game, value);
        }
        
        private bool _isTopmost;

        public bool IsTopmost
        {
            get => _isTopmost;
            set => MutateVerbose(ref _isTopmost, value);
        }

        private bool _displayTitleBar = true;

        public bool DisplayTitleBar
        {
            get => _displayTitleBar;
            set => MutateVerbose(ref _displayTitleBar, value);
        }
    }
}