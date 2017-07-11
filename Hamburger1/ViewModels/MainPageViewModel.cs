using Template10.Mvvm;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using Hamburger1.Views;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace Hamburger1.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                Value = "Designtime value";
            }
        }

        string _Value = "Gas";
        public string Value { get { return _Value; } set { Set(ref _Value, value); } }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            if (suspensionState.Any())
            {
                Value = suspensionState[nameof(Value)]?.ToString();
            }
            await Task.CompletedTask;
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> suspensionState, bool suspending)
        {
            if (suspending)
            {
                suspensionState[nameof(Value)] = Value;
            }
            await Task.CompletedTask;
        }

        public override async Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            await Task.CompletedTask;
        }

        public void GotoDetailsPage()
        {
            var navItemStack = new StackPanel { Orientation = Orientation.Horizontal };
            navItemStack.Children.Add(new SymbolIcon
            {
                Width = 48,
                Height = 48,
                Symbol = Symbol.Page
            });
            navItemStack.Children.Add(new TextBlock
            {
                Margin = new Thickness(12, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                Text = $"Details: {Value}"
            });

            Shell.Instance.HamburgerPrimaryButtons.Add(
                new Template10.Controls.HamburgerButtonInfo
                {
                    PageType = typeof(DetailPage),
                    PageParameter = Value,
                    Content = navItemStack
                });

            NavigationService.Navigate(typeof(DetailPage), Value);
        }
    }
}
