var Output = function () {
    var self = this;
    //fields
    self.results = ko.observable([]);
    self.state = ko.observable('');
    self.lastError = ko.observable('');

    //methods
    self.startSearch = function() {
        self.state('searching');
    };

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

var model = {
    input: ko.mapping.fromJS({
        Period: {
            From: '2013-4-1',
            To: '2014-3-31'
        },

        Outside: {
            parameters: {
                FromDay: "Monday",
                ToDay: "Friday",
                FromTime: "08:00",
                ToTime: "18:00"
            },

            submit: function(_, e) {
                model.input.submit(e, '/api/search', model.input.Outside);
            },

            save: function(_, e) {
                model.input.save(e, '/api/search/export', model.input.Outside);
            },

            serialise: function() {
                return model.input.serialise(model.input.Outside.parameters);
            }
        },
        

        serialise: function(parameters) {
            return JSON.stringify(ko.mapping.toJS({
                Period: model.input.Period,
                Parameters: parameters
            }));
        },

        submit: function(e, url, data) {
            e.preventDefault();
            model.output.startSearch();
            $.ajax(url, {
                data: data.serialise(),
                contentType: 'application/json',
                success: model.output.searchSuccess,
                error: model.output.searchFailure,
                type: 'POST'
            });
        },

        save: function (e, url, data) {
            e.preventDefault();
            model.exportSuccessMessage.hide();

            $.ajax(url, {

                data: data.serialise(),

                contentType: 'application/json',
                success: function (fileName) {
                    model.exportSuccessMessage.show(fileName);
                },
                type: 'POST'
            });
        },
    }),

    output: new Output(),

    exportSuccessMessage: ko.mapping.fromJS({
        visible: false,
        hide: function () {
            model.exportSuccessMessage.visible(false);
        },
        fileName: '',

        openFile: function (_, e) {
            e.preventDefault();
            $.ajax('/api/openfile', {
                data: {
                    fileName: model.exportSuccessMessage.fileName
                },
                type: 'GET'
            });
        },

        show: function (fileName) {
            model.exportSuccessMessage.fileName(fileName);
            model.exportSuccessMessage.visible(true);
        }
    })
    
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
