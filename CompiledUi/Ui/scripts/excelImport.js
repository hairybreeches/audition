var ExcelImportModel = function () {
    var self = this;

    self.fileLocation = ko.observable('').extend({ rateLimit: { method: "notifyWhenChangesStop", timeout: 400 } });
    self.useHeaderRow = ko.observable(true);
    self.sheet = ko.observable("0");
    self.showInput = ko.observable(false);

    var fields = ['TransactionDate', 'Description', 'Username', 'NominalCode', 'NominalName', 'Amount', 'Id', 'Type'];

    fields.forEach(
        function (fieldName) {
            self[fieldName] = ko.observable('-1');
        });
    
    var getData = function () {
        var data = {
            SheetDescription: {
                Filename: self.fileLocation(),
                UseHeaderRow: self.useHeaderRow(),
                Sheet: self.sheet()
            }
        };

        data.Lookups = fields.reduce(function (lookups, fieldName) {
            lookups[fieldName] = self[fieldName]();
            return lookups;
        }, {});

        return data;
    }

    var sheets = ko.observableArray();

    var toSelectOptions = function (data) {
        return data.map(function (label, index) {
            return {
                label: label,
                index: index
            }
        });
    };

    self.sheetOptions = ko.computed(function() {
        return toSelectOptions(sheets().map(function(sheet) {
            return sheet.Name;
        }));
    });

    var getColumnNames = function() {
        var sheet = sheets()[parseInt(self.sheet())];
        if (sheet) {
            var lookup = self.useHeaderRow() ? "ColumnHeaders" : "ColumnNames";
            return sheet[lookup];
        }
        return [];
    }

    var getColumns = function() {
        return toSelectOptions(getColumnNames());
    };

    var columns = ko.computed(getColumns);

    self.optionalColumns = ko.computed(function() {
        return [{ label: '*** No value ***', index: -1 }].concat(getColumns());
    });

    self.requiredColumns = ko.computed(function () {
        return [{ label: 'Choose...', index: -1 }].concat(getColumns());
    });

    self.errorMessage = {
        visible: ko.observable(false),
        message: ko.observable('')
    }

    var showError = function (jqxi) {
        self.showInput(false);
        self.errorMessage.message(getErrorMessage(jqxi));
        self.errorMessage.visible(true);
    }

    var updateSheets = function (fileLocation) {
        $.ajax('/api/excel/getSheets', {
            type: "GET",
            data: {
                filename: fileLocation
            },

            success: function (data) {
                sheets(data);
                self.errorMessage.visible(false);
                self.sheet("0");
                self.showInput(true);
            },

            error: showError
        });
    }

    self.disabled = function () {
        return !(self.showInput() && columns().some(function () { return true; }));
    };

    var onNewFilename = function (newFilename) {
        if (newFilename) {
            updateSheets(newFilename);
            return;
        }

        self.showInput(false);
        self.errorMessage.visible(false);
    }

    self.fileLocation.subscribe(onNewFilename);

    self.browseExcelFile = createBrowseFunction('/api/chooseExcelFile', self.fileLocation);

    self.submit = function () {
        model.import('/api/excel/import', getData());
    };

    autocomplete('#excelFileLocation', '/api/userdata/excelDataFiles');
};