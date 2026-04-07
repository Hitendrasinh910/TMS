const API = "/api/master/city";
const STATE_API = "/api/master/state";
let entryModal = null;

const DOM = {
    id: () => document.getElementById("hdnId"),
    stateId: () => document.getElementById("ddlState"),
    name: () => document.getElementById("txtCityName"),
    save: () => document.getElementById("btnSave"),
    tbody: () => document.getElementById("tblBody"),
    modal: () => document.getElementById("addModal"),
    table: () => document.getElementById("cityList")
};

document.addEventListener("DOMContentLoaded", async () => {

    const btnCreate = document.getElementById("btnCreateNewCity");

    if (btnCreate) {
        const hasAddRight = hasUserRight("City", "Add");
        btnCreate.classList.toggle("disabled", !hasAddRight);
    }

    await loadStates();
    await bindTable();
    entryModal = new bootstrap.Modal(DOM.modal(), { backdrop: "static" });
    DOM.modal().addEventListener("hidden.bs.modal", clearForm);
    DOM.save().addEventListener("click", saveData);
});

async function loadStates() {
    try {
        const res = await apiFetch(`${STATE_API}/get-all`);
        const json = await res.json();
        if (json.success) {
            const ddl = DOM.stateId();
            json.data.forEach(s => {
                ddl.innerHTML += `<option value="${s.idState}">${escapeHtml(s.state)}</option>`;
            });
        }

        $('.default-select, select').selectpicker('refresh');
    } catch (err) { console.error("Failed to load states", err); }
}

async function bindTable() {
    try {
        const res = await apiFetch(`${API}/get-all`);
        const json = await res.json();
        if (!json.success) return showToast("danger", json.message, "City Master");

        if ($.fn.DataTable.isDataTable(DOM.table())) $(DOM.table()).DataTable().destroy();
        DOM.tbody().innerHTML = "";

        const canEdit = hasUserRight("City", "Update");
        const canDelete = hasUserRight("City", "Delete");

        json.data.forEach(d => {
            const tr = document.createElement("tr");
            tr.innerHTML = `
                <td class="fw-semibold">${escapeHtml(d.city)}</td>
                <td>${escapeHtml(d.stateName || '')}</td>
                <td class="text-center">
                    <div class="d-flex justify-content-center">
                    ${canEdit
                    ? `<button onclick="editEntry(${d.idCity})" class="btn btn-primary shadow btn-xs sharp me-1"><i class="fa fa-pencil"></i></button>`
                        : `<button class="btn btn-primary shadow btn-xs sharp me-1 opacity-50" disabled><i class="fa fa-pencil"></i></button>`
                    }

                    ${canDelete
                    ? `<button onclick="deleteEntry(${d.idCity})" class="btn btn-danger shadow btn-xs sharp"><i class="fa fa-trash"></i></button>`
                        : `<button class="btn btn-danger shadow btn-xs sharp opacity-50" disabled><i class="fa fa-trash"></i></button>`
                     }
                    </div>
                 </td>`;
            DOM.tbody().appendChild(tr);
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
        $(DOM.table()).DataTable();
    } catch (err) { console.error(err); }
}

async function editEntry(id) {
    const res = await apiFetch(`${API}/get-by-id/${id}`);
    const json = await res.json();
    if (json.success) {
        DOM.id().value = json.data.idCity;
        DOM.stateId().value = json.data.idState || "";
        DOM.name().value = json.data.city || "";
        entryModal.show();
    }
}

async function deleteEntry(id) {
    if (!await confirmDelete("Delete this city?")) return;
    const res = await apiFetch(`${API}/delete/${id}`, { method: "DELETE" });
    const json = await res.json();
    if (json.success) { showToast("success", json.message, "City Master"); bindTable(); }
}

async function saveData() {
    if (!DOM.stateId().value || !DOM.name().value.trim()) return showToast("danger", "Fill required fields", "City Master");

    const dto = { idCity: Number(DOM.id().value || 0), idState: Number(DOM.stateId().value), city: DOM.name().value.trim() };
    DOM.save().disabled = true;

    const res = await apiFetch(`${API}/save`, { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify(dto) });
    const json = await res.json();

    if (json.success) {
        showToast("success", json.message, "City Master");
        entryModal.hide(); clearForm(); bindTable();
    }
    DOM.save().disabled = false;
}

function clearForm() { DOM.id().value = 0; DOM.stateId().value = ""; DOM.name().value = ""; }