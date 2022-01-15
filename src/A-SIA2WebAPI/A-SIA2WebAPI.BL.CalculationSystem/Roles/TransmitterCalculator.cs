using A_SIA2WebAPI.Models;

namespace A_SIA2WebAPI.BL.CalculationSystem.Roles
{
    public class TransmitterCalculator : ICalculator
    {
        public int Priority { get; set; } = 100;

        public NetworkStructure Calculate(NetworkStructure structure)
        {
            //// Get all people who are receiving relations
            //List<Person> receiver = new(structure.People.Where(p => structure.InfluencesRelations.Where(r => r.To == p.Id).Any()));
            //// Get all people who emit relations
            //List<Person> publisher = new(structure.People.Where(p => structure.InfluencesRelations.Where(r => r.From == p.Id).Any()));

            //// Assign transmitter role to all who are emitting and receiving
            //receiver.Intersect(publisher).ToList().ForEach(p => p.Roles.Add("Transmitter"));

            return structure;
        }
    }
}
