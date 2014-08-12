var model = ko.mapping.fromJS({
        input: {
            Period: {
                From: '2012-4-5',
                To: '2013-4-4'                
            },

            TimeFrame: {
                FromDay: "Monday",
                ToDay: "Friday",
                FromTime: "08:00:00",
                ToTime: "18:00:00"
            }
        },
        submit: function(data, e) {            
            e.preventDefault();
            $.get('/api/search', { SearchWindow: JSON.stringify(ko.mapping.toJS(data.input)) }, function(output) {
                console.log(output);
            });
        },

        output:[]
    });
ko.applyBindings(model);