$(document).ready(function () {
    // Establish a connection to the SignalR hub
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/chat")
        .build();

    function startConnection() {
        connection.start().then(function () {
            console.log("Connected to Chat Hub");
            fetchActiveUsers(); // Fetch active users after connection is established
            fetchContactUsers(); // Fetch contact users after connection is established
        }).catch(function (err) {
            console.error("Error while establishing connection: ", err.toString());
            setTimeout(startConnection, 5000);
        });
    }

    startConnection();

    function sendData(methodName, ...args) {
        if (connection.state === signalR.HubConnectionState.Connected) {
            connection.invoke(methodName, ...args)
                .catch(err => console.error(`Error while invoking ${methodName}:`, err.toString()));
        } else {
            console.error("SignalR connection is not in the 'Connected' state.");
        }
    }

    connection.on("ReceiveMessage", function (user, message) {
        const chatBox = $("#chat-box");
        chatBox.append(`<div><strong>${user}:</strong> ${message}</div>`);
    });

    connection.on("ReceiveVoiceRecording", function (user, fileName) {
        const chatBox = $("#chat-box");
        chatBox.append(`<div><strong>${user}:</strong> <audio controls src="/path/to/voice/${fileName}"></audio></div>`);
    });

    connection.on("ReceiveFileMessage", function (user, fileName, contentType) {
        const chatBox = $("#chat-box");
        chatBox.append(`<div><strong>${user}:</strong> <a href="/path/to/files/${fileName}" download>${fileName}</a></div>`);
    });

    function fetchContactUsers() {
        const userId = $('#SenderName').val(); // Assuming user ID is stored in hidden field

        connection.invoke("GetUniqueContactNames", userId)
            .then(function (result) {
                const contactUserListElement = $("#contactUserList");
                contactUserListElement.empty();
                result.entities.forEach(function (userName) {
                    contactUserListElement.append(`<li><a href="#" class="contact-link" data-user="${userName}">${userName}</a></li>`);
                });
            })
            .catch(function (err) {
                console.error("Error fetching contact users:", err.toString());
            });
    }

    function loadMessages(contactUserName) {
        const userName = $('#SenderName').val();

        connection.invoke("GetMessages", userName, contactUserName)
            .then(function (result) {
                const chatBox = $("#chat-box");
                chatBox.empty();
                result.entities.forEach(function (item) {
                    if (item.isMessage) {
                        chatBox.append(`<div><strong>${item.senderId}:</strong> ${item.content}</div>`);
                    } else {
                        chatBox.append(`<div><strong>${item.senderId}:</strong> <a href="/path/to/files/${item.fileName}" download>${item.fileName}</a></div>`);
                    }
                });
            })
            .catch(function (err) {
                console.error("Error loading messages:", err.toString());
            });
    }

    $(document).on('click', '.contact-link', function (event) {
        event.preventDefault();
        const contactUserName = $(this).data('user');
        console.log('Contact link clicked:', contactUserName);
        loadMessages(contactUserName);
    });

    function fetchActiveUsers() {
        connection.invoke("NotifyUsersUpdate")
            .then(function (result) {
                const activeUserListElement = $("#activeUserList");
                activeUserListElement.empty();
                result.entities.forEach(function (userName) {
                    activeUserListElement.append(`<li>${userName}</li>`);
                });
            })
            .catch(function (err) {
                console.error("Error fetching active users:", err.toString());
            });
    }

    function handleSendData(context) {
        const selectedUsers = [];
        $('#userSelectionList input[type="checkbox"]:checked').each(function () {
            selectedUsers.push($(this).val());
        });

        const senderName = $('#SenderName').val();

        if (selectedUsers.length > 0) {
            if (context === 'message') {
                const message = $('#messageInput').val();
                if (message) {
                    selectedUsers.forEach(function (receiverId) {
                        sendData("SendMessage", receiverId, senderName, message);
                    });
                    $('#messageInput').val('');
                } else {
                    alert('Please enter a message.');
                }
            } else if (context === 'file') {
                const fileInput = $('#fileInput').get(0);
                if (fileInput.files.length > 0) {
                    const file = fileInput.files[0];
                    if (file.size > 80 * 1024 * 1024) {
                        alert('File size exceeds 80 MB. Please select a smaller file.');
                        return;
                    }
                    const reader = new FileReader();
                    reader.onload = function (e) {
                        const uint8Array = new Uint8Array(e.target.result);
                        const chunkSize = 0x8000;
                        let base64String = '';
                        for (let i = 0; i < uint8Array.length; i += chunkSize) {
                            base64String += String.fromCharCode.apply(null, uint8Array.subarray(i, i + chunkSize));
                        }
                        const fileContent = btoa(base64String);
                        const fileName = file.name;
                        const contentType = file.type;

                        selectedUsers.forEach(function (receiverId) {
                            sendData("SendFile", receiverId, senderName, fileName, fileContent, contentType);
                        });
                        $('#fileInput').val('');
                    };
                    reader.readAsArrayBuffer(file);
                } else {
                    alert('Please select a file to send.');
                }
            } else if (context === 'voice') {
                if (audioChunks.length > 0) {
                    const audioBlob = new Blob(audioChunks, { type: 'audio/wav' });
                    const reader = new FileReader();
                    reader.onload = function (e) {
                        const fileContent = btoa(String.fromCharCode.apply(null, new Uint8Array(e.target.result)));
                        const fileName = 'voiceRecording.wav';

                        selectedUsers.forEach(function (receiverId) {
                            sendData("SendVoiceRecording", receiverId, senderName, fileName, fileContent);
                        });

                        audioChunks = [];
                    };
                    reader.readAsArrayBuffer(audioBlob);
                } else {
                    alert('Please record a voice message.');
                }
            }
            $('#userSelectionModal').css('display', 'none');
        } else {
            alert('Please select at least one user.');
        }
    }

    let audioChunks = [];
    let mediaRecorder;

    $('#startRecordingButton').click(function () {
        console.log('Start recording button clicked');
        navigator.mediaDevices.getUserMedia({ audio: true }).then(function (stream) {
            mediaRecorder = new MediaRecorder(stream);
            mediaRecorder.ondataavailable = function (e) {
                audioChunks.push(e.data);
            };
            mediaRecorder.onstop = function () {
                $('#recordedAudio').attr('src', URL.createObjectURL(new Blob(audioChunks, { type: 'audio/wav' })));
                $('#openUserSelectionModalButtonVoice').prop('disabled', false);
            };
            mediaRecorder.start();
            $('#startRecordingButton').prop('disabled', true);
            $('#stopRecordingButton').prop('disabled', false);
        }).catch(function (err) {
            console.error("Error accessing microphone: ", err.toString());
        });
    });

    $('#stopRecordingButton').click(function () {
        console.log('Stop recording button clicked');
        mediaRecorder.stop();
        $('#startRecordingButton').prop('disabled', false);
        $('#stopRecordingButton').prop('disabled', true);
    });

    $('.close-button').click(function () {
        console.log('Close button clicked');
        $('#userSelectionModal').css('display', 'none');
    });

    $('#openUserSelectionModalButtonMessage, #openUserSelectionModalButtonFile, #openUserSelectionModalButtonVoice').click(function () {
        const context = $(this).data('context');
        console.log('Open user selection modal button clicked with context:', context);
        $('#sendDataButton').data('context', context);

        connection.invoke("GetUserList")
            .then(function (users) {
                const userSelectionListElement = $("#userSelectionList");
                userSelectionListElement.empty();
                users.entities.forEach(function (user) {
                    userSelectionListElement.append(`<li><label><input type="checkbox" value="${user.userId}">${user.userName}</label></li>`);
                });
                $('#userSelectionModal').css('display', 'block');
            })
            .catch(function (err) {
                console.error("Error fetching user list:", err.toString());
            });
    });

    $('#sendDataButton').click(function () {
        console.log('Send data button clicked');
        const context = $(this).data('context');
        handleSendData(context);
    });
});
