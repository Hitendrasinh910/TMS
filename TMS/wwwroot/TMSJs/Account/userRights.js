// ======================================================
// CONFIGURATION & STATE
// ======================================================
const API_RIGHTS = "/api/auth/user-rights";
const API_USERS = "/api/master/user"; // Assuming you have a standard user master API

let currentRightsData = [];

// ======================================================
// DOM ELEMENTS CACHE
// ======================================================
const DOM = {
    userDropdown: function () { return document.getElementById("ddlUser"); },
    gridBody: function () { return document.getElementById("rightsGridBody"); },
    btnSave: function () { return document.getElementById("btnSaveRights"); },

    // Header "Select All" Checkboxes
    chkAllView: function () { return document.getElementById("chkAllView"); },
    chkAllAdd: function () { return document.getElementById("chkAllAdd"); },
    chkAllUpdate: function () { return document.getElementById("chkAllUpdate"); },
    chkAllDelete: function () { return document.getElementById("chkAllDelete"); }
};

// ======================================================
// INITIALIZATION
// ======================================================
document.addEventListener("DOMContentLoaded", async function () {

    // 1. Load User Dropdown
    await loadUserAccounts();

    // 2. Bind Event Listeners
    DOM.userDropdown().addEventListener("change", handleUserSelection);
    DOM.btnSave().addEventListener("click", saveUserRights);

    // 3. Bind "Select All" Headers
    bindSelectAllLogic(DOM.chkAllView(), "chk-view");
    bindSelectAllLogic(DOM.chkAllAdd(), "chk-add");
    bindSelectAllLogic(DOM.chkAllUpdate(), "chk-update");
    bindSelectAllLogic(DOM.chkAllDelete(), "chk-delete");
});

// ======================================================
// FETCH USERS
// ======================================================
async function loadUserAccounts() {
    try {
        const response = await apiFetch(`${API_USERS}/get-all`);
        const jsonResponse = await response.json();

        if (jsonResponse.success === true) {
            let optionsHtml = '<option value="">-- Please Select a User --</option>';

            for (let i = 0; i < jsonResponse.data.length; i++) {
                const user = jsonResponse.data[i];
                // Assuming your User table has idUser and fullName
                optionsHtml += `<option value="${user.idUser}">${escapeHtml(user.fullName)} (${escapeHtml(user.userName)})</option>`;
            }

            DOM.userDropdown().innerHTML = optionsHtml;
        }
    }
    catch (err) {
        console.error("Failed to load users for dropdown", err);
        showToast("danger", "Failed to load user list.", "System Error");
    }
}

// ======================================================
// FETCH RIGHTS ON USER SELECT
// ======================================================
async function handleUserSelection(event) {
    const selectedUserId = event.target.value;

    // Reset headers
    DOM.chkAllView().checked = false;
    DOM.chkAllAdd().checked = false;
    DOM.chkAllUpdate().checked = false;
    DOM.chkAllDelete().checked = false;

    if (selectedUserId === "") {
        DOM.gridBody().innerHTML = '<tr><td colspan="5" class="text-center text-muted py-4">Please select a user to load their permissions.</td></tr>';
        DOM.btnSave().disabled = true;
        currentRightsData = [];
        return;
    }

    try {
        DOM.gridBody().innerHTML = '<tr><td colspan="5" class="text-center py-4"><i class="fa fa-spinner fa-spin"></i> Loading permissions...</td></tr>';

        const response = await apiFetch(`${API_RIGHTS}/get-by-user/${selectedUserId}`);
        const jsonResponse = await response.json();

        if (jsonResponse.success === true) {
            currentRightsData = jsonResponse.data;
            renderRightsGrid();
            DOM.btnSave().disabled = false;
        }
        else {
            throw new Error(jsonResponse.message);
        }
    }
    catch (err) {
        console.error("Error loading user rights", err);
        DOM.gridBody().innerHTML = '<tr><td colspan="5" class="text-center text-danger py-4">Failed to load permissions.</td></tr>';
        DOM.btnSave().disabled = true;
    }
}

