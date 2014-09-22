﻿using System.Collections;
using System.Collections.Generic;
using Model.Accounting;

namespace Model
{
    public interface IJournalSearcher
    {
        IEnumerable<Journal> FindJournalsWithin(HoursSearchWindow searchWindow);
    }
}
