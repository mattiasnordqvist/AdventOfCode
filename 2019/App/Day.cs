using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;

namespace App
{
    public abstract class Day
    {
        private static string cookieSession = "53616c7465645f5fa6cc3fc4c2b5bc9901464963d7ed37f51480b47f4b621ae62c09894ce8171ee9e0c6ce09d3554e32";

        public List<(string, string)> part1Tests = new List<(string, string)>();
        public List<(string, string)> part2Tests = new List<(string, string)>();

        public Day()
        {
            int day = int.Parse(GetType().Name.Replace("Day", ""));
            var filename = $"Day{day}.txt";
            if (!File.Exists(filename))
            {
                File.WriteAllText(filename, GetData());    
            }

            Data = File.ReadAllText(filename);
        }

        private string Data { get; set; }

        public string Part1(string data=null)
        {
            return Part1Code(data ?? Data);
        }

        public string Part2(string data=null)
        {
            return Part2Code(data ?? Data);
        }

        protected abstract string Part1Code(string data);
        protected abstract string Part2Code(string data);

        public void AddTestForPart1(string input, string expected) {
            part1Tests.Add((input, expected));
        }

        public void AddTestForPart2(string input, string expected)
        {
            part2Tests.Add((input, expected));
        }

        private string GetData()
        {
            int day = int.Parse(GetType().Name.Replace("Day", ""));
            var baseAddress = new Uri("https://adventofcode.com/");
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            using (var client = new HttpClient(handler) { BaseAddress = baseAddress })
            {
                cookieContainer.Add(baseAddress, new Cookie("session", cookieSession));
                var result = client.GetAsync("/2019/day/" + day+"/input").Result;
                result.EnsureSuccessStatusCode();
                return result.Content.ReadAsStringAsync().Result;
            }
        }
    }
}

