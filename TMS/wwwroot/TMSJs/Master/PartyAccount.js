// ======================================================
// CONFIG
// ======================================================
const API = "/api/master/party-account";
const API_ACCOUNTTYPE = "/api/master/account-type";
const API_BALANCETYPE = "/api/master/balance-type";
const API_STATE = "/api/master/state";
const API_CITY = "/api/master/city";

let entryModal = null;

// ======================================================
// DOM CACHE
// ======================================================
const DOM = {
    id: () => document.getElementById("hdnId"),

    // Inputs
    accountSrNo: () => document.getElementById("txtAccountSrNo"),
    partyCode: () => document.getElementById("txtPartyCode"),
    partyName: () => document.getElementById("txtPartyName"),
    address: () => document.getElementById("txtAddress"),
    contactNo1: () => document.getElementById("txtContactNo1"),
    contactNo2: () => document.getElementById("txtContactNo2"),
    email: () => document.getElementById("txtEmail"),
    gstNo: () => document.getElementById("txtGSTNo"),
    panNo: () => document.getElementById("txtPanNo"),
    openingBalance: () => document.getElementById("txtOpeningBalance"),

    // Dropdowns
    accountType: () => document.getElementById("ddlAccountType"),
    balanceType: () => document.getElementById("ddlBalanceType"),
    state: () => document.getElementById("ddlState"),
    city: () => document.getElementById("ddlCity"),

    // UI Elements
    save: () => document.getElementById("btnSave"),
    tbody: () => document.getElementById("tblBody"),
    modal: () => document.getElementById("addModal"),
    table: () => document.getElementById("partyAccountList")
};

// ======================================================
// INIT
// ======================================================
document.addEventListener("DOMContentLoaded", async () => {
    if (!guardPageAccess("Party Account")) return;

    const btnCreate = document.getElementById("btnCreateNewPartyAccount");

    if (btnCreate) {
        const hasAddRight = hasUserRight("Party Account", "Add");
        btnCreate.classList.toggle("disabled", !hasAddRight);
        //if (!hasAddRight) btnCreate.removeAttribute("href");
    }

    DOM.modal().addEventListener("shown.bs.modal", async () => {
        // Fetch the next Sr No ONLY if it's a new entry (ID is 0)
        if (Number(DOM.id().value) === 0) {
            await fetchNextAccountSrNo();
        }

        setTimeout(() => DOM.partyName().focus(), 50);
    });

    // Load all dropdowns concurrently for better performance
    await loadDropdowns();
    await bindTable();

    entryModal = new bootstrap.Modal(DOM.modal(), { backdrop: "static" });


    DOM.modal().addEventListener("hidden.bs.modal", clearForm);
    DOM.save().addEventListener("click", saveData);
});

// FETCH NEXT ACCOUNT SR NO
// ======================================================
async function fetchNextAccountSrNo() {
    try {
        // Adjust this endpoint to match your actual backend API route
        const res = await apiFetch(`${API}/get-sr-no`);
        const json = await res.json();

        if (json.success) {
            // Assuming your API returns the number in json.data
            DOM.accountSrNo().value = json.data;
        } else {
            showToast("warning", "Could not generate Account Sr No", "Party Account");
        }
    } catch (err) {
        console.error("Error fetching Account Sr No:", err);
    }
}

// ======================================================
// LOAD DROPDOWNS
// ======================================================
async function loadDropdowns() {
    try {
        const [accTypeRes, balTypeRes, stateRes, cityRes] = await Promise.all([
            apiFetch(`${API}/get-all-account-type`).then(r => r.json()),
            apiFetch(`${API}/get-all-balance-type`).then(r => r.json()),
            apiFetch(`${API_STATE}/get-all`).then(r => r.json()),
            apiFetch(`${API_CITY}/get-all`).then(r => r.json())
        ]);

        populateDropdown(DOM.accountType(), accTypeRes, "idAccountType", "accountType", "-- Select Account Type --");
        populateDropdown(DOM.balanceType(), balTypeRes, "idBalanceType", "balanceType", "-- Select Balance Type --");
        populateDropdown(DOM.state(), stateRes, "idState", "state", "-- Select State --");
        populateDropdown(DOM.city(), cityRes, "idCity", "city", "-- Select City --");

        $('.default-select, select').selectpicker('refresh');
    } catch (err) {
        console.error("Error loading dropdowns:", err);
        showToast("warning", "Could not load all dropdown options", "Party Account");
    }
}

