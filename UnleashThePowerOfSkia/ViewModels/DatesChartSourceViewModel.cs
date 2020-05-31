using System;
using System.Collections.Generic;
using XamForms.Enhanced.ViewModels;

namespace UnleashThePowerOfSkia.ViewModels
{
    public class DatesChartSourceViewModel : BaseViewModel
    {
        public IList<AverageSaleValue> AverageSales { get; set; }
        
        public DatesChartSourceViewModel()
        {
            AverageSales = GenerateAverageSalesList();
        }

        private IList<AverageSaleValue> GenerateAverageSalesList()
        {
            double numberOfDaysToGenerateData = 50; //(double)new Random().Next(10, 20);
            var startDate = DateTime.Now.AddDays(-Math.Floor(numberOfDaysToGenerateData / 2));

            var result = new List<AverageSaleValue>();
            for (int i = 0; i < numberOfDaysToGenerateData; i++)
            {
                float salesValue = new Random().Next(5,40);
                result.Add(new AverageSaleValue(salesValue, startDate));
                startDate = startDate.AddDays(1);
            }

            return result;
        }
    }

    public struct AverageSaleValue
    {
        public AverageSaleValue(float value, DateTime day)
        {
            Value = value;
            Day = day;
        }

        public float Value { get; }
        public DateTime Day { get; }
    }
}