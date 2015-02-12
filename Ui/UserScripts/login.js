var ImportModel = function() {
    var self = this;
    self.blocked = ko.observable(false);
    self.system = ko.observable('');
    self.sage50 = new Sage50ImportModel();
    self.excel = new ExcelImportModel();
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

var model = new ImportModel();

ko.applyBindings(model, document.getElementById('pageElement'));