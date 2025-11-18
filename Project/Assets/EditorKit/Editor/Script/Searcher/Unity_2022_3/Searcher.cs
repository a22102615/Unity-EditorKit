// 用於 Unity 2022.3 的 Searcher 實作
#if UNITY_2022_3
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Search;

namespace Henry.EditorKit
{
    [InitializeOnLoad]
    internal sealed class Searcher : ISearcher
    {
        const string type = "EditorKitComp";
        const string displayName = "EditorKit Components";
        const string queryPrefix = "editorKitComp:";

        struct CustomSearchItemFields
        {
            public const string compInfo = "compInfo";
            public const string typefullname = "typefullname";
        }

        static Searcher()
        {
            SearcherManager.SetSearcher(new Searcher());
        }

        [SearchItemProvider]
        static SearchProvider GetProvider()
        {
            return new SearchProvider(type, displayName)
            {
                filterId = queryPrefix,
                fetchItems = (context, items, provider) =>
                {
                    var searchPhrase = context.searchPhrase;
                    var els = ComponentRegistry.List;
                    var result = new List<SearchItem>();

                    foreach (var el in els)
                    {
                        var config = el.Config;
                        var item = new SearchItem($"componentinfo/{config.Name.Replace(" ", "-").ToLower()}")
                        {
                            label = config.Name,
                            description = config.Description,
                        };
                        item.SetField(CustomSearchItemFields.compInfo, $"{config.Name} {config.Description} {config.Author} {config.Version}");
                        item.SetField(CustomSearchItemFields.typefullname, el.TypeFullName);
                        result.Add(item);
                    }

                    return result;
                }
            };
        }

        public void Search(string query, Action<IEnumerable<string>> onCompleted)
        {
            SearchService.Request($"{queryPrefix} {query}", OnSearchCompleted);

            void OnSearchCompleted(SearchContext context, IList<SearchItem> items)
            {
                var result = new List<(string typefullname, long score)>();
                foreach (var item in items)
                {
                    item.TryGetValue(CustomSearchItemFields.compInfo, out var compInfoField);
                    var outScore = 0L;
                    if (FuzzySearch.FuzzyMatch(query, compInfoField.value.ToString(), ref outScore))
                    {
                        item.TryGetValue(CustomSearchItemFields.typefullname, out var typefullnameField);
                        result.Add((typefullnameField.value.ToString(), outScore));
                    }
                }

                onCompleted(result.OrderBy(el => el.score).Select(el => el.typefullname));
            }
        }
    }
}
#endif
