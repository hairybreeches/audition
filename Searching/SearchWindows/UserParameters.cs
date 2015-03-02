using System;
using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.Time;
using Persistence;

namespace Searching.SearchWindows
{
    public class UserParameters : ISearchParameters
    {
        public IList<string> Usernames { get; private set; }

        public UserParameters(string users)
        {
            Usernames = InputParsing.ParseStringList(users);
        }

        public IQueryable<Transaction> ApplyFilter(Searcher searcher, IQueryable<Transaction> transactions)
        {
            return searcher.FindTransactionsWithin(this, transactions);
        }

        protected bool Equals(UserParameters other)
        {
            return Usernames.SequenceEqual(other.Usernames);
        }

        public override string ToString()
        {
            return "posted by users other than " + GetUsersDescriptionString();
        }

        private string GetUsersDescriptionString()
        {
            if (!Usernames.Any())
            {
                return "(no users entered)";
            }
            if (Usernames.Count == 1)
            {
                return Usernames.Single();
            }

            return String.Join(", ", Usernames.Take(Usernames.Count - 1)) + " or " + Usernames.Last();


        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UserParameters) obj);
        }

        public override int GetHashCode()
        {
            return (Usernames != null ? Usernames.GetHashCode() : 0);
        }
    }
}