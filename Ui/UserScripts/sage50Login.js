var Sage50ImportModel = function() {
    var self = this;
    self.dataDirectory = ko.observable('');
    self.username = ko.observable('');
    self.password = ko.observable('');

    self.browseDataDirectory = createBrowseFunction('/api/chooseDirectory', self.dataDirectory);

    self.submit = function() {
        model.import('/api/sage50/login', {
            username: self.username(),
            password: self.password(),
            dataDirectory: self.dataDirectory(),
        });
    }

    self.disabled = function() {
        return false;
    };

    autocomplete("#sage50dataDirectory", "/api/userdata/sage50DataLocations");
};