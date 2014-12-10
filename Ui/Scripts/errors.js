var getErrorMessage = function (jqXHR) {

    var getServerErrorMessage = function(response) {
        return response.ExceptionMessage || response.Message;
    }

    return jqXHR.responseJSON ?  getServerErrorMessage(jqXHR.responseJSON): jqXHR.responseText;
}


