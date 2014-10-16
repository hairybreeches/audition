var Sage50LoginModel = function() {
    var self = this;
    self.dataDirectory = ko.observable('');
    self.browseDataDirectory = function() {
        $.ajax('/api/chooseDirectory', {
            error: function() {

            },
            success: function(folderChosen) {
                self.dataDirectory(folderChosen);
            }
        });
    }
}



var model = {    
    system: ko.observable(''),
    sage50: new Sage50LoginModel(),
    xero: {}
}

ko.applyBindings(model);