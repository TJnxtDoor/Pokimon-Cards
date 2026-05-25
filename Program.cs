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
        { // Define the API endpoint URL
            const string apiUrl = "https://pokeapi.co/api/v2/pokemon";
            var response = await Client.GetFromJsonAsync<PokemonResponse>(apiUrl);

            if (response?.Results is not null)
            {
                foreach (var pokemon in response.Results)
                {   // Capitalize the first letter of the Pokémon name for better display
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

    // Helper method to print a Pokémon's name and URL in a formatted card style
    private static void PrintPokemonCard(string name, string url)
    {
        const int cardWidth = 65;
        const int contentWidth = cardWidth - 2; 

        string nameRow = $"│ Name: {name}".PadRight(contentWidth) + "│";
        string urlRow  = $"│ URL:  {url}".PadRight(contentWidth) + "│";

        // Build top, middle separator, and bottom border lines
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

public record PokemonResponse(List<PokemonRecord> Results);
public record PokemonRecord(string Name, string Url);