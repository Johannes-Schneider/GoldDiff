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
    }
}