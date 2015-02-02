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

    autocomplete("#sage50dataDirectory", "/api/userdata/sage50DataLocations");
};