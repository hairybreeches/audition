var model = ko.mapping.fromJS({
        input: {
            Period: {
                From: '2012-4-5',
                To: '2013-4-4'                
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

        output:[]
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