// ======================================================
// RENDER GRID
// ======================================================
function renderRightsGrid() {
    let rowsHtml = "";

    if (currentRightsData.length === 0) {
        DOM.gridBody().innerHTML = '<tr><td colspan="5" class="text-center py-4">No forms available in the system.</td></tr>';
        return;
    }

    for (let i = 0; i < currentRightsData.length; i++) {
        const right = currentRightsData[i];

        // We embed the IDForms into a data attribute on each checkbox so we can extract it easily later
        const viewChecked = right.allowToView ? "checked" : "";
        const addChecked = right.allowToAdd ? "checked" : "";
        const updateChecked = right.allowToUpdate ? "checked" : "";
        const deleteChecked = right.allowToDelete ? "checked" : "";

        rowsHtml += `
            <tr class="align-middle">
                <td class="fw-bold ps-3">${escapeHtml(right.formName)}</td>
                <td class="text-center">
                    <input type="checkbox" class="form-check-input chk-view fs-5" data-form-id="${right.idForms}" ${viewChecked}>
                </td>
                <td class="text-center">
                    <input type="checkbox" class="form-check-input chk-add fs-5" data-form-id="${right.idForms}" ${addChecked}>
                </td>
                <td class="text-center">
                    <input type="checkbox" class="form-check-input chk-update fs-5" data-form-id="${right.idForms}" ${updateChecked}>
                </td>
                <td class="text-center">
                    <input type="checkbox" class="form-check-input chk-delete fs-5" data-form-id="${right.idForms}" ${deleteChecked}>
                </td>
            </tr>
        `;
    }

    DOM.gridBody().innerHTML = rowsHtml;
}

// ======================================================
// SELECT ALL LOGIC
// ======================================================
function bindSelectAllLogic(headerCheckboxElement, targetClassName) {
    headerCheckboxElement.addEventListener("change", function (event) {
        const isChecked = event.target.checked;
        const targetCheckboxes = document.querySelectorAll(`.${targetClassName}`);

        for (let i = 0; i < targetCheckboxes.length; i++) {
            targetCheckboxes[i].checked = isChecked;
        }
    });
}

// ======================================================
// SAVE TRANSACTION
// ======================================================
async function saveUserRights() {
    const selectedUserId = DOM.userDropdown().value;

    if (selectedUserId === "") {
        showToast("warning", "Please select a user first.", "Validation");
        return;
    }

    // 1. Gather data from the DOM by iterating through table rows
    const rightsPayloadArray = [];
    const tableRows = DOM.gridBody().querySelectorAll("tr");

    for (let i = 0; i < tableRows.length; i++) {
        const row = tableRows[i];

        // Find checkboxes within this specific row
        const viewChk = row.querySelector(".chk-view");
        const addChk = row.querySelector(".chk-add");
        const updateChk = row.querySelector(".chk-update");
        const deleteChk = row.querySelector(".chk-delete");

        if (viewChk) {
            // Extract the Form ID from the data attribute we set during rendering
            const formId = parseInt(viewChk.getAttribute("data-form-id"));

            rightsPayloadArray.push({
                idForms: formId,
                allowToView: viewChk.checked,
                allowToAdd: addChk.checked,
                allowToUpdate: updateChk.checked,
                allowToDelete: deleteChk.checked
            });
        }
    }

    // 2. Build the final DTO matching our C# model
    const saveDto = {
        idUser: parseInt(selectedUserId),
        rights: rightsPayloadArray
    };

    // 3. Send to API
    DOM.btnSave().disabled = true;
    DOM.btnSave().innerHTML = '<i class="fa fa-spinner fa-spin"></i> Saving...';

    try {
        const response = await apiFetch(`${API_RIGHTS}/save`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(saveDto)
        });

        const jsonResponse = await response.json();

        if (jsonResponse.success === true) {
            showToast("success", jsonResponse.message, "Permissions Updated");
        }
        else {
            throw new Error(jsonResponse.message);
        }
    }
    catch (err) {
        console.error("Save error:", err);
        showToast("danger", err.message, "Save Failed");
    }
    finally {
        DOM.btnSave().disabled = false;
        DOM.btnSave().innerHTML = '<i class="fa fa-save"></i> Save Permissions';
    }
}