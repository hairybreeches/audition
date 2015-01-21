var ExcelLoginModel = function() {
    var self = this;

    self.fileLocation = ko.observable('');
    var fields = ['JournalDate', 'Description', 'Username', 'Created', 'AccountCode', 'AccountName', 'Amount', 'JournalType'];
    fields.forEach(
        function(fieldName) {
            self[fieldName] = ko.observable('A');
        });

    var getData = function() {
        var data = {
            Filename: self.fileLocation()
        };

        data.Lookups = fields.reduce(function(lookups, fieldName) {
            lookups[fieldName] = self[fieldName]();
            return lookups;
        }, {});

        return data;
    }

    self.browseExcelFile = createBrowseFunction('/api/chooseExcelFile', self.fileLocation);

    self.submit = function() {
        model.login('/api/excel/login', getData());
    };
};