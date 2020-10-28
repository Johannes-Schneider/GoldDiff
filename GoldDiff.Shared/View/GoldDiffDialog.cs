using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

namespace GoldDiff.Shared.View
{
    [TemplatePart(Name = "PART_BlurryBackground", Type = typeof(Rectangle))]
    public class GoldDiffDialog : Window
    {
        private Rectangle? _blurryBackground;
        
        public GoldDiffDialog()
        {
            Style = Application.Current?.Resources[GoldDiffSharedResourceKeys.DefaultDialogStyle] as Style;
            WindowState = WindowState.Minimized;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Width = Owner?.Width ?? 400;
            Height = Owner?.Height ?? 400;
            Top = Owner?.Top ?? 0;
            Left = Owner?.Left ?? 0;
            WindowState = WindowState.Normal;
        }
        
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _blurryBackground = GetTemplateChild("PART_BlurryBackground") as Rectangle ?? throw new Exception($"Unable to get the PART_BlurryBackground template part!");
            
            _blurryBackground.MouseLeftButtonDown += BlurryBackground_OnMouseLeftButtonDown;
        }

        private void BlurryBackground_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_blurryBackground?.IsMouseDirectlyOver == false)
            {
                return;
            }

            DialogResult = false;
            Close();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == Key.Escape)
            {
                Close();
            }
        }
    }
}