// ======================================================
// CONFIG
// ======================================================
const API = "/api/master/bill-to-party";
const API_PARTY = "/api/master/party-account"; // Assuming this handles parties
let entryModal = null;

// ======================================================
// DOM CACHE
// ======================================================
const DOM = {
    id: () => document.getElementById("hdnId"),
    srNo: () => document.getElementById("txtSrNo"),
    consignor: () => document.getElementById("ddlConsignor"),
    consignee: () => document.getElementById("ddlConsignee"),
    billTo: () => document.getElementById("ddlBillTo"),
    remarks: () => document.getElementById("txtRemarks"),

    save: () => document.getElementById("btnSave"),
    tbody: () => document.getElementById("tblBody"),
    modal: () => document.getElementById("addModal"),
    table: () => document.getElementById("billToList")
};

// ======================================================
// INIT
// ======================================================
document.addEventListener("DOMContentLoaded", async () => {
    const btnCreate = document.getElementById("btnCreateNewBillToParty");

    if (btnCreate) {
        const hasAddRight = hasUserRight("Bill To Party", "Add");
        btnCreate.classList.toggle("disabled", !hasAddRight);
    }

    await loadDropdowns();
    await bindTable();

    entryModal = new bootstrap.Modal(DOM.modal(), { backdrop: "static" });

    // Fetch the next Sr No ONLY if it's a new entry (ID is 0)
    if (Number(DOM.id().value) === 0) {
        await fetchNextAccountSrNo();
    }
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
            DOM.srNo().value = json.data;
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
        //const res = await apiFetch(`${API_PARTY}/get-all`);
        //const json = await res.json();

        //if (json.success && json.data) {
        //    let options = '<option value="">-- Select Party --</option>';
        //    json.data.forEach(p => {
        //        options += `<option value="${p.idPartyAccount}">${escapeHtml(p.partyName)}</option>`;
        //    });

        //    // Populate all 3 dropdowns with the exact same party data
        //    DOM.consignor().innerHTML = options;
        //    DOM.consignee().innerHTML = options;
        //    DOM.billTo().innerHTML = options;

        //}
        const [allPartyRes, consignorRes, consigneeRes] = await Promise.all([
            apiFetch(`${API_PARTY}/get-all`).then(r => r.json()),
            apiFetch(`${API_PARTY}/get-all-consignor`).then(r => r.json()),
            apiFetch(`${API_PARTY}/get-all-consignee`).then(r => r.json())  
        ]);

        populateDropdown(DOM.billTo(), allPartyRes, "idPartyAccount", "partyName", "-- Select Party --");
        populateDropdown(DOM.consignor(), consignorRes, "idPartyAccount", "partyName", "-- Select Consignor --");
        populateDropdown(DOM.consignee(), consigneeRes, "idPartyAccount", "partyName", "-- Select Consignee --");

        $('.form-select, select').selectpicker('refresh');
    } catch (err) {
        console.error("Error loading parties:", err);
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
    const res = await apiFetch(`${API}/get-all`);
    const json = await res.json();
    if ($.fn.DataTable.isDataTable(DOM.table())) $(DOM.table()).DataTable().destroy();
    DOM.tbody().innerHTML = "";

    const canEdit = hasUserRight("Bill To Party", "Update");
    const canDelete = hasUserRight("Bill To Party", "Delete");

    json.data.forEach(d => {
        const tr = document.createElement("tr");
        tr.innerHTML = `
            <td>${d.srNo || '-'}</td>
            <td class="fw-semibold text-primary">${escapeHtml(d.consignorName || 'Unknown')}</td>
            <td class="fw-semibold text-info">${escapeHtml(d.consigneeName || 'Unknown')}</td>
            <td class="fw-semibold text-success">${escapeHtml(d.billToName || 'Unknown')}</td>
            <td class="text-center">
            <div class="d-flex justify-content-center">
                    ${canEdit
            ? `<button onclick="editEntry(${d.idPartyBill})" class="btn btn-primary shadow btn-xs sharp me-1"><i class="fa fa-pencil"></i></button>`
                : `<button class="btn btn-primary shadow btn-xs sharp me-1 opacity-50" disabled><i class="fa fa-pencil"></i></button>`
            }

                    ${canDelete
            ? `<button onclick="deleteEntry(${d.idPartyBill})" class="btn btn-danger shadow btn-xs sharp"><i class="fa fa-trash"></i></button>`
                : `<button class="btn btn-danger shadow btn-xs sharp opacity-50" disabled><i class="fa fa-trash"></i></button>`
                     }
                    </div>
           </td>`;
        DOM.tbody().appendChild(tr);
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
}

// ======================================================
// EDIT
// ======================================================
async function editEntry(id) {
    const res = await apiFetch(`${API}/get-by-id/${id}`);
    const json = await res.json();
    if (json.success) {
        DOM.id().value = json.data.idPartyBill;
        DOM.srNo().value = json.data.srNo || "";
        DOM.consignor().value = json.data.idConsignor || "";
        DOM.consignee().value = json.data.idConsignee || "";
        DOM.billTo().value = json.data.billTo || "";
        DOM.remarks().value = json.data.remarks || "";
        entryModal.show();
    }
}

// ======================================================
// DELETE
// ======================================================
async function deleteEntry(id) {
    if (!await confirmDelete("Delete this mapping?")) return;
    const res = await apiFetch(`${API}/delete/${id}`, { method: "DELETE" });
    const json = await res.json();
    if (json.success) { showToast("success", json.message, "Bill To Party"); bindTable(); }
}

// ======================================================
// SAVE
// ======================================================
async function saveData() {
    let isValid = true;
    [DOM.consignor(), DOM.consignee(), DOM.billTo()].forEach(el => {
        if (!el.value) { el.classList.add("is-invalid"); isValid = false; }
        else { el.classList.remove("is-invalid"); }
    });

    if (!isValid) return showToast("danger", "Please select all required parties", "Bill To Party");

    const dto = {
        idPartyBill: Number(DOM.id().value || 0),
        srNo: Number(DOM.srNo().value || 0),
        idConsignor: Number(DOM.consignor().value),
        idConsignee: Number(DOM.consignee().value),
        billTo: Number(DOM.billTo().value),
        remarks: DOM.remarks().value.trim()
    };

    DOM.save().disabled = true;
    const res = await apiFetch(`${API}/save`, { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify(dto) });
    const json = await res.json();

    if (json.success) { showToast("success", json.message, "Bill To Party"); entryModal.hide(); clearForm(); bindTable(); }
    else { showToast("danger", json.message, "Error"); }

    DOM.save().disabled = false;
}

function clearForm() {
    DOM.id().value = 0;
    DOM.srNo().value = "";
    DOM.consignor().value = "";
    DOM.consignee().value = "";
    DOM.billTo().value = "";
    DOM.remarks().value = "";

    [DOM.consignor(), DOM.consignee(), DOM.billTo()].forEach(el => el.classList.remove("is-invalid"));
}