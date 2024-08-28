const togglePassword = document.querySelector('#togglePassword');
const password = document.querySelector('#password');

togglePassword.addEventListener('click', function () {
    const type = password.getAttribute('type') === 'password' ? 'text' : 'password';
    password.setAttribute('type', type);
    this.querySelector('i').classList.toggle('fa-eye-slash');
});

let codeSent = false;
let sendCodeButton = document.querySelector("#send-code");
const saveChangesButton = document.querySelector("#save-changes");
const codeInput = document.querySelector("#code-input");
const emailInput = document.querySelector("#email");
const passwordInput = document.querySelector("#password");
const firstNameInput = document.querySelector("#firstName");
const lastNameInput = document.querySelector("#lastName");


sendCodeButton.addEventListener('click', (e) => {
    if (!codeSent) {
        let removeAccountButton = document.querySelector("#remove-account");
        let validation = document.querySelector("#validation");
        
        codeSent = true;
        saveChangesButton.classList.remove("d-none");
        sendCodeButton.classList.add("d-none");
        removeAccountButton.classList.add("d-none");
        codeInput.parentElement.classList.remove("d-none");
        codeInput.value = "";
        validation.classList.add("d-none");
        
        emailInput.setAttribute("readonly", "");
        passwordInput.setAttribute("readonly", "");
        firstNameInput.setAttribute("readonly", "");
        lastNameInput.setAttribute("readonly", "");

        // Выполнение AJAX-запроса на сервер при нажатии кнопки
        fetch(`/user/get-code/${emailInput.value}`, {
            method: 'GET'
        })
            .then(response => {
                if (!response.ok) {
                    throw new Error('Ошибка при отправке кода'); // Обработка ошибок
                }
                return response.json(); // Обработка ответа от сервера, если он в формате JSON
            })
            .then(data => {
                // Обработка данных, если сервер возвращает что-то
                console.log('Успешно отправлен код');
            })
            .catch(error => {
                console.error('Ошибка:', error); // Обработка ошибок
            });
    }
})