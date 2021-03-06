﻿using System;
using System.IO;
using System.Net;
using System.Net.Http;

namespace App
{
    public abstract class Day
    {
        private static string cookieSession = File.ReadAllText("my-once-secret-session.secret");
        
        public Day()
        {
            int day = int.Parse(GetType().Name.Replace("Day", ""));
            var filename = $"Day{day}.txt";
            if (!File.Exists(filename))
            {
                File.WriteAllText(filename, DownloadData());    
            }

            Data = File.ReadAllText(filename);
        }

        private string Data { get; set; }

        public string Part1(string data=null)
        {
            return Part1Code(GetData(data));
        }

        public string Part2(string data=null)
        {
            return Part2Code(GetData(data));
        }

        protected abstract string Part1Code(string data);
        protected abstract string Part2Code(string data);

        protected virtual string GetData(string data = null)
        {
            return data ?? Data;
        }
        private string DownloadData()
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

