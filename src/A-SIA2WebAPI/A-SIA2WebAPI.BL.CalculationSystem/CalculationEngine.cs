using A_SIA2WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace A_SIA2WebAPI.BL.CalculationSystem
{
    public class CalculationEngine
    {
        private readonly List<ICalculator> _calculators = new();

        public void AddCalculator(ICalculator calculator)
        {
            _calculators.Add(calculator);
            _calculators.Sort(new CalculatorComparer());
        }
        public void AddCalculatorRange(IEnumerable<ICalculator> calculators)
        {
            _calculators.AddRange(calculators);
            _calculators.Sort(new CalculatorComparer());
        }

        public NetworkStructure CalculateNetwork(NetworkStructure networkStructure, int time, params Type[] filter)
        {
            // Copy calc set
            List<ICalculator> calcSet = new(_calculators.ToArray());
            // Remove filter
            calcSet.RemoveAll(c => filter.Contains(c.GetType()));

            // Calculate
            int i = 0;
            do
            {
                foreach (ICalculator calculator in calcSet)
                {
                    networkStructure = calculator.Calculate(networkStructure);
                }
                i++;
            }
            while (i < time);

            return networkStructure;
        }

        private class CalculatorComparer : IComparer<ICalculator>
        {
            public int Compare(ICalculator? x, ICalculator? y)
            {
                return x?.Priority == y?.Priority ? 0 : (x?.Priority < y?.Priority ? 1 : -1);
            }
        }
    }

     
}
