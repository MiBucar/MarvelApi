$(document).ready(function () {
    $('form').on('submit', function (e) {
        e.preventDefault();

        var form = $(this);
        var url = form.attr('action');

        $.ajax({
            type: "POST",
            url: url,
            data: form.serialize(),
            success: function (response) {
                if (response.success) {
                    $('#contact-modal').modal('hide');
                    form[0].reset();
                } else {
                    alert("Something went wrong. Please try again.");
                }
            },
            error: function () {
                alert("An error occurred. Please try again.");
            }
        });
    });
});