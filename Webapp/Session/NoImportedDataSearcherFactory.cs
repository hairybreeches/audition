﻿using Capabilities;
using Persistence;
using Searching;

namespace Webapp.Session
{
    public class NoImportedDataSearcherFactory : ISearcherFactory
    {
        public Searcher CreateSearcher()
        {
            throw new NoImportedDataException();
        }

        public SearchCapability GetSearchCapability()
        {
            throw new NoImportedDataException();
        }
    }
}