using System;
using System.Collections.Generic;
using System.Linq;

namespace App
{
    public class Day14 : Day
    {
        protected override string Part1Code(string data)
        {
            var reactions = new Dictionary<(long n, string r), List<(long n, string r)>>();
            foreach (var line in data.Lines())
            {
                var left = line.Split("=>")[0].Trim();
                var right = line.Split("=>")[1].Trim();
                (long n, string r) output = (long.Parse(right.Split(" ")[0].Trim()), right.Split(" ")[1].Trim());
                var input = left.Split(", ").Select(x => x.Trim()).Select(x => (long.Parse(x.Split(" ")[0].Trim()), x.Split(" ")[1].Trim())).ToList();
                reactions[output] = input;
            }
            var any = reactions.Select(x => x.Key).GroupBy(x => x.r).Any(x => x.Count() > 1);
            if (any)
            {
                throw new Exception("unexpected duplicate output paths");
            }

            var restProducts = new Dictionary<string, long>();
            var requiredResources = new Dictionary<string, long>();
            requiredResources["FUEL"] = 1;
            while (requiredResources.Any(x => x.Key != "ORE" && x.Value > 0))
            {
                var satisfyMe = requiredResources.Where(x => x.Key != "ORE" && x.Value > 0).First();
                var amountNeeded = satisfyMe.Value;

                if (restProducts.ContainsKey(satisfyMe.Key))
                {
                    var usedRestProductsAmount = Math.Min(amountNeeded, restProducts[satisfyMe.Key]);
                    amountNeeded -= usedRestProductsAmount;
                    Add(restProducts, satisfyMe.Key, -usedRestProductsAmount);
                }

                var reaction = reactions.Single(x => x.Key.r == satisfyMe.Key);
                while (amountNeeded > 0)
                {
                    var reactionsNeeded = amountNeeded / reaction.Key.n;
                    if (amountNeeded % reaction.Key.n != 0)
                    {
                        reactionsNeeded++;
                    }
                    foreach (var input in reaction.Value)
                    {
                        Add(requiredResources, input.r, input.n * reactionsNeeded);
                    }
                    amountNeeded -= (reaction.Key.n * reactionsNeeded);
                }
                Add(restProducts, reaction.Key.r, -amountNeeded);
                requiredResources[satisfyMe.Key] = 0;
            }
            return requiredResources.First(x => x.Key == "ORE").Value.ToString();

        }

        private static void Add(Dictionary<string, long> d, string r, long n)
        {
            d[r] = (d.ContainsKey(r) ? d[r] : 0) + n;
        }

        protected override string Part2Code(string data)
        {
            var restProducts = new Dictionary<string, long>();
            restProducts["ORE"] = 1000000000000;

            var reactions = new Dictionary<(long n, string r), List<(long n, string r)>>();
            foreach (var line in data.Lines())
            {
                var left = line.Split("=>")[0].Trim();
                var right = line.Split("=>")[1].Trim();
                (long n, string r) output = (long.Parse(right.Split(" ")[0].Trim()), right.Split(" ")[1].Trim());
                var input = left.Split(", ").Select(x => x.Trim()).Select(x => (long.Parse(x.Split(" ")[0].Trim()), x.Split(" ")[1].Trim())).ToList();
                reactions[output] = input;
            }
            var any = reactions.Select(x => x.Key).GroupBy(x => x.r).Any(x => x.Count() > 1);
            if (any)
            {
                throw new Exception("unexpected duplicate output paths");
            }
            var fuelProduced = 0;
            while (true)
            {
                var requiredResources = new Dictionary<string, long>();
                requiredResources["FUEL"] = 1;
                while (requiredResources.Any(x => x.Value > 0))
                {
                    var satisfyMe = requiredResources.Where(x => x.Value > 0).First();
                    var amountNeeded = satisfyMe.Value;

                    if (restProducts.ContainsKey(satisfyMe.Key) && restProducts[satisfyMe.Key] > 0)
                    {
                        if (satisfyMe.Key == "ORE" && restProducts["ORE"] < satisfyMe.Value)
                        {
                            return fuelProduced.ToString();
                        }
                        var usedRestProductsAmount = Math.Min(amountNeeded, restProducts[satisfyMe.Key]);
                        amountNeeded -= usedRestProductsAmount;
                        Add(restProducts, satisfyMe.Key, -usedRestProductsAmount);
                    }
                    if (satisfyMe.Key != "ORE")
                    {
                        var reaction = reactions.Single(x => x.Key.r == satisfyMe.Key);
                        if (amountNeeded > 0)
                        {
                            var reactionsNeeded = amountNeeded / reaction.Key.n;
                            if (amountNeeded % reaction.Key.n != 0)
                            {
                                reactionsNeeded++;
                            }
                            foreach (var input in reaction.Value)
                            {
                                Add(requiredResources, input.r, input.n * reactionsNeeded);
                            }
                            amountNeeded -= (reaction.Key.n * reactionsNeeded);
                        }
                        if (amountNeeded < 0)
                        {
                            Add(restProducts, reaction.Key.r, -amountNeeded);
                        }

                    }
                    if (amountNeeded > 0)
                    {
                        throw new Exception("unexpected");
                    }
                    requiredResources.Remove(satisfyMe.Key);
                }
                fuelProduced++;
                if (fuelProduced % 10000 == 0)
                {
                    Console.WriteLine("Fuel produced: " + fuelProduced);
                    Console.WriteLine("Ore left: " + restProducts["ORE"]);
                }
            }
            throw new Exception("unexpected");
        }
    }
}