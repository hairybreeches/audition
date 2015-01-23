var Sage50LoginModel = function() {
    var self = this;
    self.dataDirectory = ko.observable('');
    self.username = ko.observable('');
    self.password = ko.observable('');

    self.browseDataDirectory = createBrowseFunction('/api/chooseDirectory', self.dataDirectory);

    self.submit = function() {
        model.login('/api/sage50/login', {
            username: self.username(),
            password: self.password(),
            dataDirectory: self.dataDirectory(),
        });
    }

    $("#sage50dataDirectory").typeahead({
        source: function (request, response) {
            $.ajax({
                url: "/api/userdata/sage50DataLocations",
                data: {
                    enteredData: request.term
                },
                success: function (data) {
                    response(data);
                }
            });
        },
        minLength: 0
    });
};