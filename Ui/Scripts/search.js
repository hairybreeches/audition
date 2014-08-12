var model = ko.mapping.fromJS({
    Period: {
        From: "5/4/2013",
        To: "4/4/2013"
    },

    TimeFrame: {
        FromDay: "Monday",
        ToDay: "Friday",
        FromTime: "08:00",
        ToTime: "18:00"
    }
});
ko.applyBindings(model);