// ======================================================
// CONFIG
// ======================================================
const API = "/api/master/state";
const COUNTRY_API = "/api/master/country";
let entryModal = null;

// ======================================================
// DOM CACHE
// ======================================================
const DOM = {
    id: () => document.getElementById("hdnId"),
    countryId: () => document.getElementById("ddlCountry"),
    name: () => document.getElementById("txtStateName"),
    save: () => document.getElementById("btnSave"),
    tbody: () => document.getElementById("tblBody"),
    modal: () => document.getElementById("addModal"),
    table: () => document.getElementById("stateList")
};

// ======================================================
// INIT
// ======================================================
document.addEventListener("DOMContentLoaded", async () => {

    if (!guardPageAccess("State")) return;

    const btnCreate = document.getElementById("btnCreateNewState");

    if (btnCreate) {
        const hasAddRight = hasUserRight("State", "Add");
        btnCreate.classList.toggle("disabled", !hasAddRight);
        //if (!hasAddRight) btnCreate.removeAttribute("href");
    }

    await loadCountries();
    await bindTable();

    entryModal = new bootstrap.Modal(DOM.modal(), { backdrop: "static" });

    DOM.modal().addEventListener("shown.bs.modal", () => {
        setTimeout(() => DOM.name().focus(), 50);
    });

    DOM.modal().addEventListener("hidden.bs.modal", clearForm);
    DOM.save().addEventListener("click", saveData);
});

// ======================================================
// LOAD DROPDOWNS
// ======================================================
async function loadCountries() {
    try {
        const res = await apiFetch(`${COUNTRY_API}/get-all`);
        const json = await res.json();

        if (json.success) {
            const ddl = DOM.countryId();
            ddl.innerHTML = '<option value="">-- Select Country --</option>';
            json.data.forEach(c => {
                ddl.innerHTML += `<option value="${c.idCountry}">${escapeHtml(c.country)}</option>`;
            });
        }

        $('.default-select, select').selectpicker('refresh');
    } catch (err) {
        console.error("Failed to load countries", err);
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
            showToast("danger", json.message, "State Master");
            return;
        }

        if ($.fn.DataTable.isDataTable(DOM.table())) {
            $(DOM.table()).DataTable().destroy();
        }

        const tbody = DOM.tbody();
        tbody.innerHTML = "";

        const canEdit = hasUserRight("State", "Update");
        const canDelete = hasUserRight("State", "Delete");

        // Assuming your backend query does a JOIN to get the CountryName, 
        // if not, you might just display ID or handle it via a map. 
        // We'll fallback to idCountry if countryName isn't returned from the API.
        json.data.forEach(d => {
            const tr = document.createElement("tr");
            tr.id = `row-${d.idState}`;

            tr.innerHTML = `
                <td class="fw-semibold">
                    <h6 class="w-space-no mb-0 fs-14 font-w600"> ${escapeHtml(d.state)} </h6>
                </td>
                <td>${escapeHtml(d.countryName || '')}</td>
                <td>${formatDateTime(d.e_Date)}</td>
                <td class="text-center">
                    <div class="d-flex justify-content-center">
                    ${canEdit
                    ? `<button onclick="editEntry(${d.idState})" class="btn btn-primary shadow btn-xs sharp me-1"><i class="fa fa-pencil"></i></button>`
                    : `<button class="btn btn-primary shadow btn-xs sharp me-1 opacity-50" disabled><i class="fa fa-pencil"></i></button>`
                    }

                    ${canDelete
                    ? `<button onclick="deleteEntry(${d.idState})" class="btn btn-danger shadow btn-xs sharp"><i class="fa fa-trash"></i></button>`
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
        showToast("danger", "Failed to load data", "State Master");
    }
}

// ======================================================
// EDIT
// ======================================================
async function editEntry(id) {
    try {
        const res = await apiFetch(`${API}/get-by-id/${id}`, { method: "GET" });
        const json = await res.json();

        if (!json.success) throw new Error(json.message || "Failed to load state");

        const d = json.data;

        DOM.id().value = d.idState;
        DOM.countryId().value = d.idCountry || "";
        DOM.name().value = d.state || "";

        entryModal.show();
    } catch (err) {
        showToast("danger", err.message, "State Master");
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

        showToast("success", json.message, "State Master");
        bindTable();
    } catch (err) {
        showToast("danger", err.message, "State Master");
    }
}

// ======================================================
// SAVE
// ======================================================
async function saveData() {
    let isValid = true;

    if (!DOM.countryId().value) {
        DOM.countryId().classList.add("is-invalid");
        isValid = false;
    } else {
        DOM.countryId().classList.remove("is-invalid");
    }

    if (!DOM.name().value.trim()) {
        DOM.name().classList.add("is-invalid");
        isValid = false;
    } else {
        DOM.name().classList.remove("is-invalid");
    }

    if (!isValid) {
        showToast("danger", "Please fill required fields", "State Master");
        return;
    }

    const dto = {
        idState: Number(DOM.id().value || 0),
        idCountry: Number(DOM.countryId().value),
        state: DOM.name().value.trim()
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

        showToast("success", json.message, "State Master");

        entryModal.hide();
        clearForm();
        bindTable();
    } catch (err) {
        showToast("danger", err.message, "State Master");
    } finally {
        DOM.save().disabled = false;
    }
}

// ======================================================
// CLEAR
// ======================================================
function clearForm() {
    DOM.id().value = 0;
    DOM.countryId().value = "";
    DOM.name().value = "";

    DOM.countryId().classList.remove("is-invalid");
    DOM.name().classList.remove("is-invalid");
}