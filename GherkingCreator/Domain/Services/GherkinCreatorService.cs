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

                        i++;
                    }
                    else if (element.Value.ValueKind == JsonValueKind.Object)
                    {
                        var jsonKeyValuePairAdd = BreakObject(json, element);

                        json.Remove(element.Key);
                        foreach (var item in jsonKeyValuePairAdd)
                            json.Add(item.Key, item.Value);

                        i++;
                    }
                    i++;
                }
            }
            return JsonSerializer.Serialize(json); ;
        }

        private static Dictionary<string, JsonElement> BreakArray(Dictionary<string, JsonElement> json, KeyValuePair<string, JsonElement> element)
        {
            var jsonKeyValuePairAdd = new Dictionary<string, JsonElement>();
            var valuesList = element.Value.EnumerateArray();
            var positionIndex = 1;

            foreach (var itemList in valuesList)
            {
                jsonKeyValuePairAdd = BreakObject(json, element);

                json.Remove(element.Key);
                foreach (var item in jsonKeyValuePairAdd)
                    jsonKeyValuePairAdd.Add($"{element.Key}{positionIndex}.{item.Key}", item.Value);

                positionIndex++;
            }
            return jsonKeyValuePairAdd;
        }

        private static Dictionary<string, JsonElement> BreakObject(Dictionary<string, JsonElement> json, KeyValuePair<string, JsonElement> element)
        {
            var jsonKeyValuePairAdd = new Dictionary<string, JsonElement>();
            Dictionary<string, JsonElement> valuesList = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(element.Value);

            foreach (var item in valuesList)
                jsonKeyValuePairAdd.Add($"{element.Key}.{item.Key}", item.Value);

            return jsonKeyValuePairAdd;
        }
    }
}