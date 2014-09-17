Mousetrap.bind('ctrl+f5', function (e) {
    location.reload();
});


Mousetrap.bind('ctrl+f12', function (e) {
    $.get('/api/devtools');
});