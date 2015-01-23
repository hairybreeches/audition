var ExcelLoginModel = function() {
    var self = this;

    self.fileLocation = ko.observable('');
    self.useHeaderRow = ko.observable(true);
    var fields = ['JournalDate', 'Description', 'Username', 'Created', 'AccountCode', 'AccountName', 'Amount'];
    fields.forEach(
        function(fieldName) {
            self[fieldName] = ko.observable('A');
        });

    var getData = function() {
        var data = {
            Filename: self.fileLocation(),
            UseHeaderRow: self.useHeaderRow()
        };

        data.Lookups = fields.reduce(function(lookups, fieldName) {
            lookups[fieldName] = self[fieldName]();
            return lookups;
        }, {});

        return data;
    }

    self.columnNames = ko.observableArray(['Enter an excel spreadsheet name above']);    

    var updateColumnNames = function(fileLocation, useHeaderRow) {
        $.ajax('/api/excel/getHeaders', {
            type: "POST",
            contentType: 'application/json',
            data: JSON.stringify({
                Filename: fileLocation,
                UseHeaderRow: useHeaderRow

            }),
            success: function(data) {
                self.columnNames(data);
            }
        });
    }

    self.fileLocation.subscribe(function (newFilename) {
        return updateColumnNames(newFilename, self.useHeaderRow());
    });

    self.useHeaderRow.subscribe(function (newUseHeaderRow) {
        return updateColumnNames(self.fileLocation(), newUseHeaderRow);
    });

    self.browseExcelFile = createBrowseFunction('/api/chooseExcelFile', self.fileLocation);

    self.submit = function() {
        model.login('/api/excel/login', getData());
    };

    autocomplete('#excelFileLocation', '/api/userdata/excelDataFiles');
};