var Sage50ImportModel = function() {
    var self = this;
    self.dataDirectory = ko.observable('');
    self.username = ko.observable('');
    self.password = ko.observable('');
    self.includeArchived = ko.observable(false);
    self.browseDataDirectory = createBrowseFunction('/api/chooseDirectory', self.dataDirectory);

    self.submit = function() {
        model.import('/api/sage50/import', {
            username: self.username(),
            password: self.password(),
            dataDirectory: self.dataDirectory(),
            includeArchived: self.includeArchived()
    });
    }

    self.disabled = function() {
        return false;
    };
    

    autocomplete("#sage50dataDirectory", "/api/userdata/sage50DataLocations");
};