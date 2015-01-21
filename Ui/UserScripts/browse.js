var createBrowseFunction = function(url, property) {
    return function() {
        $.ajax(url, {
            data: {
                start: property()
            },
            error: function() {

            },
            success: function(chosen) {
                property(chosen);
            }
        });
    }
}