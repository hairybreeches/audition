var model = ko.mapping.fromJS({
        input: {
            Period: {
                From: '2013-4-5',
                To: '2014-4-4'                
            },

            Outside: {
                FromDay: "Monday",
                ToDay: "Friday",
                FromTime: "08:00:00",
                ToTime: "18:00:00"
            }
        },

        submit: function(data, e) {
            e.preventDefault();
            $.ajax('/api/search', {
                data: { searchWindow: JSON.stringify(ko.mapping.toJS(data.input)) },
                contentType: 'application/json',
                success: function(output) {
                    data.output(output);
                },
                type: 'GET'
            });
        },

        output: [],

        toggleShowExport : function() {
            model.dataExport.visible(!model.dataExport.visible());
        },

        dataExport: {
            visible: false,
            fileName: 'acme-possible-duplicated-journals',
            fileType: 'xlsx',
            save: function(data,e) {
                e.preventDefault();
                $.ajax('/api/search/export', {
                    data: {
                            saveRequest: JSON.stringify({
                                searchWindow: ko.mapping.toJS(model.input),
                                fileName: ko.mapping.toJS(data.fileName)
                        })
                    },
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