function populateDropdown(selectElement, responseJson, valueProp, textProp, defaultText) {
    selectElement.innerHTML = `<option value="">${defaultText}</option>`;
    if (responseJson && responseJson.success && responseJson.data) {
        responseJson.data.forEach(item => {
            selectElement.innerHTML += `<option value="${item[valueProp]}">${escapeHtml(item[textProp])}</option>`;
        });
    }
}

// ======================================================
// BIND TABLE
// ======================================================
async function bindTable() {
    try {
        const res = await apiFetch(`${API}/get-all`);
        const json = await res.json();

        if (!json.success) {
            showToast("danger", json.message, "Party Account Master");
            return;
        }

        if ($.fn.DataTable.isDataTable(DOM.table())) {
            $(DOM.table()).DataTable().destroy();
        }

        const tbody = DOM.tbody();
        tbody.innerHTML = "";

        const canEdit = hasUserRight("Party Account", "Update");
        const canDelete = hasUserRight("Party Account", "Delete");

        json.data.forEach(d => {
            const tr = document.createElement("tr");
            tr.id = `row-${d.idPartyAccount}`;

            // Formatting balance
            const balanceDisplay = d.openingBalance ? parseFloat(d.openingBalance).toFixed(2) : "0.00";

            tr.innerHTML = `
                <td class="fw-semibold">${escapeHtml(d.partyCode || '-')}</td>
                <td><h6 class="w-space-no mb-0 fs-14 font-w600">${escapeHtml(d.partyName)}</h6></td>
                <td>${escapeHtml(d.contactNo1 || '-')}</td>
                <td>${escapeHtml(d.cityName || '-')},${escapeHtml(d.stateName || '-')}</td>
                <td>₹ ${balanceDisplay}</td>
                <td class="text-center">
                    <div class="d-flex justify-content-center">
                    ${canEdit
                    ? `<button onclick="editEntry(${d.idPartyAccount})" class="btn btn-primary shadow btn-xs sharp me-1"><i class="fa fa-pencil"></i></button>`
                        : `<button class="btn btn-primary shadow btn-xs sharp me-1 opacity-50" disabled><i class="fa fa-pencil"></i></button>`
                    }

                    ${canDelete
                    ? `<button onclick="deleteEntry(${d.idPartyAccount})" class="btn btn-danger shadow btn-xs sharp"><i class="fa fa-trash"></i></button>`
                        : `<button class="btn btn-danger shadow btn-xs sharp opacity-50" disabled><i class="fa fa-trash"></i></button>`
                     }
                 </div>
                </td>
            `;
            tbody.appendChild(tr);
        });

        $(DOM.table()).DataTable({
            lengthChange: true,
            searching: true,
            pageLength: 10,
            ordering: false,
            language: {
                paginate: {
                    next: '<i class="fa fa-angle-double-right" aria-hidden="true"></i>',
                    previous: '<i class="fa fa-angle-double-left" aria-hidden="true"></i>'
                }
            }
        });

    } catch (err) {
        console.error(err);
        showToast("danger", "Failed to load data", "Party Account Master");
    }
}

