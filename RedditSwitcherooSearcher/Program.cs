using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

class Program
{
    private static readonly HttpClient client = new HttpClient();

    static async Task Main(string[] args)
    {
        await ProcessComments();
    }

    private static async Task ProcessComments()
    {
        client.DefaultRequestHeaders.Accept.Clear();
        //client.DefaultRequestHeaders.Accept.Add() Add headers here
        client.DefaultRequestHeaders.Add("User-Agent", "Reddit Switcheroo Searcher");

        
    }
}