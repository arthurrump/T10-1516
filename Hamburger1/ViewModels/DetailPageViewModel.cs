using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hamburger1.Views;
using Template10.Common;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;

namespace Hamburger1.ViewModels
{
    public class DetailPageViewModel : ViewModelBase
    {
        public DetailPageViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                Value = "Designtime value";
            }

            CloseCurrentView = new DelegateCommand(() =>
            {
                string param = Value;

                Func<PageStackEntry, bool> isCurrentPage =
                    page => page.SourcePageType == typeof(DetailPage) && ((string)page.Parameter).Contains(param);

                while (NavigationService.Frame.BackStack.Any(isCurrentPage))
                {
                    NavigationService.Frame.BackStack.Remove(
                        NavigationService.Frame.BackStack.First(isCurrentPage)
                    );
                }

                NavigationService.GoBack();

                while (NavigationService.Frame.ForwardStack.Any(isCurrentPage))
                {
                    NavigationService.Frame.ForwardStack.Remove(
                        NavigationService.Frame.ForwardStack.First(isCurrentPage)
                    );
                }
                
                var navItems = Shell.Instance.HamburgerPrimaryButtons;
                navItems.Remove(navItems.Last(x => x.PageType == typeof(DetailPage) && (string)x.PageParameter == param));
            });
        }

        private string _Value = "Default";
        public string Value { get { return _Value; } set { Set(ref _Value, value); } }

        public DelegateCommand CloseCurrentView { get; }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> suspensionState)
        {
            Value = (suspensionState.ContainsKey(nameof(Value))) ? suspensionState[nameof(Value)]?.ToString() : parameter?.ToString();
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
    }
}
