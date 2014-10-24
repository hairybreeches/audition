var Output = function () {
    var self = this;
    //fields
    self.results = ko.observable([]);
    self.state = ko.observable('');
    self.lastError = ko.observable('');   
      
    self.searchSuccess = function(results) {
        self.state('results');
        self.results(results.Journals);
    };

    self.searchFailure = function(jqXhr, textStatus, errorThrown) {
        self.state('error');
        self.lastError(textStatus + " : " + errorThrown);
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
            data: {
                fileName: self.fileName()
            },
            type: 'GET'
        });
    };

    self.show = function (fileName) {
        self.fileName(fileName);
        self.visible(true);
    };
};


var InputSection = function (parameters, period, output, exportSuccessMessage, searchUrl, exportUrl, blocked) {

    var self = this;
    //fields
    self.parameters = ko.mapping.fromJS(parameters);
    self.blocked = ko.observable(blocked || false);
    //methods

    var getSearchWindow = function() {
        return ko.mapping.toJS({
            Period: period,
            Parameters: self.parameters
        });
    }

    var searchSerialise = function() {
        return JSON.stringify({
            pageNumber: 1,
            searchWindow: getSearchWindow()
        });
    };

    var exportSerialise = function() {
        return JSON.stringify({
            SerialisationOptions: {
                showUsername: model.showUsername(),
                showDescription: model.showDescription(),
            },
            searchWindow: getSearchWindow()
        });
    }

    self.submit = function (_, e) {
        e.preventDefault();
        model.startSearch();
        $.ajax(searchUrl, {
            data: searchSerialise(),
            contentType: 'application/json',
            success: output.searchSuccess,
            error: output.searchFailure,
            complete: model.finishSearch,
            type: 'POST'
        });
    };

    self.save = function (_, e) {
        e.preventDefault();
        exportSuccessMessage.hide();

        $.ajax(exportUrl, {
            data: exportSerialise(),

            contentType: 'application/json',
            success: function (fileName) {
                exportSuccessMessage.show(fileName);
            },
            type: 'POST'
        });
    };
}

var output = new Output();

var exportSuccessMessage = new ExportSuccessMessage();

var period = ko.mapping.fromJS({
    From: '2013-4-1',
    To: '2014-3-31'
});

var showField = function(fieldName) {
    return !searchModel.unavilableFields[fieldName];
}

var SearchModel = function () {

    var self = this;

    self.searching = ko.observable(false);

    self.startSearch = function() {
        self.searching(true);
    };

    self.finishSearch = function() {
        self.searching(false);
    };

    self.showDescription = function() {
        return showField('description');
    };

    self.showUsername = function() {
        return showField('username');
    };

    self.input = {
        Period: period,

        Outside: new InputSection({
            FromDay: "Monday",
            ToDay: "Friday",
            FromTime: "08:00",
            ToTime: "18:00"
        }, period, output, exportSuccessMessage, '/api/search/hours', '/api/export/hours', searchModel.unavailableActions.hours),

        Accounts: new InputSection({
            minimumEntriesToBeConsideredNormal: 10
        }, period, output, exportSuccessMessage, '/api/search/accounts', '/api/export/accounts', searchModel.unavailableActions.accounts),

        Date: new InputSection({
            daysBeforeYearEnd: 10
        }, period, output, exportSuccessMessage, '/api/search/date', '/api/export/date', searchModel.unavailableActions.date),

        Users: new InputSection({
            users: ""
        }, period, output, exportSuccessMessage, '/api/search/users', '/api/export/users', searchModel.unavailableActions.username),

        Ending: new InputSection({
            minimumZeroesToBeConsideredUnusual: 3
        }, period, output, exportSuccessMessage, '/api/search/ending', '/api/export/ending', searchModel.unavailableActions.ending)
    };
    self.output = output;
    self.exportSuccessMessage = exportSuccessMessage;
};

var model = new SearchModel();
//todo: these functions belong on a journal object
var userFriendlyDate = function(jsonDate) {
    var date = new Date(jsonDate);
    return date.toDateString();
}

var userFriendlyDateTime = function (jsonDate) {
    var date = new Date(jsonDate);
    return getTimeString(date) + ' ' + date.toDateString();
}

var getTimeString = function(date){
    return date.toLocaleTimeString("en-UK", {hour: '2-digit', minute: '2-digit', hour12:false});
}

var formatCurrency = function(decimal) {
    return '£' + decimal.toFixed(2).replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
}

ko.applyBindings(model);
