var Output = function (unavailableFields) {
    var self = this;
    //fields
    self.results = ko.observable([]);
    self.state = ko.observable('');
    self.lastError = ko.observable('');   

    //methods
    self.startSearch = function() {
        self.state('searching');
    };

    var showField = function(fieldName) {
        return !unavailableFields[fieldName];
    }

    self.showDescription = function() {
        return showField('description');
    };

    self.showUsername = function () {
        return showField('username');
    }

    self.searchSuccess = function(results) {
        self.state('results');
        self.results(results);
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

    self.showSearching = ko.computed(function() {
        return self.state() === 'searching';
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
    var serialise = function () {
        return JSON.stringify(ko.mapping.toJS({
            Period: period,
            Parameters: self.parameters
        }));
    };

    self.submit = function (_, e) {
        e.preventDefault();
        output.startSearch();
        $.ajax(searchUrl, {
            data: serialise(),
            contentType: 'application/json',
            success: output.searchSuccess,
            error: output.searchFailure,
            type: 'POST'
        });
    };

    self.save = function (_, e) {
        e.preventDefault();
        exportSuccessMessage.hide();

        $.ajax(exportUrl, {
            data: serialise(),

            contentType: 'application/json',
            success: function (fileName) {
                exportSuccessMessage.show(fileName);
            },
            type: 'POST'
        });
    };
}

var output = new Output(searchModel.unavilableFields);

var exportSuccessMessage = new ExportSuccessMessage();

var period = ko.mapping.fromJS({
    From: '2013-4-1',
    To: '2014-3-31'
});


var model = {
    input: {
        Period: period,

        Outside: new InputSection({
            FromDay: "Monday",
            ToDay: "Friday",
            FromTime: "08:00",
            ToTime: "18:00"
        }, period, output, exportSuccessMessage, '/api/search/hours', '/api/export/hours', searchModel.unavailableActions.hours),

        Accounts: new InputSection({
            minimumEntriesToBeConsideredNormal: 10
        }, period, output, exportSuccessMessage, '/api/search/accounts', '/api/export/accounts',searchModel.unavailableActions.accounts),

        Date: new InputSection({
            daysBeforeYearEnd: 10
        }, period, output, exportSuccessMessage, '/api/search/date', '/api/export/date', searchModel.unavailableActions.date),

        Users: new InputSection({
            users: ""
        }, period, output, exportSuccessMessage, '/api/search/users', '/api/export/users', searchModel.unavailableActions.username),

        Keyword: new InputSection({
            keywords: ""
        }, period, output, exportSuccessMessage, '/api/search/keyword', '/api/export/keyword', searchModel.unavailableActions.keyword),

        Ending: new InputSection({
            minimumZeroesToBeConsideredUnusual: 3
        }, period, output, exportSuccessMessage, '/api/search/ending', '/api/export/ending', searchModel.unavailableActions.ending)
    },
    output: output,
    exportSuccessMessage: exportSuccessMessage 
};

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

ko.applyBindings(model);
