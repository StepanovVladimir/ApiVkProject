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
        dataType: 'JSON',
        complete: function () {
            $('#message_area').val('');
        },
        success: function (data) {
            if ($(event.target).data('recipient') == 'User') {
                successSendMessageToUser(data);
            } else {
                successSendMessageToGroup(data);
            }
        },
        error: function () {
            alert('Что-то пошло не так');
        }
    });
}

function successSendMessageToUser(data) {
    alert(data);
}

function successSendMessageToGroup(data) {
    if (data == '') {
        alert('Не удалось отправить сообщение никому');
    } else {
        alert('Сообщение отправленно следующим пользователям:\n' + data);
    }
}