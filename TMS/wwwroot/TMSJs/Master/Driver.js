// ======================================================
// CONFIG
// ======================================================
const API = "/api/master/driver";
let entryModal = null;

// ======================================================
// DOM CACHE
// ======================================================
const DOM = {
    id: () => document.getElementById("hdnId"),
    name: () => document.getElementById("txtDriverName"),
    address: () => document.getElementById("txtAddress"),
    contactNo: () => document.getElementById("txtContactNo"),
    emergencyNo: () => document.getElementById("txtEmergencyContactNo"),
    dlNo: () => document.getElementById("txtDLNo"),
    dlValidTill: () => document.getElementById("txtDLValidTill"),

    save: () => document.getElementById("btnSave"),
    tbody: () => document.getElementById("tblBody"),
    modal: () => document.getElementById("addModal"),
    table: () => document.getElementById("driverList")
};

// ======================================================
// INIT
// ======================================================
document.addEventListener("DOMContentLoaded", async () => {
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

        if (!json.success) return showToast("danger", json.message, "Driver Master");

        if ($.fn.DataTable.isDataTable(DOM.table())) {
            $(DOM.table()).DataTable().destroy();
        }

        DOM.tbody().innerHTML = "";

        json.data.forEach(d => {
            const tr = document.createElement("tr");

            // Format DL date if it exists
            const dlDate = d.dlValidTill ? new Date(d.dlValidTill).toLocaleDateString() : '-';

            tr.innerHTML = `
                <td class="fw-semibold">${escapeHtml(d.driverName)}</td>
                <td>${escapeHtml(d.contactNo || '-')}</td>
                <td>${escapeHtml(d.drivingLicenceNo || '-')}</td>
                <td>${dlDate}</td>
                <td class="text-center">
                    <div class="d-flex">
                        <a href="javascript:void(0);" onclick="editEntry(${d.idDriver})" class="btn btn-primary btn-xs sharp me-1"><i class="fa fa-pencil"></i></a>
                        <a href="javascript:void(0);" onclick="deleteEntry(${d.idDriver})" class="btn btn-danger btn-xs sharp"><i class="fa fa-trash"></i></a>
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

    } catch (err) { console.error(err); }
}

// ======================================================
// EDIT
// ======================================================
async function editEntry(id) {
    try {
        const res = await apiFetch(`${API}/get-by-id/${id}`);
        const json = await res.json();

        if (json.success) {
            const d = json.data;
            DOM.id().value = d.idDriver;
            DOM.name().value = d.driverName || "";
            DOM.address().value = d.address || "";
            DOM.contactNo().value = d.contactNo || "";
            DOM.emergencyNo().value = d.emergencyContactNo || "";
            DOM.dlNo().value = d.drivingLicenceNo || "";

            // Bind date value to HTML5 date input (requires YYYY-MM-DD format)
            if (d.dlValidTill) {
                DOM.dlValidTill().value = d.dlValidTill.split("T")[0];
            } else {
                DOM.dlValidTill().value = "";
            }

            entryModal.show();
        }
    } catch (err) { showToast("danger", err.message, "Driver Master"); }
}

// ======================================================
// DELETE
// ======================================================
async function deleteEntry(id) {
    if (!await confirmDelete("Delete this driver?")) return;

    try {
        const res = await apiFetch(`${API}/delete/${id}`, { method: "DELETE" });
        const json = await res.json();

        if (json.success) {
            showToast("success", json.message, "Driver Master");
            bindTable();
        }
    } catch (err) { showToast("danger", err.message, "Driver Master"); }
}

// ======================================================
// SAVE
// ======================================================
async function saveData() {
    let isValid = true;

    if (!DOM.name().value.trim()) {
        DOM.name().classList.add("is-invalid");
        isValid = false;
    } else {
        DOM.name().classList.remove("is-invalid");
    }

    if (!DOM.contactNo().value.trim()) {
        DOM.contactNo().classList.add("is-invalid");
        isValid = false;
    } else {
        DOM.contactNo().classList.remove("is-invalid");
    }

    if (!isValid) return showToast("danger", "Fill required fields", "Driver Master");

    const dto = {
        idDriver: Number(DOM.id().value || 0),
        driverName: DOM.name().value.trim(),
        address: DOM.address().value.trim(),
        contactNo: DOM.contactNo().value.trim(),
        emergencyContactNo: DOM.emergencyNo().value.trim(),
        drivingLicenceNo: DOM.dlNo().value.trim(),
        dlValidTill: DOM.dlValidTill().value || null
    };

    DOM.save().disabled = true;

    try {
        const res = await apiFetch(`${API}/save`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(dto)
        });
        const json = await res.json();

        if (json.success) {
            showToast("success", json.message, "Driver Master");
            entryModal.hide();
            clearForm();
            bindTable();
        } else {
            throw new Error(json.message);
        }
    } catch (err) {
        showToast("danger", err.message, "Driver Master");
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
    DOM.address().value = "";
    DOM.contactNo().value = "";
    DOM.emergencyNo().value = "";
    DOM.dlNo().value = "";
    DOM.dlValidTill().value = "";

    DOM.name().classList.remove("is-invalid");
    DOM.contactNo().classList.remove("is-invalid");
}