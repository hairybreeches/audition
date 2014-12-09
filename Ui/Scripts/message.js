var Message = function () {
    var self = this;
    //fields
    self.visible = ko.observable(false);
    self.message = ko.observable('');

    //methods
    self.hide = function () {
        self.visible(false);
    };

    self.show = function (jqXHR) {
        var errorMessage = getErrorMessage(jqXHR);
        self.message(errorMessage);
        self.visible(true);
    }
};