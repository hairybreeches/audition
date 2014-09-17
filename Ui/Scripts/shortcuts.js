Mousetrap.bind('ctrl+f5', function (e) {
    location.reload(true);
});


Mousetrap.bind('ctrl+f12', function (e) {
    $.get('/api/devtools');
});