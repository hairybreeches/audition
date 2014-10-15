
var Model = function () {
    var self = this;

    self.blocked = ko.observable(false);
    self.code = ko.observable('');    
    
    self.submit = function() {
        self.blocked(true);
        $.ajax({
            type: "POST",
            url: '/api/xero/completelogin',
            data: { code: self.code },
            success: function() {
                location.href = '/views/xeroSearch.html';
            },
            failure: function () {
                self.blocked(false);
            }
        });       
    }
}

ko.applyBindings(new Model());