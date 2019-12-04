using System.Linq;

namespace App
{
    public class Day1 : Day
    {
        protected override string Part1Code(string data) => data
                .Lines()
                .Select(int.Parse)
                .Select(FuelForMass)
                .Sum()
                .ToString();

        protected override string Part2Code(string data) => data
                .Lines()
                .Select(int.Parse)
                .Select(AddFuelRecursively)
                .Sum()
                .ToString();

        private static int FuelForMass(int mass) => (mass / 3) - 2;

        private static int AddFuelRecursively(int mass)
        {
            var massOfAddFuel = FuelForMass(mass);
            return massOfAddFuel <= 0 ? 0 : massOfAddFuel + AddFuelRecursively(massOfAddFuel);
        }
    }
}
