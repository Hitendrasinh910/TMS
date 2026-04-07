// ======================================================
// CONFIG
// ======================================================
const API = "/api/master/country";
let entryModal = null;

// ======================================================
// DOM CACHE
// ======================================================
const DOM = {
    id: () => document.getElementById("hdnId"),
    name: () => document.getElementById("txtCountryName"),
    save: () => document.getElementById("btnSave"),
    tbody: () => document.getElementById("tblBody"),
    modal: () => document.getElementById("addModal"),
    table: () => document.getElementById("countryList")
};

// ======================================================
// INIT
// ======================================================
document.addEventListener("DOMContentLoaded", async () => {
    const btnCreate = document.getElementById("btnCreateNewCountry");

    if (btnCreate) {
        const hasAddRight = hasUserRight("Country", "Add");
        btnCreate.classList.toggle("disabled", !hasAddRight);
        //if (!hasAddRight) btnCreate.removeAttribute("href");
    }

    await bindTable();

    entryModal = new bootstrap.Modal(DOM.modal(), { backdrop: "static" });

    DOM.modal().addEventListener("shown.bs.modal", () => {
        setTimeout(() => DOM.name().focus(), 50);
    });

    DOM.modal().addEventListener("hidden.bs.modal", clearForm);

    DOM.save().addEventListener("click", saveData);
});

// ======================================================
// BIND TABLE
// ======================================================
async function bindTable() {
    try {
        const res = await apiFetch(`${API}/get-all`);
        const json = await res.json();

        if (!json.success) {
            showToast("danger", json.message, "Country Master");
            return;
        }

        // Destroy existing DataTable before modifying DOM to prevent errors
        if ($.fn.DataTable.isDataTable(DOM.table())) {
            $(DOM.table()).DataTable().destroy();
        }

        const tbody = DOM.tbody();
        tbody.innerHTML = "";

        const canEdit = hasUserRight("Country", "Update");
        const canDelete = hasUserRight("Country", "Delete");

        json.data.forEach(d => {
            const tr = document.createElement("tr");
            tr.id = `row-${d.idCountry}`;

            tr.innerHTML = `
                <td class="fw-semibold">
                    <h6 class="w-space-no mb-0 fs-14 font-w600"> ${escapeHtml(d.country)} </h6>
                </td>
                <td>${escapeHtml(d.e_By || 'System')}</td>
                <td>${formatDateTime(d.e_Date)}</td>
                <td class="text-center">
                    <div class="d-flex justify-content-center">
                    ${canEdit
                    ? `<button onclick="editEntry(${d.idCountry})" class="btn btn-primary shadow btn-xs sharp me-1"><i class="fa fa-pencil"></i></button>`
                        : `<button class="btn btn-primary shadow btn-xs sharp me-1 opacity-50" disabled><i class="fa fa-pencil"></i></button>`
                    }

                    ${canDelete
                    ? `<button onclick="deleteEntry(${d.idCountry})" class="btn btn-danger shadow btn-xs sharp"><i class="fa fa-trash"></i></button>`
                        : `<button class="btn btn-danger shadow btn-xs sharp opacity-50" disabled><i class="fa fa-trash"></i></button>`
                     }
                 </div>
                </td>
            `;
            tbody.appendChild(tr);
        });

        // Re-initialize DataTable
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
        showToast("danger", "Failed to load data", "Country Master");
    }
}

// ======================================================
// EDIT
// ======================================================
async function editEntry(id) {
    try {
        const res = await apiFetch(`${API}/get-by-id/${id}`, { method: "GET" });
        const json = await res.json();

        if (!json.success) throw new Error(json.message || "Failed to load country");

        const d = json.data;

        DOM.id().value = d.idCountry;
        DOM.name().value = d.country || "";

        entryModal.show();
    } catch (err) {
        showToast("danger", err.message, "Country Master");
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

        showToast("success", json.message, "Country Master");
        bindTable();
    } catch (err) {
        showToast("danger", err.message, "Country Master");
    }
}

// ======================================================
// SAVE
// ======================================================
async function saveData() {
    if (!DOM.name().value.trim()) {
        DOM.name().classList.add("is-invalid");
        showToast("danger", "Country name required", "Country Master");
        return;
    }

    const dto = {
        idCountry: Number(DOM.id().value || 0),
        country: DOM.name().value.trim()
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

        showToast("success", json.message, "Country Master");

        entryModal.hide();
        clearForm();
        bindTable();
    } catch (err) {
        showToast("danger", err.message, "Country Master");
    } finally {
        DOM.save().disabled = false;
    }
}

// ======================================================
// CLEAR
// ======================================================
function clearForm() {
    DOM.id().value = 0;
    DOM.name().value = "";
    DOM.name().classList.remove("is-invalid");
}