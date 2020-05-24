using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UnleashThePowerOfSkia.Models;
using UnleashThePowerOfSkia.Views;
using Xamarin.Forms;
using XamForms.Enhanced.Providers;
using XamForms.Enhanced.ViewModels;

namespace UnleashThePowerOfSkia.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public INavigation Navigation { private get; set; }

        public MainPageViewModel()
        {
        }
        
        public ObservableCollection<PageItem> Pages => new ObservableCollection<PageItem>
        {
            new PageItem("Skia Watch", typeof(SkiaWatchPage))
        };
        
        public  ICommand PageSelectedCmd => new Command<PageItem>(async (page) =>
        {
            var pageObject = (Page) Activator.CreateInstance(page.PageType);
            await Navigation.PushAsync(pageObject);
        });
    }
}