﻿using System.Collections.Generic;
using Licensing;
using Model.Accounting;
using Persistence;
using Searching;
using Webapp.Controllers;

namespace Webapp.Session
{
    public class LoginSession
    {
        private readonly JournalSearcherFactoryStorage searcherFactoryStorage;
        private readonly IJournalRepository repository;
        private readonly ILicenceStorage licenceStorage;

        public LoginSession(JournalSearcherFactoryStorage searcherFactoryStorage, IJournalRepository repository, ILicenceStorage licenceStorage)
        {
            this.searcherFactoryStorage = searcherFactoryStorage;
            this.repository = repository;
            this.licenceStorage = licenceStorage;
        }

        public JournalSearcher GetCurrentJournalSearcher()
        {            
            licenceStorage.EnsureUseAllowed();
            return SearcherFactory.CreateJournalSearcher(repository);
        }

        public void Login(IJournalSearcherFactory newSearcherFactory, IEnumerable<Journal> journals)
        {
            searcherFactoryStorage.CurrentSearcherFactory = newSearcherFactory;
            repository.UpdateJournals(journals);
        }

        public void Logout()
        {
            searcherFactoryStorage.CurrentSearcherFactory = new NotLoggedInJournalSearcherFactory();
            repository.ClearJournals();
        }

        public SearchCapability GetCurrentSearchCapability()
        {
            return SearcherFactory.GetSearchCapability();
        }

        private IJournalSearcherFactory SearcherFactory
        {
            get { return searcherFactoryStorage.CurrentSearcherFactory; }
        }
    }
}
