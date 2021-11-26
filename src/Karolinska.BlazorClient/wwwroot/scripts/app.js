var blazorApplication = {};

blazorApplication.setSessionStorage = function (key, data) {
    sessionStorage.setItem(key, data);
}

blazorApplication.getSessionStorage = function (key) {
    return sessionStorage.getItem(key);
}

//blazorApplication.hideModal = function (element) {
//    bootstrap.Modal.getInstance(element).hide();
//}