var licenceModel = ko.mapping.fromJS({
    IsFullyLicensed: true,
    TrialValid: true,
    RemainingTrialDays: 28    
});

licenceModel.licenceText = ko.computed(function () {

    if (licenceModel.IsFullyLicensed()) {
        return "";
    }

    if (licenceModel.TrialValid()) {
        return licenceModel.RemainingTrialDays() + " days of trial remaining";
    }

    return "Unlicensed";
});

ko.applyBindings(licenceModel, document.getElementById('licensingElement'));

var updateLicence = function (newLicence) {
    ko.mapping.fromJS(newLicence, licenceModel);
}

$.ajax("/api/licence/get", {
    success: updateLicence,
    type: 'GET'
});