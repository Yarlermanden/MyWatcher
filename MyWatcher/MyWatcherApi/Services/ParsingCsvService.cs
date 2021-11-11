using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyWatcher.Entities;

namespace MyWatcher.Services;

public interface IParsingCsvService
{
    Task ParsingContinentsFromCsv(DatabaseContext dbContext);
    Task ParsingCountriesFromCsv(DatabaseContext dbContext);
    Task ParsingWebsitesFromCsv(DatabaseContext dbContext);
}

public class ParsingCsvService : IParsingCsvService
{
    public async Task ParsingContinentsFromCsv(DatabaseContext dbContext)
    {
        var continents = new List<Continent>();
        var lines = await ReadCsvFile("Resources/continents.csv");
        foreach (var line in lines)
        {
            try
            {
                var continent = new Continent()
                {
                    Name = line[0],
                    Code = line[1]
                };
                continents.Add(continent);
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed at parsing line in continent csv: {0}", e.Message);
            }
        }
        await dbContext.Continents.AddRangeAsync(continents);
        await dbContext.SaveChangesAsync();
        Console.WriteLine("continents ended");
    }

    public async Task ParsingCountriesFromCsv(DatabaseContext dbContext)
    {
        Console.WriteLine("countries started");
        var continents = new Dictionary<string, Continent>();
        try
        {
            await dbContext.Continents.ForEachAsync(x => continents.TryAdd(x.Name, x));
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed at retrieving existing continents with {0} ", e);
        }

        var countries = new List<Country>();
        var lines = await ReadCsvFile("Resources/countries.csv");
        foreach (var line in lines)
        {
            try
            {
                var continent = continents[line[0]];
                var country = new Country()
                {
                    Name = line[1],
                    CountryCode = line[2],
                    Continent = continent,
                };
                countries.Add(country);
                //await dbContext.Countries.AddAsync(country);
                //await dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed at parsing line in country csv: {0}", e.Message);
            }
        }
        await dbContext.Countries.AddRangeAsync(countries);
        await dbContext.SaveChangesAsync();
    }

    public async Task ParsingWebsitesFromCsv(DatabaseContext dbContext)
    {
        
    }

    
    //private IEnumerable<string[]> ReadCsvFile(string filePath)
    private async Task<List<string[]>> ReadCsvFile(string filePath)
    {
        var list = new List<string[]>();
        using (var reader = new StreamReader(filePath))
        {
            reader.ReadLine(); //throw away format line
            while (!reader.EndOfStream)
            {
                //yield return reader.ReadLine().Split(";");
                list.Add((await reader.ReadLineAsync()).Split(";").Select(x => x.Trim()).ToArray());
            }
        }
        return list;
    }
}