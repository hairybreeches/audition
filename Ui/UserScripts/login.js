var Sage50LoginModel = function () {
    var self = this;
    self.dataDirectory = ko.observable('');
    self.username = ko.observable('');
    self.password = ko.observable('');

    self.browseDataDirectory = function() {
        $.ajax('/api/chooseDirectory', {
            data: {
                startFolder: self.dataDirectory()
            },
            error: function() {

            },
            success: function(folderChosen) {
                self.dataDirectory(folderChosen);
            }
        });
    }

    self.submit = function() {
        model.login('/api/sage50/login', {
            username: self.username(),
            password: self.password(),
            dataDirectory: self.dataDirectory(),
        });
    }    
}

var LoginModel = function() {
    var self = this;
    self.blocked = ko.observable(false);
    self.system = ko.observable('');
    self.sage50 = new Sage50LoginModel();
    self.error = new ErrorMessage();

    var goToSearch = function() {
        location.href = '/views/search.html';
    };

    var showError = function(jqXHR) {
        self.blocked(false);
        self.error.show(jqXHR);
    };

    var startLogin = function() {
        self.error.visible(false);
        self.blocked(true);
    }

    self.login = function(url, data) {
        startLogin();
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            success: goToSearch,
            error: showError
        });
    }
}

var model = new LoginModel();

ko.applyBindings(model, document.getElementById('pageElement'));

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