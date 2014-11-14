var getErrorMessage = function (jqXHR) {
    return jqXHR.responseJSON ? jqXHR.responseJSON.ExceptionMessage : jqXHR.responseText;
}
