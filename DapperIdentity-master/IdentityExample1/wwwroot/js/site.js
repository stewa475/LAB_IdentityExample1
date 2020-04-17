// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
let today = new Date().toISOString().substr(0, 10);
document.querySelector("#today").value = today;

$("input[type=checkbox]").change(function () {
    if (this.checked) {
        $(this).parent().next().css('text-decoration', 'line-through');
    } else {
        $(this).parent().next().css('text-decoration', 'none');
    }
});