// ======================================================
// EDIT
// ======================================================
async function editEntry(id) {
    try {
        const res = await apiFetch(`${API}/get-by-id/${id}`, { method: "GET" });
        const json = await res.json();

        if (!json.success) throw new Error(json.message || "Failed to load party account");

        const d = json.data;

        DOM.id().value = d.idPartyAccount;
        DOM.accountSrNo().value = d.accountSrNo || "";
        DOM.partyCode().value = d.partyCode || "";
        DOM.accountType().value = d.idAccountType || "";
        DOM.partyName().value = d.partyName || "";
        DOM.address().value = d.address || "";
        DOM.state().value = d.idState || "";
        DOM.city().value = d.idCity || "";
        DOM.contactNo1().value = d.contactNo1 || "";
        DOM.contactNo2().value = d.contactNo2 || "";
        DOM.email().value = d.email || "";
        DOM.gstNo().value = d.gstNo || "";
        DOM.panNo().value = d.panNo || "";
        DOM.openingBalance().value = d.openingBalance || "0.00";
        DOM.balanceType().value = d.idBalanceType || "";

        entryModal.show();
    } catch (err) {
        showToast("danger", err.message, "Party Account Master");
    }
}

// ======================================================
// DELETE
// ======================================================
async function deleteEntry(id) {
    const ok = await confirmDelete("This record will be deleted permanently!");
    if (!ok) return;

    try {
        const res = await apiFetch(`${API}/delete/${id}`, { method: "DELETE" });
        const json = await res.json();

        if (!json.success) throw new Error(json.message);

        showToast("success", json.message, "Party Account Master");
        bindTable();
    } catch (err) {
        showToast("danger", err.message, "Party Account Master");
    }
}

// ======================================================
// SAVE
// ======================================================
async function saveData() {
    let isValid = true;

    // Validate Party Name
    if (!DOM.partyName().value.trim()) {
        DOM.partyName().classList.add("is-invalid");
        isValid = false;
    } else {
        DOM.partyName().classList.remove("is-invalid");
    }

    // Validate Contact No 1
    if (!DOM.contactNo1().value.trim()) {
        DOM.contactNo1().classList.add("is-invalid");
        isValid = false;
    } else {
        DOM.contactNo1().classList.remove("is-invalid");
    }

    if (!isValid) {
        showToast("danger", "Please fill required fields", "Party Account Master");
        return;
    }

    const dto = {
        idPartyAccount: Number(DOM.id().value || 0),
        accountSrNo: Number(DOM.accountSrNo().value || 0),
        partyCode: DOM.partyCode().value.trim(),
        idAccountType: Number(DOM.accountType().value || 0),
        partyName: DOM.partyName().value.trim(),
        address: DOM.address().value.trim(),
        idState: Number(DOM.state().value || 0),
        idCity: Number(DOM.city().value || 0),
        contactNo1: DOM.contactNo1().value.trim(),
        contactNo2: DOM.contactNo2().value.trim(),
        email: DOM.email().value.trim(),
        gstNo: DOM.gstNo().value.trim(),
        panNo: DOM.panNo().value.trim(),
        openingBalance: Number(DOM.openingBalance().value || 0),
        idBalanceType: Number(DOM.balanceType().value || 0)
    };

    DOM.save().disabled = true;

    try {
        const res = await apiFetch(`${API}/save`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(dto)
        });

        const json = await res.json();

        if (!json.success) throw new Error(json.message || "Save failed");

        showToast("success", json.message, "Party Account Master");

        entryModal.hide();
        clearForm();
        bindTable();
    } catch (err) {
        showToast("danger", err.message, "Party Account Master");
    } finally {
        DOM.save().disabled = false;
    }
}

// ======================================================
// CLEAR
// ======================================================
function clearForm() {
    DOM.id().value = 0;

    DOM.accountSrNo().value = "";
    DOM.partyCode().value = "";
    DOM.accountType().value = "";
    DOM.partyName().value = "";
    DOM.address().value = "";
    DOM.state().value = "";
    DOM.city().value = "";
    DOM.contactNo1().value = "";
    DOM.contactNo2().value = "";
    DOM.email().value = "";
    DOM.gstNo().value = "";
    DOM.panNo().value = "";
    DOM.openingBalance().value = "0.00";
    DOM.balanceType().value = "";

    // Clear validation styling
    DOM.partyName().classList.remove("is-invalid");
    DOM.contactNo1().classList.remove("is-invalid");
}