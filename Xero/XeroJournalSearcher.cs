﻿using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Accounting;
using Model.SearchWindows;
using Model.Time;
using Journal = XeroApi.Model.Journal;

namespace Xero
{
    public class XeroJournalSearcher : IJournalSearcher
    {
        private readonly IFullRepository repository;

        public XeroJournalSearcher(IFullRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Model.Accounting.Journal> FindJournalsWithin(SearchWindow<WorkingHours> searchWindow)
        {
            var periodJournals = GetJournalsApplyingTo(searchWindow.Period).ToList();

            return periodJournals.Where(x => Matches(x, searchWindow.Parameters)).Select(x => x.ToModelJournal());
        }

        public IEnumerable<Model.Accounting.Journal> FindJournalsWithin(SearchWindow<UnusualAccountsParameters> searchWindow)
        {
            var periodJournals = GetJournalsApplyingTo(searchWindow.Period).ToList();
            var lookup = new AccountsLookup(periodJournals);
            return lookup.JournalsMadeToUnusualAccountCodes(searchWindow.Parameters.MinimumEntriesToBeConsideredNormal)
                .Select(XeroJournalExtensions.ToModelJournal);
        }

        public IEnumerable<Model.Accounting.Journal> FindJournalsWithin(SearchWindow<YearEndParameters> searchWindow)
        {
            var periodJournals = GetJournalsApplyingTo(searchWindow.Period).ToList();

            var startOfSearchPeriod = searchWindow.CreationStartDate();
            return periodJournals.Where(x => x.UkCreationTime() >= startOfSearchPeriod).Select(x => x.ToModelJournal());            
        }

        public IEnumerable<Model.Accounting.Journal> FindJournalsWithin(SearchWindow<UserParameters> searchWindow)
        {
            throw new NotSupportedException("Xero does not record who raises individual journals");
        }

        public IEnumerable<Model.Accounting.Journal> FindJournalsWithin(SearchWindow<KeywordParameters> searchWindow)
        {
            throw new NotSupportedException("Xero does not have the concept of descriptions");
        }

        public IEnumerable<Model.Accounting.Journal> FindJournalsWithin(SearchWindow<EndingParameters> searchWindow)
        {
            var periodJournals = GetJournalsApplyingTo(searchWindow.Period).ToList();
            var magnitude = (int) Math.Pow(10,searchWindow.Parameters.MinimumZeroesToBeConsideredUnusual);
            return periodJournals.Where(journal => HasRoundLine(journal, magnitude))
                .Select(x=>x.ToModelJournal());
        }

        private bool HasRoundLine(Journal journal, int magnitude)
        {
            return journal.JournalLines.Exists(line => ContainsRoundValue(line, magnitude));
        }

        private bool ContainsRoundValue(XeroApi.Model.JournalLine line, int magnitude)
        {
            return IsRound(line.GrossAmount, magnitude)
                   || IsRound(line.TaxAmount, magnitude)
                   || IsRound(line.NetAmount, magnitude);
        }

        public bool IsRound(decimal amount, int magnitude)
        {
            var pence = amount*100;
            return pence !=0 && pence%magnitude == 0;
        }

        private static bool Matches(Journal x, WorkingHours workingHours)
        {
            return !workingHours.Contains(x.UkCreationTime());
        }

        private IEnumerable<Journal> GetJournalsApplyingTo(DateRange period)
        {
            return repository.Journals.ToList().Where(x => period.Contains(x.JournalDate));
        }
    }
}
