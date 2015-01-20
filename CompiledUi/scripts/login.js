var goToSearch = function() {
    location.href = '/views/search.html';
};

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
        model.startLogin();
        $.ajax({
            type: "POST",
            url: '/api/sage50/login',
            data: {
                 username: self.username(),
                 password: self.password(),
                 dataDirectory: self.dataDirectory(),
            },
            success: goToSearch,
            error: model.showError
        });
    }
}


var XeroLoginModel = function () {
    var self = this;
    self.code = ko.observable('');

    self.initialiseLogin = function() {
        $.ajax({
            type: "POST",
            url: '/api/xero/initialiselogin'
        });
    }

    self.submit = function () {
        model.startLogin();
        $.ajax({
            type: "POST",
            url: '/api/xero/login',
            data: { code: self.code },
            success: goToSearch,
            error: model.showError
        });
    }
}

var LoginModel = function () {
    var self = this;
    self.blocked = ko.observable(false);
    self.system = ko.observable('');
    self.sage50 = new Sage50LoginModel();
    self.xero = new XeroLoginModel();
    self.error = new ErrorMessage();

    self.startLogin = function () {
        self.error.visible(false);
        self.blocked(true);
    }

    self.showError = function (jqXHR) {
        self.blocked(false);
        self.error.show(jqXHR);
    };

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
