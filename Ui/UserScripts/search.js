var searchCapabilities = ko.mapping.fromJS({
    AvailableFields: [],
    UnvailableActionMessages: {
        Hours: false,
        NominalCodes: false,
        Users: false,
        Date: false,
        Ending: false,
        Duplicates: false
    }
});

$.ajax("/api/session/searchCapability", {
    success: function (data) {
        ko.mapping.fromJS(data, searchCapabilities);
    }
});

var Transaction = function(json) {

    var self = this;

    var userFriendlyDate = function (jsonDate) {
        var date = new Date(jsonDate);
        return date.toDateString();
    }     

    self.transactionDate = userFriendlyDate(json.TransactionDate);
    self.description = json.Description;
    self.username = json.Username;
    self.type = json.TransactionType;
    self.accountCode = json.AccountCode;
    self.lines = json.Lines.map(function(json) {
        return new LedgerEntry(json);
    });
}


var LedgerEntry = function (json) {
    var self = this;

    var formatCurrency = function (decimal) {
        return '£' + decimal.toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
    }

    self.ledgerEntryType = json.LedgerEntryType;
    self.nominalName = json.NominalName;
    self.amount = formatCurrency(json.Amount);
    self.nominalCode = json.NominalCode;    
}


var Output = function () {

    var self = this;
    //fields
    self.results = ko.observable([]);
    self.state = ko.observable('');
    self.lastError = ko.observable('');

    var currentPageNumber = ko.observable(1);
    var lastSearchUrl = '';
    var lastSearchWindow = {};

    var totalResults = ko.observable('');
    var firstResult = ko.observable(0);

    var setPage = function (pageNumber) {        
        model.search({
            pageNumber: pageNumber,
            searchUrl: lastSearchUrl,
            searchWindow: lastSearchWindow
        });
    };

    self.resultsComment = function() {        
        var lastResult = firstResult() + self.results().length - 1;
        return "Showing " + firstResult() + "-" + lastResult + " of " + totalResults() + " results";
    }

    self.isNextPage = ko.observable(false);
    self.isPreviousPage = ko.observable(false);
    
    self.goToNextPage = function()
    {        
        setPage(currentPageNumber() + 1);
    }

    self.goToPreviousPage = function() {
        setPage(currentPageNumber() - 1);
    }

    self.setLastSearch = function (search) {
        currentPageNumber(search.pageNumber);
        lastSearchUrl = search.searchUrl;
        lastSearchWindow = search.searchWindow;
    };

    self.searchSuccess = function(results) {
        self.state('results');
        self.results(results.Transactions.map(function(json) {
            return new Transaction(json);
        }));


        totalResults(results.TotalResults);
        self.isNextPage(results.IsNextPage);
        self.isPreviousPage(results.IsPreviousPage);
        firstResult(results.FirstResult);
    };

    self.searchFailure = function(jqXhr) {
        self.state('error');

        self.lastError(getErrorMessage(jqXhr));
    };

    //computed fields ("properties")
    var areResults = function() {
        return self.results().some(function() { return true; });
    };

    self.showApology = ko.computed(function() {
        return self.state() === 'results' && !areResults();
    });

    self.showResults = ko.computed(function() {        
        return self.state() === 'results' && areResults();
    });

    self.showError = ko.computed(function() {
        return self.state() === 'error';
    });
};

var ExportSuccessMessage = function () {
    var self = this;
    //fields
    self.visible = ko.observable(false);
    self.fileName = ko.observable('');

    //methods
    self.hide = function () {
        self.visible(false);
    };

    self.openFile = function (_, e) {
        e.preventDefault();
        $.ajax('/api/openfile', {
            data: JSON.stringify({
                fileName: self.fileName()
            }),
            type: 'POST',
            contentType: 'application/json'
        });
    };

    self.show = function (fileName) {
        self.fileName(fileName);
        self.visible(true);
    };
};


