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
        model.blocked(true);
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
            failure: function () {
                model.blocked(false);
            }
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
        model.blocked(true);
        $.ajax({
            type: "POST",
            url: '/api/xero/login',
            data: { code: self.code },
            success: function () {
                location.href = '/views/xeroSearch.html';
            },
            failure: function () {
                model.blocked(false);
            }
        });
    }
}


var model = {
    blocked: ko.observable(false),
    system: ko.observable(''),
    sage50: new Sage50LoginModel(),
    xero: new XeroLoginModel()
}

ko.applyBindings(model);