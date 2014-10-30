﻿using Persistence;

namespace Searching
{
    public interface IJournalSearcherFactory
    {
        JournalSearcher CreateJournalSearcher(JournalRepository repository);
    }
}