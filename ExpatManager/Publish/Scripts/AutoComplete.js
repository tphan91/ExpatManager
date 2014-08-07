$(document).ready(function () {
    $('#autocomplete').each(function () {
        $(this).autocomplete({ source: $(this).attr('data-autocomplete') });
        $(this).autocompleteid
    });
});