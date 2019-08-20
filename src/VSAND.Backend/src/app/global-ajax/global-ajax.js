
$.ajaxPrefilter(function (options, originalOptions, jqXHR) {
    jqXHR.always(function () {
        var sJsonStatus = jqXHR.getResponseHeader("X-Responded-JSON");
        if (sJsonStatus !== undefined && sJsonStatus !== null && sJsonStatus !== "") {
            var oJsonStatus = null;
            try {
                oJsonStatus = JSON.parse(sJsonStatus);
            } catch (e) {
                oJsonStatus = null;
                // don't do anything, but don't have an error
            }

            if (oJsonStatus === undefined || oJsonStatus === null) return;

            switch (oJsonStatus.status) {
                case 401:
                    // they are not authenticated
                    window.location = '/';
                    break;

                default:
                    break;
            }
        }
    });
});

// ajax progress spinner
var $loading = $('#loadingSpinner');
$(document)
    .ajaxStart(function () {
        $loading.show();
    })
    .ajaxStop(function () {
        $loading.hide();
    });