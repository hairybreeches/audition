var LicenceModel = function() {

    var self = this;

    self.licence = ko.mapping.fromJS({
        IsFullyLicensed: true,
        TrialValid: true,
        RemainingTrialDays: 28
    });

    self.showLicensingInfo = ko.computed(function() {
        return !self.licence.IsFullyLicensed();
    });

    self.licenceText = ko.computed(function () {

        if (self.licence.IsFullyLicensed()) {
            return "";
        }

        if (self.licence.TrialValid()) {
            return self.licence.RemainingTrialDays() + " days of trial remaining";
        }

        return "Unlicensed";
    });
}

var licenceModel = new LicenceModel();

ko.applyBindings(licenceModel, document.getElementById('licensingElement'));

var updateLicence = function () {

    $.ajax("/api/licence/get", {
        success: function (newLicence) {
            ko.mapping.fromJS(newLicence, licenceModel.licence);
        },
        type: 'GET'
    });
}

updateLicence();

