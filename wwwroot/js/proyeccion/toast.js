function mostrarToast(mensaje) {
    $("#toastGuardado .toast-body").text(mensaje);
    let toast = new bootstrap.Toast(document.getElementById('toastGuardado'));
    toast.show();
}