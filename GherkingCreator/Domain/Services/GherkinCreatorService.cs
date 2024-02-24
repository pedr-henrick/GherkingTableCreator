using System.Linq;
using System.Text.Json;

namespace GherkinCreator.Domain.Services
{
    public static class GherkinCreatorService
    {
        //private static List<dynamic> GherkinList = new();

        public static string CreateGherkin(dynamic jsonFromTransform)
        {
            var stringJson = JsonSerializer.Serialize(jsonFromTransform);
            Dictionary<string, JsonElement> json = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(stringJson);
            return WriteGherkin(json);
        }

        private static string WriteGherkin(Dictionary<string, JsonElement> json)
        {
            var i = 0;
            while (i < json.Count)
            {
                foreach (var element in json.ToList())
                {
                    if (element.Value.ValueKind == JsonValueKind.Array)
                    {
                        var jsonKeyValuePairAdd = BreakArray(json, element);

                        json.Remove(element.Key);
                        foreach (var item in jsonKeyValuePairAdd)
                            json.Add(item.Key, item.Value);
                    }
                    else if (element.Value.ValueKind == JsonValueKind.Object)
                    {
                        var jsonKeyValuePairAdd = BreakObject(element.Key, element: element);

                        json.Remove(element.Key);
                        foreach (var item in jsonKeyValuePairAdd)
                            json.Add(item.Key, item.Value);
                    }
                }
                i++;
            }
            var dicionarioOrdenado = json.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            return JsonSerializer.Serialize(dicionarioOrdenado);
        }

        private static Dictionary<string, JsonElement> BreakArray(Dictionary<string, JsonElement> json, KeyValuePair<string, JsonElement> element)
        {
            var jsonKeyValuePairAdd = new Dictionary<string, JsonElement>();
            var valuesList = element.Value.EnumerateArray();
            var positionIndex = 1;

            foreach (var itemList in valuesList)
            {
                var BrokenElements = BreakObject(element.Key+positionIndex, jsonElement: itemList);
                
                foreach (var item in BrokenElements)
                    jsonKeyValuePairAdd.Add(item.Key, item.Value);
                
                positionIndex++;
            }
            return jsonKeyValuePairAdd;
        }

        private static Dictionary<string, JsonElement> BreakObject(string elementUpper, KeyValuePair<string, JsonElement>? element = null, JsonElement? jsonElement = null)
        {
            var jsonKeyValuePairAdd = new Dictionary<string, JsonElement>();
            Dictionary<string, JsonElement> valuesList =  jsonElement != null
                    ? JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonElement.Value.GetRawText())
                    : JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(element.Value.Value.GetRawText());

            foreach (var item in valuesList)
                jsonKeyValuePairAdd.Add($"{elementUpper}.{item.Key}", item.Value);

            return jsonKeyValuePairAdd;
        }
    }
}