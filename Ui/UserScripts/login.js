var Sage50LoginModel = function () {
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
}

var ExcelLoginModel = function() {
    var self = this;

    self.fileLocation = ko.observable('');
    var fields = ['JournalDate', 'Description', 'Username', 'Created', 'AccountCode', 'AccountName', 'Amount', 'JournalType'];
    fields.forEach(
        function(fieldName) {
            self[fieldName] = ko.observable('A');
        });

    var getData = function() {
        var data = {
            Filename: self.fileLocation()
        };

        data.Lookups = fields.reduce(function(lookups, fieldName) {
            lookups[fieldName] = self[fieldName]();
            return lookups;
        }, {});

        return data;
    }

    self.browseExcelFile = createBrowseFunction('/api/chooseExcelFile', self.fileLocation);

    self.submit = function() {
        model.login('/api/excel/login', getData());
    };
}

var LoginModel = function() {
    var self = this;
    self.blocked = ko.observable(false);
    self.system = ko.observable('');
    self.sage50 = new Sage50LoginModel();
    self.excel = new ExcelLoginModel();
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
            contentType: 'application/json',
            type: "POST",
            url: url,
            data: JSON.stringify(data),
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