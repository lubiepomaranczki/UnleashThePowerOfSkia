using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UnleashThePowerOfSkia.Models;
using UnleashThePowerOfSkia.Views;
using Xamarin.Forms;
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
            new PageItem("Skia Watch ⌚️", typeof(SkiaWatchPage)),
            new PageItem("User drawing 👨‍🎨", typeof(UserDrawingPage)),
            new PageItem("Bar chart 📊", typeof(BarChartPage)),
            new PageItem("Line chart 📊", typeof(LineChartPage)),
            new PageItem("Pie chart 📊", typeof(PieChartPage))
        };
        
        public  ICommand PageSelectedCmd => new Command<PageItem>(async (page) =>
        {
            var pageObject = (Page) Activator.CreateInstance(page.PageType);
            await Navigation.PushAsync(pageObject);
        });
    }
}