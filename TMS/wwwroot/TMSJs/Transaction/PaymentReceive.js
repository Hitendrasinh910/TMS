const API = "/api/transaction/payment-receive";
const API_PARTY = "/api/master/party-account";
let entryModal = null;

const DOM = {
    id: () => document.getElementById("hdnId"),
    receiptNo: () => document.getElementById("txtReceiptNo"),
    date: () => document.getElementById("txtDate"),
    type: () => document.getElementById("ddlType"),
    party: () => document.getElementById("ddlParty"),
    billNo: () => document.getElementById("ddlBillNo"),
    billAmt: () => document.getElementById("txtBillAmt"),
    outstanding: () => document.getElementById("txtOutstanding"),
    mode: () => document.getElementById("ddlMode"),
    received: () => document.getElementById("txtAmtReceived"),
    tds: () => document.getElementById("txtTDS"),
    balance: () => document.getElementById("txtBalance"),
    remarks: () => document.getElementById("txtRemarks"),
    save: () => document.getElementById("btnSave"),
    tbody: () => document.getElementById("tblBody"),
    modal: () => document.getElementById("addModal"),
    table: () => document.getElementById("paymentList")
};

document.addEventListener("DOMContentLoaded", async () => {
    DOM.date().value = new Date().toISOString().split('T')[0];
    await loadParties();
    await bindTable();

    // Auto Calculate Balance
    document.querySelectorAll('.calc').forEach(el => {
        el.addEventListener('input', calculateBalance);
    });

    entryModal = new bootstrap.Modal(DOM.modal(), { backdrop: "static" });
    DOM.modal().addEventListener("hidden.bs.modal", clearForm);
    DOM.save().addEventListener("click", saveData);
});

async function loadParties() {
    try {
        const res = await apiFetch(`${API_PARTY}/get-all`);
        const json = await res.json();
        if (json.success) {
            let options = '<option value="">-- Select Party --</option>';
            json.data.forEach(p => options += `<option value="${p.idPartyAccount}">${escapeHtml(p.partyName)}</option>`);
            DOM.party().innerHTML = options;
        }
    } catch (err) { console.error(err); }
}

function calculateBalance() {
    const out = parseFloat(DOM.outstanding().value) || 0;
    const rec = parseFloat(DOM.received().value) || 0;
    const tds = parseFloat(DOM.tds().value) || 0;
    DOM.balance().value = (out - (rec + tds)).toFixed(2);
}

async function bindTable() {
    const res = await apiFetch(`${API}/get-all`);
    const json = await res.json();
    if ($.fn.DataTable.isDataTable(DOM.table())) $(DOM.table()).DataTable().destroy();
    DOM.tbody().innerHTML = "";

    json.data.forEach(d => {
        const dateStr = d.paymentDate ? new Date(d.paymentDate).toLocaleDateString('en-GB') : '-';
        const tr = document.createElement("tr");
        tr.innerHTML = `
            <td class="fw-bold">${escapeHtml(d.receiptNo || '-')}</td>
            <td>${dateStr}</td>
            <td>${escapeHtml(d.partyName || 'Unknown')}</td>
            <td>${escapeHtml(d.idPaymentMode || '-')}</td>
            <td class="text-end fw-bold text-success">₹ ${parseFloat(d.amountReceived || 0).toFixed(2)}</td>
            <td class="text-center">
                <div class="d-flex">
                    <a href="javascript:void(0);" onclick="editEntry(${d.idPayment})" class="btn btn-primary btn-xs sharp me-1"><i class="fa fa-pencil"></i></a>
                    <a href="javascript:void(0);" onclick="deleteEntry(${d.idPayment})" class="btn btn-danger btn-xs sharp"><i class="fa fa-trash"></i></a>
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

async function editEntry(id) {
    const res = await apiFetch(`${API}/get-by-id/${id}`);
    const json = await res.json();
    if (json.success) {
        const d = json.data;
        DOM.id().value = d.idPayment;
        DOM.receiptNo().value = d.receiptNo || "";
        if (d.paymentDate) DOM.date().value = d.paymentDate.split('T')[0];
        DOM.type().value = d.idPaymentType || "1";
        DOM.party().value = d.idParty || "";
        DOM.billNo().value = d.idBill || "";
        DOM.billAmt().value = parseFloat(d.billAmount || 0).toFixed(2);
        DOM.outstanding().value = parseFloat(d.outstandingAmount || 0).toFixed(2);
        DOM.mode().value = d.idPaymentMode || "NEFT/RTGS";
        DOM.received().value = parseFloat(d.amountReceived || 0).toFixed(2);
        DOM.tds().value = parseFloat(d.tdsAmt || 0).toFixed(2);
        DOM.balance().value = parseFloat(d.balanceAmt || 0).toFixed(2);
        DOM.remarks().value = d.remarks || "";
        entryModal.show();
    }
}

async function deleteEntry(id) {
    if (!await confirmDelete("Delete this Payment?")) return;
    const res = await apiFetch(`${API}/delete/${id}`, { method: "DELETE" });
    const json = await res.json();
    if (json.success) { showToast("success", json.message, "Payment Receive"); bindTable(); }
}

async function saveData() {
    if (!DOM.party().value || !DOM.date().value) {
        return showToast("danger", "Please fill required fields (Date, Party)", "Error");
    }

    const dto = {
        idPayment: Number(DOM.id().value || 0),
        receiptNo: DOM.receiptNo().value,
        paymentDate: DOM.date().value,
        idPaymentType: Number(DOM.type().value),
        idParty: Number(DOM.party().value),
        idBill: Number(DOM.billNo().value || 0),
        billAmount: parseFloat(DOM.billAmt().value) || 0,
        outstandingAmount: parseFloat(DOM.outstanding().value) || 0,
        idPaymentMode: DOM.mode().value,
        amountReceived: parseFloat(DOM.received().value) || 0,
        tdsAmt: parseFloat(DOM.tds().value) || 0,
        balanceAmt: parseFloat(DOM.balance().value) || 0,
        remarks: DOM.remarks().value
    };

    DOM.save().disabled = true;
    const res = await apiFetch(`${API}/save`, { method: "POST", headers: { "Content-Type": "application/json" }, body: JSON.stringify(dto) });
    const json = await res.json();

    if (json.success) { showToast("success", json.message, "Payment Receive"); entryModal.hide(); clearForm(); bindTable(); }
    DOM.save().disabled = false;
}

function clearForm() {
    DOM.id().value = 0; DOM.receiptNo().value = ""; DOM.billAmt().value = "0.00"; DOM.outstanding().value = "0.00";
    DOM.received().value = "0.00"; DOM.tds().value = "0.00"; DOM.balance().value = "0.00"; DOM.remarks().value = "";
    DOM.date().value = new Date().toISOString().split('T')[0];
    DOM.party().value = "";
}