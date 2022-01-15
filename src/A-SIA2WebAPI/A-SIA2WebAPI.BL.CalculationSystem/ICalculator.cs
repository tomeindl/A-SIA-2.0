using A_SIA2WebAPI.Models;

namespace A_SIA2WebAPI.BL.CalculationSystem
{
    public interface ICalculator
    {
        public int Priority { get; set; }
        public NetworkStructure Calculate(NetworkStructure structure);
    }
}

