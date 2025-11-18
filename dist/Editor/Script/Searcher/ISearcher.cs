using System;
using System.Collections.Generic;

namespace Henry.EditorKit
{
    public interface ISearcher
    {
        void Search(string query, Action<IEnumerable<string>> onCompleted);
    }
}