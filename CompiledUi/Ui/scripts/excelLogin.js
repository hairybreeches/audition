var ErrorMessageNamesList = function() {
    var self = this;
    self.names = ko.observableArray([]);
    self.errorMessage = ko.observable('');
    self.showError = ko.observable(false);
    self.showInput = ko.observable(false);

    self.update = function (data) {
        self.names(data);
        self.showError(false);
        self.showInput(true);
        self.errorMessage(false);
    };

    self.hideAll = function () {
        self.showError(false);
        self.showInput(false);
    }

    self.updateError = function (message) {
        self.errorMessage(getErrorMessage(message));
        self.showError(true);
        self.showInput(false);
    }
}

var ExcelLoginModel = function () {
    var self = this;

    self.fileLocation = ko.observable('');
    self.useHeaderRow = ko.observable(true);
    self.sheet = ko.observable("");
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

    var updateColumnNames = function (fileLocation, useHeaderRow, sheet) {
        $.ajax('/api/excel/getHeaders', {
            type: "POST",
            contentType: 'application/json',
            data: JSON.stringify({
                Filename: fileLocation,
                UseHeaderRow: useHeaderRow,
                Sheet: sheet
            }),
            success: self.columns.update,
            error: self.columns.updateError
        });
    };

    self.sheets = new ErrorMessageNamesList();

    var updateSheetNames = function (fileLocation) {
        $.ajax('/api/excel/getSheetNames', {
            type: "GET",            
            data: {
                filename: fileLocation
            },
            success: self.sheets.update,
            error: self.sheets.updateError
        });
    }

    var onNewFilename = function (newFilename) {
        if (newFilename) {
            updateSheetNames(newFilename);
            updateColumnNames(newFilename, self.useHeaderRow(), self.sheet());            
            return;
        }

        self.sheets.hideAll();
        self.columns.hideAll();
    }

    self.fileLocation.subscribe(onNewFilename);

    self.useHeaderRow.subscribe(function (newUseHeaderRow) {
        updateColumnNames(self.fileLocation(), newUseHeaderRow, self.sheet());
    });

    self.sheet.subscribe(function (newSheet) {
        updateColumnNames(self.fileLocation(), self.useHeaderRow(), newSheet);
    });

    self.browseExcelFile = createBrowseFunction('/api/chooseExcelFile', self.fileLocation);

    self.submit = function() {
        model.login('/api/excel/login', getData());
    };

    self.errorMessage = {
        visible: function() {
            return self.sheets.showError() || self.columns.showError();
        },

        message: function() {
            return self.sheets.errorMessage() || self.columns.errorMessage();
        }     
    }

    self.disabled = function () {
        return !(self.sheets.showInput() && self.columns.showInput());
    };

    autocomplete('#excelFileLocation', '/api/userdata/excelDataFiles');
};