using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PokemonAsyncDemo;

class Program
{
    private static readonly HttpClient Client = new();

    static async Task Main(string[] args)
    {
        Console.WriteLine("Fetching Pokémon data asynchronously...\n");

        try
        {  // Fetch the list of Pokémon from the API
            const string apiUrl = "https://pokeapi.co/api/v2/pokemon";
            var response = await Client.GetFromJsonAsync<PokemonResponse>(apiUrl);

            if (response?.Results is not null)
            {
                foreach (var pokemon in response.Results)
                {  
                    string capitalizedName = char.ToUpper(pokemon.Name[0]) + pokemon.Name[1..];
                    
                    PrintPokemonCard(capitalizedName, pokemon.Url);
                }
            }
            else
            {
                Console.WriteLine("No data received from the endpoint.");
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Network error fetching data: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }

        Console.WriteLine("\nPress any key to exit.");
        Console.ReadKey();
    }

    
    private static void PrintPokemonCard(string name, string url)
    {
        const int cardWidth = 65;
        const int contentWidth = cardWidth - 2; 

        string nameRow = $"│ Name: {name}".PadRight(contentWidth) + "│";
        string urlRow  = $"│ URL:  {url}".PadRight(contentWidth) + "│";

        
        string topBorder = "┌" + new string('─', contentWidth - 1) + "┐";
        string botBorder = "└" + new string('─', contentWidth - 1) + "┘";

        // Output the card block to the console
        Console.WriteLine(topBorder);
        Console.WriteLine(nameRow);
        Console.WriteLine(urlRow);
        Console.WriteLine(botBorder);
        Console.WriteLine(); // Small spacer between cards
    }
}

public enum PokemonType
{
    Normal, Fire, Water, Grass, Electric, Ice, Fighting, Poison,
    Ground, Flying, Psychic, Bug, Rock, Ghost, Dragon, Dark,
    Steel, Fairy
}

public enum PokemonGerartion
{
    Gen1, Gen2, Gen3, Gen4, Gen5, Gen6, Gen7, Gen8, Gen9
}

public record PokemonCard(string Name, PokemonType Type, PokemonGerartion Generation);
public record PokemonCardDetails(string Name, PokemonType Type, PokemonGerartion Generation, string Description);


public record PokemonResponse(List<PokemonRecord> Results);
public record PokemonRecord(string Name, string Url);