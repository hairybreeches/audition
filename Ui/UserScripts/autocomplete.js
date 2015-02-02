var autocomplete = function(selector, url) {
    $(selector).typeahead({
        source: function (request, response) {
            $.ajax({
                url: url,
                data: {
                    enteredData: request.term
                },
                success: function (data) {
                    response(data);
                }
            });
        },
        minLength: 0
    });
}