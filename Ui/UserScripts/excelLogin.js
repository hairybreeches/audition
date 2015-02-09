var ExcelLoginModel = function () {
    var self = this;

    self.fileLocation = ko.observable('').extend({ rateLimit: { method: "notifyWhenChangesStop", timeout: 400 } });
    self.useHeaderRow = ko.observable(true);
    self.sheet = ko.observable("0");
    self.showInput = ko.observable(false);

    var fields = ['JournalDate', 'Description', 'Username', 'Created', 'AccountCode', 'AccountName', 'Amount', 'Id'];

    fields.forEach(
        function(fieldName) {
            self[fieldName] = ko.observable('A');
        });

    var getData = function() {
        var data = {
            SheetDescription: {
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

    self.sheets = ko.observableArray();

    self.columns = ko.computed(function() {
        
        var sheet = self.sheets()[parseInt(self.sheet())];
        if (sheet) {
            var lookup = self.useHeaderRow ? "ColumnHeaders" : "ColumnNames";
            return sheet[lookup];
        }
        return [];
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

            success: function(data) {
                self.sheets(data);
                self.sheet("0");
                self.showInput(true);
            },

            error: showError
        });
    }    

    self.disabled = function () {
        return !self.showInput();
    };

    var onNewFilename = function (newFilename) {
        if (newFilename) {
            updateSheets(newFilename);            
            return;
        }

        self.showInput(false);
    }

    self.fileLocation.subscribe(onNewFilename);   

    self.browseExcelFile = createBrowseFunction('/api/chooseExcelFile', self.fileLocation);

    self.submit = function() {
        model.login('/api/excel/login', getData());
    };   

    autocomplete('#excelFileLocation', '/api/userdata/excelDataFiles');
};