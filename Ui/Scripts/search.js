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
            $.ajax('/api/search', {
                data: JSON.stringify(ko.mapping.toJS(model.input.parameters)),
                contentType: 'application/json',
                success: function(output) {
                    model.output.results(output);
                },
                type: 'POST'
            });
        }
    },
        
    output: {
        results: [],
        visible: function () { return model.state() === 'output'; }
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