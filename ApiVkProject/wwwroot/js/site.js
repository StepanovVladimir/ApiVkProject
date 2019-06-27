$(window).on('load', onWindowLoaded);

function onWindowLoaded() {
    $('#send_message').on('click', sendMessage);
}

function sendMessage(event) {
    var id = +$(event.target).data('id');
    var message = $('#message_area').val();

    if (!message) {
        alert('Вы не ввели сообщение');
        return;
    }

    $.ajax({
        url: '/ApiVk/SendMessageTo' + $(event.target).data('recipient'),
        data: { id: id, message: message },
        method: "POST",
        success: function () {
            alert('Сообщение отправленно');
        },
        error: function () {
            alert('Что-то пошло не так');
        }
    });
}