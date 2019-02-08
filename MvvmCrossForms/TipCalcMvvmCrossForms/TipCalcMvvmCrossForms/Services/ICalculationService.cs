using System;
using System.Collections.Generic;
using System.Text;

namespace TipCalcMvvmCrossForms.Services
{
    public interface ICalculationService
    {
        double TipAmount(double subTotal, int generosity);
    }

    public class CalculationService : ICalculationService
    {
        public double TipAmount(double subTotal, int generosity)
        {
            return subTotal * ((double) generosity) / 100.0;
        }
    }
}
