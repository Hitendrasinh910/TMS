const API = "/api/master/truck";
let entryModal = null;

const DOM = {
    id: () => document.getElementById("hdnId"),
    truckNo: () => document.getElementById("txtTruckNumber"),
    panHolder: () => document.getElementById("txtPanHolder"),
    panNo: () => document.getElementById("txtPanNo"),
    remarks: () => document.getElementById("txtRemarks"),
    save: () => document.getElementById("btnSave"),
    tbody: () => document.getElementById("tblBody"),
    modal: () => document.getElementById("addModal"),
    table: () => document.getElementById("truckList")
};

document.addEventListener("DOMContentLoaded", async () => {
    if (!guardPageAccess("Master Truck")) return;

    const btnCreate = document.getElementById("btnCreateNewTruck");

    if (btnCreate) {
        const hasAddRight = hasUserRight("Master Truck", "Add");
        btnCreate.classList.toggle("disabled", !hasAddRight);
        //if (!hasAddRight) btnCreate.removeAttribute("href");
    }

    await bindTable();
    entryModal = new bootstrap.Modal(DOM.modal(), { backdrop: "static" });
    DOM.modal().addEventListener("hidden.bs.modal", clearForm);
    DOM.save().addEventListener("click", saveData);
});

async function bindTable() {
    const res = await apiFetch(`${API}/get-all`);
    const json = await res.json();
    if ($.fn.DataTable.isDataTable(DOM.table())) $(DOM.table()).DataTable().destroy();
    DOM.tbody().innerHTML = "";

    const canEdit = hasUserRight("Master Truck", "Update");
    const canDelete = hasUserRight("Master Truck", "Delete");

    json.data.forEach(d => {
        const tr = document.createElement("tr");
        tr.innerHTML = `
            <td class="fw-semibold">${escapeHtml(d.truckNumber)}</td>
            <td>${escapeHtml(d.panCardHolder || '-')}</td>
            <td>${escapeHtml(d.panCardNo || '-')}</td>
            <td class="text-center">
                <div class="d-flex justify-content-center">
                    ${canEdit
                    ? `<button onclick="editEntry(${d.idTruck})" class="btn btn-primary shadow btn-xs sharp me-1"><i class="fa fa-pencil"></i></button>`
                        : `<button class="btn btn-primary shadow btn-xs sharp me-1 opacity-50" disabled><i class="fa fa-pencil"></i></button>`
                    }

                    ${canDelete
                    ? `<button onclick="deleteEntry(${d.idTruck})" class="btn btn-danger shadow btn-xs sharp"><i class="fa fa-trash"></i></button>`
                        : `<button class="btn btn-danger shadow btn-xs sharp opacity-50" disabled><i class="fa fa-trash"></i></button>`
                     }
                 </div>
            </td>`;
        DOM.tbody().appendChild(tr);
    });
    //$(DOM.table()).DataTable();
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
}

async function editEntry(id) {
    const res = await apiFetch(`${API}/get-by-id/${id}`);
    const json = await res.json();
    if (json.success) {
        DOM.id().value = json.data.idTruck;
        DOM.truckNo().value = json.data.truckNumber || "";
        DOM.panHolder().value = json.data.panCardHolder || "";
        DOM.panNo().value = json.data.panCardNo || "";
        DOM.remarks().value = json.data.remarks || "";
        entryModal.show();
    }
}

async function deleteEntry(id) {
    if (!await confirmDelete("Delete this truck?")) return;
    const res = await apiFetch(`${API}/delete/${id}`, { method: "DELETE" });
    const json = await res.json();
    if (json.success) { showToast("success", json.message, "Truck Master"); bindTable(); }
}

async function saveData() {
    if (!DOM.truckNo().value.trim()) return showToast("danger", "Truck Number required", "Truck Master");

    const dto = {
        idTruck: Number(DOM.id().value || 0),
        truckNumber: DOM.truckNo().value.trim(),
        panCardHolder: DOM.panHolder().value.trim(),
        panCardNo: DOM.panNo().value.trim(),
        remarks: DOM.remarks().value.trim()
    };

    DOM.save().disabled = true;
    const res = await apiFetch(`${API}/save`, { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify(dto) });
    const json = await res.json();

    if (json.success) { showToast("success", json.message, "Truck Master"); entryModal.hide(); clearForm(); bindTable(); }
    DOM.save().disabled = false;
}

function clearForm() { DOM.id().value = 0; DOM.truckNo().value = ""; DOM.panHolder().value = ""; DOM.panNo().value = ""; DOM.remarks().value = ""; }