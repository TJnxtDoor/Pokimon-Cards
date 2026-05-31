using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization; 
using System.Linq;
using System.Threading.Tasks;

namespace PokemonAsyncDemo
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            Console.WriteLine("Getting Pokemon data...");

            try
            {       // API end point of  pokemon 
                string url = "https://pokeapi.co/api/v2/pokemon?limit=5";

                MyPokemonResponse? result = await client.GetFromJsonAsync<MyPokemonResponse?>(url);

                if (result?.Results != null)
                {
                    foreach (MyPokemonRecord p in result.Results)
                    {
                        MyPokemonDetails? details = await client.GetFromJsonAsync<MyPokemonDetails?>(p.Url ?? string.Empty);

                        //
                        string finalName = string.IsNullOrEmpty(p.Name) 
                            ? "Unknown" 
                            : char.ToUpper(p.Name[0]) + p.Name.Substring(1);

                            //converted string to enum and if failes go back to unknown 
                        string typeString = details?.Types?.FirstOrDefault()?.Type?.Name ?? "Unknown";
                        if (!Enum.TryParse(typeString, true, out PokemonType typeEnum))
                        {
                            typeEnum = PokemonType.Unknown;
                        }

                        DateTime rightNow = DateTime.Now;

                        //Console output for the Pokemon details
                        Console.WriteLine("========================================");
                        Console.WriteLine("Pokemon:    " + finalName);
                        Console.WriteLine("Type:       " + typeEnum.ToString());
                        Console.WriteLine("Fetched On: " + rightNow.ToString("g"));
                        
                        if (details?.Sprites != null)
                        {
                            Console.WriteLine("Picture:    " + details.Sprites.FrontDefault);
                        }
                        
                        Console.WriteLine("========================================");
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            Console.ReadLine();
        }
    }

    public enum PokemonType
    {
        Normal, Fire, Water, Grass, Electric, Ice, Fighting, Poison,
        Ground, Flying, Psychic, Bug, Rock, Ghost, Dragon, Dark,
        Steel, Fairy, Unknown
    }

    public class MyPokemonResponse
    {
        [JsonPropertyName("results")]
        public List<MyPokemonRecord>? Results { get; set; }
    }

    public class MyPokemonRecord
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }

    public class MyPokemonDetails
    {
        [JsonPropertyName("types")]
        public List<MyTypeSlot>? Types { get; set; }
        
      
        [JsonPropertyName("sprites")]
        public MyPokemonSprites? Sprites { get; set; }
    }

    public class MyTypeSlot
    {
        [JsonPropertyName("type")]
        public MyTypeInfo? Type { get; set; }
    }

    public class MyTypeInfo
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

    public class MyPokemonSprites
    {
        [JsonPropertyName("front_default")]
        public string? FrontDefault { get; set; }
    }
}

// please consider my code may look like a alcholic wrote this but i wanna get this demo done for next weeks class. hopefully  its good enough,
// when exucting code with dotnet run, it fetches the pokemon api with deatails such as "name" "type" "fetched on" and url pics of pokemon 



