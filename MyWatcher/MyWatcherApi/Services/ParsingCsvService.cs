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
    void ParsingContinentsFromCsv(DatabaseContext dbContext);
    void ParsingCountriesFromCsv(DatabaseContext dbContext);
    void ParsingWebsitesFromCsv(DatabaseContext dbContext);
}

public class ParsingCsvService : IParsingCsvService
{
    public void ParsingContinentsFromCsv(DatabaseContext dbContext)
    {
        var continents = new List<Continent>();
        var lines = ReadCsvFile("Resources/continents.csv");
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
        dbContext.Continents.AddRange(continents);
        dbContext.SaveChanges();
        Console.WriteLine("continents ended");
    }

    public void ParsingCountriesFromCsv(DatabaseContext dbContext)
    {
        Console.WriteLine("countries started");
        var continents = new Dictionary<string, Continent>();
        try
        {
            //dbContext.Continents.ForEachAsync(x => continents.TryAdd(x.Name, x));
            dbContext.Continents.ToList().ForEach(x => continents.TryAdd(x.Name, x));
        }
        catch (Exception e)
        {
            Console.WriteLine("Failed at retrieving existing continents with {0} ", e);
        }

        var countries = new List<Country>();
        var lines = ReadCsvFile("Resources/countries.csv");
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
        dbContext.Countries.AddRange(countries);
        dbContext.SaveChanges();
    }

    public void ParsingWebsitesFromCsv(DatabaseContext dbContext)
    {
        
    }

    
    //private IEnumerable<string[]> ReadCsvFile(string filePath)
    private List<string[]> ReadCsvFile(string filePath)
    {
        var list = new List<string[]>();
        using (var reader = new StreamReader(filePath))
        {
            reader.ReadLine(); //throw away format line
            while (!reader.EndOfStream)
            {
                //yield return reader.ReadLine().Split(";");
                list.Add((reader.ReadLine()).Split(";").Select(x => x.Trim()).ToArray());
            }
        }
        return list;
    }
}