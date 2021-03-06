﻿var EnterLicenceModel = function () {
    var self = this;
    self.licenceKey = ko.observable('');


    self.goBack = function() {
        window.history.back();
    }

    self.errorMessage = new ErrorMessage();
    self.successMessage = new Message();
    self.submit = function() {
        $.ajax("/api/licence/update", {
            type: 'GET',
            success: function () {
                self.errorMessage.hide();
                updateLicence();
                self.successMessage.show();
            },

            error: self.errorMessage.show,
            data: {
                licenceKey: self.licenceKey()
            }
        });
    }
}
var model = new EnterLicenceModel();
ko.applyBindings(model, document.getElementById('pageElement'));