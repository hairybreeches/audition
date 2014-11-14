var Sage50LoginModel = function() {
    var self = this;
    self.dataDirectory = ko.observable('');
    self.username = ko.observable('');
    self.password = ko.observable('');

    self.browseDataDirectory = function() {
        $.ajax('/api/chooseDirectory', {
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
            success: function () {
                location.href = '/views/sage50Search.html';
            },
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
            success: function () {
                location.href = '/views/xeroSearch.html';
            },
            error: model.showError
        });
    }
}

var ErrorMessage = function () {
    var self = this;
    //fields
    self.visible = ko.observable(false);
    self.message = ko.observable('');

    //methods
    self.hide = function () {
        self.visible(false);
    };    
};


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
        var errorMessage = getErrorMessage(jqXHR);
        self.error.message(errorMessage);
        self.error.visible(true);
    };

}

var model = new LoginModel();

ko.applyBindings(model);