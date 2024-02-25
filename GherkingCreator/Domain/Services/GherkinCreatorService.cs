using System.Data;
using System.Text;
using System.Text.Json;

namespace GherkinCreator.Domain.Services
{
    public static class GherkinCreatorService
    {
        private static List<Dictionary<string, JsonElement>> DictionariesList = new();
        public static string CreateGherkin(dynamic jsonFromTransform)
        {
            var stringJson = JsonSerializer.Serialize(jsonFromTransform);
            
            if (jsonFromTransform.ValueKind is JsonValueKind.Array)
            {   
                var jsonsArray = JsonSerializer.Deserialize<JsonElement[]>(stringJson);
                foreach (var json in jsonsArray)
                {
                    DictionariesList.Add(JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json));
                }
            } else
            {
                DictionariesList.Add(JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonFromTransform));
            }

            List<Dictionary<string, JsonElement>> removedDictionariesList = new();
            List<Dictionary<string, JsonElement>> addedDictionariesList = new();

            foreach (var dictionary in DictionariesList)
            {
                var ordedDictionary = WriteGherkin(dictionary);
                addedDictionariesList.Add(ordedDictionary);
                removedDictionariesList.Add(dictionary);
            }

            foreach (var dictionary in removedDictionariesList)
            {
                DictionariesList.Remove(dictionary);
            }

            foreach (var dictionary in addedDictionariesList)
            {
                DictionariesList.Add(dictionary);
            }

            var tableBuilder = new StringBuilder();

            tableBuilder.Append("| ");
            foreach (var key in DictionariesList[0].Keys)
            {
                tableBuilder.Append($"{key} | ");
            }
            tableBuilder.AppendLine();

            foreach (var kvp in DictionariesList)
            {
                tableBuilder.Append("| ");
                foreach (var value in kvp.Values)
                {
                    tableBuilder.Append($"{value} | ");
                }
                tableBuilder.AppendLine();
            }

            return tableBuilder.ToString();
        }

        private static Dictionary<string, JsonElement> WriteGherkin(Dictionary<string, JsonElement> dictionary)
        {
            var i = 0;
            while (i < dictionary.Count)
            {
                foreach (var element in dictionary.ToList())
                {
                    if (element.Value.ValueKind == JsonValueKind.Array)
                    {
                        var jsonKeyValuePairAdd = BreakArray(element);

                        dictionary.Remove(element.Key);
                        foreach (var item in jsonKeyValuePairAdd)
                        {
                            var keyValue = item.Key.Replace(".", "_").Replace(item.Key[0], char.ToUpper(item.Key[0]));
                            dictionary.Add(keyValue, item.Value);
                        }
                    }
                    else if (element.Value.ValueKind == JsonValueKind.Object)
                    {
                        var jsonKeyValuePairAdd = BreakObject(element.Key, element: element);

                        dictionary.Remove(element.Key);
                        foreach (var item in jsonKeyValuePairAdd)
                        {
                            var keyValue = item.Key.Replace(".", "_").Replace(item.Key[0], char.ToUpper(item.Key[0]));
                            dictionary.Add(keyValue, item.Value);
                        }
                    }
                }
                i++;
            }
            return dictionary.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        }

        private static Dictionary<string, JsonElement> BreakArray(KeyValuePair<string, JsonElement> element)
        {
            var jsonKeyValuePairAdd = new Dictionary<string, JsonElement>();
            var valuesList = element.Value.EnumerateArray();
            var positionIndex = 1;

            foreach (var itemList in valuesList)
            {
                var BrokenElements = BreakObject(element.Key + positionIndex, jsonElement: itemList);

                foreach (var item in BrokenElements)
                    jsonKeyValuePairAdd.Add(item.Key, item.Value);

                positionIndex++;
            }
            return jsonKeyValuePairAdd;
        }

        private static Dictionary<string, JsonElement> BreakObject(string elementUpper, KeyValuePair<string, JsonElement>? element = null, JsonElement? jsonElement = null)
        {
            var jsonKeyValuePairAdd = new Dictionary<string, JsonElement>();
            Dictionary<string, JsonElement> valuesList = jsonElement != null
                    ? JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonElement.Value.GetRawText())
                    : JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(element.Value.Value.GetRawText());

            var upperCaseElement = elementUpper.Replace(".", "_").Replace(elementUpper[0], char.ToUpper(elementUpper[0]));
            foreach (var item in valuesList)
            {
                var keyValue =  item.Key.Replace(".", "_").Replace(item.Key[0], char.ToUpper(item.Key[0]));
                jsonKeyValuePairAdd.Add($"{upperCaseElement}.{keyValue}", item.Value);
            }

            return jsonKeyValuePairAdd;
        }
    }
}