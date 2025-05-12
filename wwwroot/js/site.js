document.addEventListener("DOMContentLoaded", function () {
    const checkbox = document.getElementById("useTodayCheckbox");
    const dateInput = document.getElementById("expenseDateInput");

    if (checkbox && dateInput) {
        checkbox.addEventListener("change", function () {
            if (this.checked) {
                dateInput.value = new Date().toISOString().split('T')[0];
                dateInput.readOnly = true;
            }
            else {
                dateInput.readOnly = false;
            }
        });
    }
});
