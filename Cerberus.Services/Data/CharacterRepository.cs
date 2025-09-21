using Cerberus.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cerberus.Services.Data
{
    public class CharacterRepository
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true
        };

        public CharacterRepository(string filePath)
        {
            _filePath = filePath;
        }

        public CharacterDto GetById(long id)
        {
            var fileName = Path.Combine(_filePath, $"{id}.json");

            if (!File.Exists(fileName))
            {
                var character = new CharacterDto
                {
                    Id = id,
                    LastUpdated = DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc)
                };

                Save(character);

                return character;
            }

            var json = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<CharacterDto>(json, _options);
        }

        public void Save(CharacterDto character)
        {
            character.LastUpdated = DateTime.UtcNow;
            var json = JsonSerializer.Serialize(character, _options);
            var fileName = Path.Combine(_filePath, $"{character.Id}.json");

            // Ensure directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(fileName)!);

            // This will create or overwrite the file
            File.WriteAllText(fileName, json);
        }
    }
}
