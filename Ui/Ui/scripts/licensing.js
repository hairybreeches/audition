var licenceModel = ko.mapping.fromJS({
    IsFullyLicensed: true
});

licenceModel.showUnlicensed = ko.computed(function() {
    return ! licenceModel.IsFullyLicensed();
})

ko.applyBindings(licenceModel, document.getElementById('licensingElement'));

var updateLicence = function (newLicence) {
    ko.mapping.fromJS(newLicence, licenceModel);
}




$.ajax("/api/licence/get", {
    success: updateLicence,
    type: 'GET'
});