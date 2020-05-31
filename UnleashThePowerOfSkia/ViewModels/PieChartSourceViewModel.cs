using System;
using System.Windows.Input;
using UnleashThePowerOfSkia.Models.PieChartModels;
using Xamarin.Forms;
using XamForms.Enhanced.ViewModels;

namespace UnleashThePowerOfSkia.ViewModels
{
    public class PieChartSourceViewModel : BaseViewModel
    {
        public PieChartSourceViewModel()
        {
            PieChartSource = GenerateRandomData();
            Title = PieChartSource.Title;
            ToggleExplodeCommand = new Command(DoToggleExplodeCommand);
        }

        private void DoToggleExplodeCommand()
        {
            ShouldExplode = !ShouldExplode;
        }

        public BasePieChartSource PieChartSource { get; private set; }
        private bool _shouldExplode;

        public bool ShouldExplode
        {
            get => _shouldExplode;
            set => SetProperty(ref _shouldExplode, value);
        }

        public ICommand ToggleExplodeCommand { get; }

        private BasePieChartSource GenerateRandomData()
        {
            var rand = new Random().Next(1, 100);
            if (rand % 4 == 0)
            {
                return new WhoThinksIamBeautifulChartSource();
            }

            if (rand % 2 != 0)
            {
                return new PitbullChartSource();
            }

            if (rand % 2 == 0)
            {
                return new FearPhresesChartSource();
            }

            throw new NotSupportedException($"{rand} is not supported in {nameof(GenerateRandomData)}");
        }
    }
}