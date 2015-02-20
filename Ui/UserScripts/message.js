var Message = function () {
    var self = this;
    //fields
    self.visible = ko.observable(false);

    //methods
    self.hide = function () {
        self.visible(false);
    };

    self.show = function () {
        self.visible(true);
    }
};

var ErrorMessage = function() {
    var self = this;
    var message = new Message();

    //fields
    self.visible = message.visible;
    self.message = ko.observable('');

    //methods
    self.hide = message.hide;

    self.show = function (jqXHR) {
        var errorMessage = getErrorMessage(jqXHR);
        self.message(errorMessage);
        message.visible(true);
    }
}