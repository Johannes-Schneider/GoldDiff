using System.Windows.Media;
using GoldDiff.LeagueOfLegends.Game;
using GoldDiff.Shared.View.Model;

namespace GoldDiff.View.Model
{
    public class GoldDifferenceWindowViewModel : ViewModel
    {
    #region Blue Side

        private LoLPlayer? _topPlayerBlueSide;

        public LoLPlayer? TopPlayerBlueSide
        {
            get => _topPlayerBlueSide;
            set => MutateVerbose(ref _topPlayerBlueSide, value);
        }

        private LoLPlayer? _middlePlayerBlueSide;

        public LoLPlayer? MiddlePlayerBlueSide
        {
            get => _middlePlayerBlueSide;
            set => MutateVerboseIfNotNull(ref _middlePlayerBlueSide, value);
        }

        private LoLPlayer? _bottomPlayerBlueSide;

        public LoLPlayer? BottomPlayerBlueSide
        {
            get => _bottomPlayerBlueSide;
            set => MutateVerboseIfNotNull(ref _bottomPlayerBlueSide, value);
        }

        private LoLPlayer? _junglePlayerBlueSide;

        public LoLPlayer? JunglePlayerBlueSide
        {
            get => _junglePlayerBlueSide;
            set => MutateVerboseIfNotNull(ref _junglePlayerBlueSide, value);
        }

        private LoLPlayer? _supportPlayerBlueSide;

        public LoLPlayer? SupportPlayerBlueSide
        {
            get => _supportPlayerBlueSide;
            set => MutateVerboseIfNotNull(ref _supportPlayerBlueSide, value);
        }

    #endregion
        
    #region Red Side

        private LoLPlayer? _topPlayerRedSide;

        public LoLPlayer? TopPlayerRedSide
        {
            get => _topPlayerRedSide;
            set => MutateVerbose(ref _topPlayerRedSide, value);
        }

        private LoLPlayer? _middlePlayerRedSide;

        public LoLPlayer? MiddlePlayerRedSide
        {
            get => _middlePlayerRedSide;
            set => MutateVerboseIfNotNull(ref _middlePlayerRedSide, value);
        }

        private LoLPlayer? _bottomPlayerRedSide;

        public LoLPlayer? BottomPlayerRedSide
        {
            get => _bottomPlayerRedSide;
            set => MutateVerboseIfNotNull(ref _bottomPlayerRedSide, value);
        }

        private LoLPlayer? _junglePlayerRedSide;

        public LoLPlayer? JunglePlayerRedSide
        {
            get => _junglePlayerRedSide;
            set => MutateVerboseIfNotNull(ref _junglePlayerRedSide, value);
        }

        private LoLPlayer? _supportPlayerRedSide;

        public LoLPlayer? SupportPlayerRedSide
        {
            get => _supportPlayerRedSide;
            set => MutateVerboseIfNotNull(ref _supportPlayerRedSide, value);
        }

    #endregion

        private Brush? _activePlayerOnBlueSideBackground;

        public Brush? ActivePlayerOnBlueSideBackground
        {
            get => _activePlayerOnBlueSideBackground;
            set => MutateVerbose(ref _activePlayerOnBlueSideBackground, value);
        }
        
        private Brush? _activePlayerOnRedSideBackground;

        public Brush? ActivePlayerOnRedSideBackground
        {
            get => _activePlayerOnRedSideBackground;
            set => MutateVerbose(ref _activePlayerOnRedSideBackground, value);
        }

        private Brush? _inactivePlayerBackground;

        public Brush? InactivePlayerBackground
        {
            get => _inactivePlayerBackground;
            set => MutateVerbose(ref _inactivePlayerBackground, value);
        }

        private Brush? _topPlayerBackground;

        public Brush? TopPlayerBackground
        {
            get => _topPlayerBackground;
            set => MutateVerbose(ref _topPlayerBackground, value);
        }
        
        private Brush? _junglePlayerBackground;

        public Brush? JunglePlayerBackground
        {
            get => _junglePlayerBackground;
            set => MutateVerbose(ref _junglePlayerBackground, value);
        }
        
        private Brush? _middlePlayerBackground;

        public Brush? MiddlePlayerBackground
        {
            get => _middlePlayerBackground;
            set => MutateVerbose(ref _middlePlayerBackground, value);
        }
        
        private Brush? _bottomPlayerBackground;

        public Brush? BottomPlayerBackground
        {
            get => _bottomPlayerBackground;
            set => MutateVerbose(ref _bottomPlayerBackground, value);
        }
        
        private Brush? _supportPlayerBackground;

        public Brush? SupportPlayerBackground
        {
            get => _supportPlayerBackground;
            set => MutateVerbose(ref _supportPlayerBackground, value);
        }

    }
}