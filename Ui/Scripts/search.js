var model = ko.mapping.fromJS({
    input: {
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
                model.dataExport.save(e, '/api/search/export', model.input.Outside);
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
        }
    },

    output: {
        results: [],        
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

    dataExport: {

        save: function (e, url, data) {
            e.preventDefault();
            model.dataExport.successMessage.hide();            

            $.ajax(url, {

                data: data.serialise(),

                contentType: 'application/json',
                success: function (fileName) {
                    model.dataExport.successMessage.show(fileName);
                },
                type: 'POST'
            });
        },

        successMessage: {
            visible: false,
            hide: function() {
                model.dataExport.successMessage.visible(false);
            },
            fileName: '',

            openFile: function(_, e) {
                e.preventDefault();
                $.ajax('/api/openfile', {
                    data: {
                        fileName: model.dataExport.successMessage.fileName
                    },
                    type: 'GET'
                });
            },                       

            show: function (fileName) {
                model.dataExport.successMessage.fileName(fileName);
                model.dataExport.successMessage.visible(true);                
            }
        }
    }
});

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
