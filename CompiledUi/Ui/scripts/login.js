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