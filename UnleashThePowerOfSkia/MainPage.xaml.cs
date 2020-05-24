using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnleashThePowerOfSkia.ViewModels;
using Xamarin.Forms;

namespace UnleashThePowerOfSkia
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel ViewModel => (MainPageViewModel) BindingContext;
        
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainPageViewModel();
            ViewModel.Navigation = Navigation;
        }
    }
}
