﻿using Model.Persistence;

namespace Model.Searching
{
    public interface IJournalSearcherFactory
    {
        JournalSearcher CreateJournalSearcher(JournalRepository repository);
    }
}