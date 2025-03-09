using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ComputerGraphicsProject.Filters.Function;

namespace ComputerGraphicsProject.Utils
{
    public static class FilterStorage
    {
        private static string SavePath = "CustomFilters.json";

        public static void SaveFilter(CustomFunctionFilter filter)
        {
            List<CustomFunctionFilter> filters = LoadAllFilters();
            filters.RemoveAll(f => f.FilterName == filter.FilterName);
            filters.Add(filter);

            string json = JsonSerializer.Serialize(filters, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SavePath, json);
        }

        public static CustomFunctionFilter LoadFilter(string filterName)
        {
            List<CustomFunctionFilter> filters = LoadAllFilters();
            return filters.FirstOrDefault(f => f.FilterName == filterName);
        }

        public static List<CustomFunctionFilter> LoadAllFilters()
        {
            if (!File.Exists(SavePath)) return new List<CustomFunctionFilter>();

            string json = File.ReadAllText(SavePath);
            return JsonSerializer.Deserialize<List<CustomFunctionFilter>>(json) ?? new List<CustomFunctionFilter>();
        }
    }
}
