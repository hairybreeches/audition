﻿var ErrorMessageNamesList = function() {
        var self = this;
        self.names = ko.observableArray([]);
        self.errorMessage = ko.observable('');
        self.showError = ko.observable(false);

        self.update = function(data) {
            self.names(data);
            self.showError(false);
        };

        self.updateError = function(message) {
            self.errorMessage(message);
            self.showError(true);
        }
}

var ExcelLoginModel = function () {
    var self = this;

    self.fileLocation = ko.observable('');
    self.useHeaderRow = ko.observable(true);
    self.sheet = ko.observable(0);
    var fields = ['JournalDate', 'Description', 'Username', 'Created', 'AccountCode', 'AccountName', 'Amount', 'Id'];
    fields.forEach(
        function(fieldName) {
            self[fieldName] = ko.observable('A');
        });

    var getData = function() {
        var data = {
            SheetData: {
                Filename: self.fileLocation(),
                UseHeaderRow: self.useHeaderRow(),
                Sheet: self.sheet()
            }
        };

        data.Lookups = fields.reduce(function(lookups, fieldName) {
            lookups[fieldName] = self[fieldName]();
            return lookups;
        }, {});

        return data;
    }

    self.columns = new ErrorMessageNamesList();

    var updateColumnNames = function(fileLocation, useHeaderRow, sheet) {
        $.ajax('/api/excel/getHeaders', {
            type: "POST",
            contentType: 'application/json',
            data: JSON.stringify({
                Filename: fileLocation,
                UseHeaderRow: useHeaderRow,
                Sheet: sheet
            }),
            success: self.columns.update
        });
    };

    self.sheets = new ErrorMessageNamesList();

    var updateSheetNames = function(fileLocation) {
        $.ajax('/api/excel/getSheetNames', {
            type: "GET",            
            data: {
                filename: fileLocation
            },
            success: self.sheets.update            
        });
    }

    self.fileLocation.subscribe(function (newFilename) {
        return updateColumnNames(newFilename, self.useHeaderRow(), self.sheet());
    });

    self.useHeaderRow.subscribe(function (newUseHeaderRow) {
        return updateColumnNames(self.fileLocation(), newUseHeaderRow, self.sheet());
    });

    self.sheet.subscribe(function (newSheet) {
        return updateColumnNames(self.fileLocation(), self.useHeaderRow(), newSheet);
    });

    self.fileLocation.subscribe(updateSheetNames);

    self.browseExcelFile = createBrowseFunction('/api/chooseExcelFile', self.fileLocation);

    self.submit = function() {
        model.login('/api/excel/login', getData());
    };

    autocomplete('#excelFileLocation', '/api/userdata/excelDataFiles');
};