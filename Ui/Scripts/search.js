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
            $.ajax('/api/search', {
                data: { searchWindow: JSON.stringify(ko.mapping.toJS(data.input)) },
                contentType: 'application/json',
                success: function(output) {
                    console.log(output);
                },
                type: 'GET'
            });
        },         

        output:[]
    });
ko.applyBindings(model);