var InputSection = function (parameters, period, searchCapabilities, name) {

    var self = this;
    var exportUrl = '/api/export/' + name;
    var searchUrl = '/api/search/' + name;
    //fields
    self.parameters = ko.mapping.fromJS(parameters);
    self.blocked = ko.computed(function() {
        return !!searchCapabilities.UnvailableActionMessages[name]();
    });

    self.errorMessage = ko.computed(function() {
        return searchCapabilities.UnvailableActionMessages[name]() || "";
    });
        
    //methods

    var getSearchWindow = function() {
        return ko.mapping.toJS({
            Period: period,
            Parameters: self.parameters
        });
    }   

    var exportSerialise = function() {
        return JSON.stringify(getSearchWindow());
    }

    self.submit = function(_, e) {
        e.preventDefault();
        model.search({
            pageNumber: 1,
            searchWindow: getSearchWindow(),
            searchUrl: searchUrl
        });    
    };

    self.save = function (_, e) {
        e.preventDefault();
        model.export({
            url: exportUrl,
            data: exportSerialise()
        });
    };
}

var output = new Output();

var period = ko.mapping.fromJS({
    From: '2013-4-1',
    To: '2014-3-31'
});

var SearchModel = function () {

    var self = this;

    self.exportSuccessMessage = new ExportSuccessMessage();
    self.searching = ko.observable(false);

    var showField = function (fieldName) {
        return searchCapabilities.AvailableFields.indexOf(fieldName) !== -1;
    }

    var searchSerialise = function (searchWindow, pageNumber) {
        return JSON.stringify({
            pageNumber: pageNumber,
            searchWindow: searchWindow
        });
    };

    var finishSearch = function (search) {
        self.searching(false);
        output.setLastSearch(search);
    };

    self.search = function(search) {
        self.searching(true);
        self.exportErrorMessage.hide();
        $.ajax(search.searchUrl, {
            data: searchSerialise(search.searchWindow, search.pageNumber),
            contentType: 'application/json',
            success: output.searchSuccess,
            error: output.searchFailure,
            complete: function() {
                finishSearch(search);
            },
            type: 'POST'
        });
    };

    self.export = function(exportOptions) {
        self.exportSuccessMessage.hide();
        self.exportErrorMessage.hide();
        $.ajax(exportOptions.url, {
            data: exportOptions.data,

            contentType: 'application/json',
            success: function (exportResult) {
                if (exportResult.Completed) {
                    self.exportSuccessMessage.show(exportResult.Filename);
                }                
            },                
            error: self.exportErrorMessage.show,
            type: 'POST'
        });
    }    

    self.showNominalCode = function () {
        return showField('NominalCode');
    };
    self.showType = function () {
        return showField('Type');
    };
    self.showNominalName = function () {
        return showField('NominalName');
    };
    self.showAmount = function () {
        return showField('Amount');
    };
    self.showTransactionDate = function () {
        return showField('TransactionDate');
    };
    self.showLedgerEntryType = function () {
        return showField('LedgerEntryType');
    };
    self.showDescription = function() {
        return showField('Description');
    };
    self.showUsername = function() {
        return showField('Username');
    };
    self.showAccountCode = function() {
        return showField('AccountCode');
    };

    self.showEntries = function() {
        return self.showNominalCode() || self.showNominalName() || self.showAmount() || self.showLedgerEntryType();
    }

    self.exportErrorMessage = new ErrorMessage();

    self.input = {
        Period: period,

        NominalCodes: new InputSection({
            minimumEntriesToBeConsideredNormal: ko.observable(10)
        }, period, searchCapabilities, 'NominalCodes'),

        Users: new InputSection({
            users: ko.observable("")
        }, period, searchCapabilities, 'Users'),

        Ending: new InputSection({
            minimumZeroesToBeConsideredUnusual: ko.observable(3)
        }, period, searchCapabilities, 'Ending'),

        Duplicates: new InputSection({
            maximumDaysBetweenTransactions: ko.observable(31)
        }, period, searchCapabilities, 'Duplicates')
    };

    self.output = output;    
};


var model = new SearchModel();

ko.applyBindings(model, document.getElementById('pageElement'));
