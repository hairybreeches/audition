var model = ko.mapping.fromJS({
        input: {
            Period: {
                From: "5/4/2013",
                To: "4/4/2013"
            },

            TimeFrame: {
                FromDay: "Monday",
                ToDay: "Friday",
                FromTime: "08:00",
                ToTime: "18:00"
            }
        },
        submit: function(data, e) {            
            e.preventDefault();
            $.get('/api/search', data.input, function(output) {
                console.log(output);
            });
        },

        output:[]
    });
ko.applyBindings(model);