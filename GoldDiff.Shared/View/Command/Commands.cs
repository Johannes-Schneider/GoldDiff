using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace GoldDiff.Shared.View.Command
{
    public static class Commands
    {
        public static ICommand CloseWindow { get; } = new GenericCommand(parameter => parameter is Window,
                                                                         parameter =>
                                                                         {
                                                                             if (!(parameter is Window window))
                                                                             {
                                                                                 return;
                                                                             }

                                                                             window.Dispatcher.Invoke(() => window.Close());
                                                                         });

        public static ICommand MinimizeWindow { get; } = new GenericCommand(parameter => parameter is Window,
                                                                            parameter =>
                                                                            {
                                                                                if (!(parameter is Window window))
                                                                                {
                                                                                    return;
                                                                                }

                                                                                window.Dispatcher.Invoke(() => window.WindowState = WindowState.Minimized);
                                                                            });
        
        public static ICommand ToggleMaximizeWindow { get; } = new GenericCommand(parameter => parameter is Window,
                                                                            parameter =>
                                                                            {
                                                                                if (!(parameter is Window window))
                                                                                {
                                                                                    return;
                                                                                }

                                                                                window.Dispatcher.Invoke(() =>
                                                                                                         {
                                                                                                             return window.WindowState = window.WindowState != WindowState.Maximized
                                                                                                                                             ? WindowState.Maximized
                                                                                                                                             : WindowState.Normal;
                                                                                                         });
                                                                            });
        
        public static ICommand Copy { get; } = new GenericCommand(parameter =>
                                                                  {
                                                                      if (parameter is string stringParameter)
                                                                      {
                                                                          Clipboard.SetText(stringParameter);
                                                                          return;
                                                                      }
                                                                  });
        
        public static ICommand OpenUrl { get; } = new GenericCommand(parameter => parameter is string,
                                                                     parameter =>
                                                                     {
                                                                         if (!(parameter is string stringParameter))
                                                                         {
                                                                             return;
                                                                         }

                                                                         Process.Start(stringParameter);
                                                                     });
    }
}