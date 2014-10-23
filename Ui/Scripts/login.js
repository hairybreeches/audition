var Sage50LoginModel = function() {
    var self = this;
    self.dataDirectory = ko.observable('');
    self.username = ko.observable('');
    self.password = ko.observable('');
    self.blocked = ko.observable(false);

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
        self.blocked(true);
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
                self.blocked(false);
            }
        });
    }
}


var XeroLoginModel = function () {
    var self = this;

    self.blocked = ko.observable(false);
    self.code = ko.observable('');


    self.initialiseLogin = function() {
        $.ajax({
            type: "POST",
            url: '/api/xero/initialiselogin'
        });
    }

    self.submit = function () {        
        self.blocked(true);
        $.ajax({
            type: "POST",
            url: '/api/xero/login',
            data: { code: self.code },
            success: function () {
                location.href = '/views/xeroSearch.html';
            },
            failure: function () {
                self.blocked(false);
            }
        });
    }
}


var model = {    
    system: ko.observable(''),
    sage50: new Sage50LoginModel(),
    xero: new XeroLoginModel()
}

ko.applyBindings(model);