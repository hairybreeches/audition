var model = ko.mapping.fromJS({
    input: {
        parameters: {
            Period: {
                From: '2013-4-5',
                To: '2014-4-4'
            },

            Outside: {
                FromDay: "Monday",
                ToDay: "Friday",
                FromTime: "08:00",
                ToTime: "18:00"
            }
        },

        submit: function(data, e) {
            e.preventDefault();
            model.state('output');
            model.output.startSearch();
            $.ajax('/api/search', {
                data: JSON.stringify(ko.mapping.toJS(model.input.parameters)),
                contentType: 'application/json',
                success: model.output.searchSuccess,
                error: model.output.searchFailure,
                type: 'POST'
            });
        }
    },

    output: {
        results: [],
        visible: function() { return model.state() === 'output'; },
        state: '',

        startSearch: function() {
            model.output.state('searching');
        },

        searchSuccess: function (results) {
            model.output.state('results');
            model.output.results(results);
        },

        searchFailure: function(jqXhr, textStatus, errorThrown) {
            model.output.state('error');
            model.output.lastError(textStatus + " : " + errorThrown);
        },

        showApology: function () {
            var output = model.output;
            return output.state() === 'results' && !output.areResults();
        },

        areResults: function() {
            return model.output.results().some(function() { return true; });
        },

        showResults: function () {
            var output = model.output;
            return output.state() === 'results' && output.areResults();
        },

        showSearching: function() {
            return model.output.state() === 'searching';
        },

        showError: function () {
            return model.output.state() === 'error';
        },

        lastError: ''
    },

    toggleShowExport: function () {
        var newState = model.state() !== 'export' ? 'export' : 'output';
        model.state(newState);
    },

    state: '',

    dataExport: {
        visible: function () { return model.state() === 'export'; },
        fileName: '',
        browse: function (data, e) {
            e.preventDefault();
            $.ajax('/api/choosefile', {

                data: {
                    current: data.fileName(),
                },

                success: function (newName) {
                    data.fileName(newName);
                },
                type: 'GET'
            });
        },
        save: function (data, e) {
            e.preventDefault();
            $.ajax('/api/search/export', {

                data: JSON.stringify({
                    searchWindow: ko.mapping.toJS(model.input.parameters),
                    fileName: ko.mapping.toJS(data.fileName)
                }),

                contentType: 'application/json',
                success: function () {
                    console.log("Saved file");
                },
                type: 'POST'
            });
        }
    }
});

var userFriendlyDate = function(jsonDate) {
    var date = new Date(jsonDate);
    return date.toDateString();
}

var userFriendlyDateTime = function (jsonDate) {
    var date = new Date(jsonDate);
    return date.toTimeString() + ' ' + date.toDateString();
}
ko.applyBindings(model);