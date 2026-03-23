// ======================================================
// CONFIG & STATE
// ======================================================
const API = "/api/transaction/bill";
let gridItems = [];
let lrData = [];
let cityData = [];

// ======================================================
// DOM CACHE
// ======================================================
const DOM = {
    // Header
    id: () => document.getElementById("hdnId"),
    billNo: () => document.getElementById("txtBillNo"),
    date: () => document.getElementById("txtBillDate"),
    party: () => document.getElementById("ddlParty"),
    truck: () => document.getElementById("ddlTruck"),
    mobile: () => document.getElementById("txtMobile"),

    // Footer
    final: () => document.getElementById("txtFinal"),
    toPay: () => document.getElementById("txtToPay"),
    comm: () => document.getElementById("txtCommission"),
    remarks: () => document.getElementById("txtRemarks"),

    // Grid Inputs (TFOOT)
    inLR: () => document.getElementById("inLR"),
    inLRDate: () => document.getElementById("inLRDate"),
    inDesc: () => document.getElementById("inDesc"),
    inFrom: () => document.getElementById("inFrom"),
    inTo: () => document.getElementById("inTo"),
    inFreightOn: () => document.getElementById("inFreightOn"),
    inFix: () => document.getElementById("inFix"),
    inWt: () => document.getElementById("inWeight"),
    inRate: () => document.getElementById("inRate"),
    inExtra: () => document.getElementById("inExtra"),
    inAmt: () => document.getElementById("inAmount"),

    // UI Elements
    gridBody: () => document.getElementById("gridBody"),
    btnAdd: () => document.getElementById("btnAddItem"),
    btnSave: () => document.getElementById("btnSave")
};

// ======================================================
// INIT
// ======================================================
document.addEventListener("DOMContentLoaded", async () => {

    // Set default date to today
    const today = new Date().toISOString().split('T')[0];
    DOM.date().value = today;

    // Load master data for dropdowns
    await loadDropdowns();

    // Auto-fill LR Details when an LR is selected
    DOM.inLR().addEventListener("change", handleLRSelection);

    // Bind calculation events
    document.querySelectorAll('.calc-trigger').forEach(el => {
        el.addEventListener('input', calculateRowAmt);
    });

    // Button Events
    DOM.btnAdd().addEventListener('click', addGridItem);
    DOM.btnSave().addEventListener('click', saveTransaction);

    // Check if editing
    const urlParams = new URLSearchParams(window.location.search);
    const editId = urlParams.get('id');

    if (editId) {
        await loadForEdit(editId);
    }
});

// ======================================================
// LOAD DROPDOWNS
// ======================================================
async function loadDropdowns() {
    try {
        const [partyRes, truckRes, lrRes, cityRes] = await Promise.all([
            apiFetch(`/api/master/party-account/get-all`).then(r => r.json()),
            apiFetch(`/api/master/truck/get-all`).then(r => r.json()),
            apiFetch(`/api/transaction/lr/get-all`).then(r => r.json()),
            apiFetch(`/api/master/city/get-all`).then(r => r.json())
        ]);

        populateDropdown(DOM.party(), partyRes, "idPartyAccount", "partyName", "-- Select Bill To Party --");
        populateDropdown(DOM.truck(), truckRes, "idTruck", "truckNumber", "-- Select Truck --");

        // Cache LR data so we can auto-fill details later
        if (lrRes.success) {
            lrData = lrRes.data;
            populateDropdown(DOM.inLR(), lrRes, "idLR", "lrNo", "-- Select LR --");
        }

        // Cache City data
        if (cityRes.success) {
            cityData = cityRes.data;
            let cityOptions = '<option value="">-- Select City --</option>';
            cityRes.data.forEach(c => {
                cityOptions += `<option value="${c.idCity}">${escapeHtml(c.city)}</option>`;
            });
            DOM.inFrom().innerHTML = cityOptions;
            DOM.inTo().innerHTML = cityOptions;
        }

    } catch (err) {
        console.error("Failed to load dropdown data:", err);
        showToast("warning", "Could not load all master dropdowns.", "Bill Entry");
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
// EVENT HANDLERS & CALCULATIONS
// ======================================================
function handleLRSelection(e) {
    const selectedLRId = e.target.value;
    const lr = lrData.find(x => x.idLR == selectedLRId);

    if (lr) {
        DOM.inLRDate().value = lr.lrDate ? lr.lrDate.split('T')[0] : '';
        DOM.inFrom().value = lr.idFromCity || '';
        DOM.inTo().value = lr.idToCity || '';
    } else {
        DOM.inLRDate().value = '';
        DOM.inFrom().value = '';
        DOM.inTo().value = '';
    }
}

function calculateRowAmt() {
    const freightType = DOM.inFreightOn().value;
    const fixAmount = parseFloat(DOM.inFix().value) || 0;
    const weight = parseFloat(DOM.inWt().value) || 0;
    const rate = parseFloat(DOM.inRate().value) || 0;
    const extraCharges = parseFloat(DOM.inExtra().value) || 0;

    let calculatedAmount = 0;

    if (freightType === 'Fix') {
        calculatedAmount = fixAmount + extraCharges;
    } else {
        calculatedAmount = (weight * rate) + extraCharges;
    }

    DOM.inAmt().value = calculatedAmount.toFixed(2);
}

// ======================================================
// GRID ACTIONS
// ======================================================
function addGridItem() {
    // 1. Validation
    if (!DOM.inLR().value) {
        showToast("warning", "Please select an LR Number.", "Bill Entry");
        return;
    }

    // 2. Fetch Display Text for UI
    const lrText = DOM.inLR().options[DOM.inLR().selectedIndex].text;
    const fromText = DOM.inFrom().value ? DOM.inFrom().options[DOM.inFrom().selectedIndex].text : '';
    const toText = DOM.inTo().value ? DOM.inTo().options[DOM.inTo().selectedIndex].text : '';

    // 3. Create Grid Object
    const item = {
        idTemp: Date.now(), // Local identifier for deletion
        idLR: Number(DOM.inLR().value),
        lrNo: lrText,
        lrDate: DOM.inLRDate().value,
        description: DOM.inDesc().value.trim(),
        idFromCity: Number(DOM.inFrom().value) || null,
        fromCityName: fromText,
        idToCity: Number(DOM.inTo().value) || null,
        toCityName: toText,
        freightOn: DOM.inFreightOn().value,
        fixAmt: parseFloat(DOM.inFix().value) || 0,
        weight: parseFloat(DOM.inWt().value) || 0,
        rate: parseFloat(DOM.inRate().value) || 0,
        extraCharges: parseFloat(DOM.inExtra().value) || 0,
        amount: parseFloat(DOM.inAmt().value) || 0
    };

    gridItems.push(item);
    renderGrid();

    // 4. Clear Footer Inputs
    DOM.inLR().value = "";
    DOM.inLRDate().value = "";
    DOM.inDesc().value = "";
    DOM.inFrom().value = "";
    DOM.inTo().value = "";
    DOM.inFreightOn().value = "Fix";
    DOM.inFix().value = "0.00";
    DOM.inWt().value = "0.00";
    DOM.inRate().value = "0.00";
    DOM.inExtra().value = "0.00";
    DOM.inAmt().value = "0.00";
}

function removeGridItem(idTemp) {
    gridItems = gridItems.filter(i => i.idTemp !== idTemp);
    renderGrid();
}

function renderGrid() {
    DOM.gridBody().innerHTML = "";
    let finalAmount = 0;

    gridItems.forEach(item => {
        finalAmount += item.amount;

        const dateDisplay = item.lrDate ? new Date(item.lrDate).toLocaleDateString('en-GB') : '-';

        const tr = document.createElement("tr");
        tr.innerHTML = `
            <td class="fw-bold">${escapeHtml(item.lrNo)}</td>
            <td>${dateDisplay}</td>
            <td>${escapeHtml(item.description)}</td>
            <td>${escapeHtml(item.fromCityName)}</td>
            <td>${escapeHtml(item.toCityName)}</td>
            <td>${escapeHtml(item.freightOn)}</td>
            <td class="text-end">${item.fixAmt.toFixed(2)}</td>
            <td class="text-end">${item.weight.toFixed(2)}</td>
            <td class="text-end">${item.rate.toFixed(2)}</td>
            <td class="text-end">${item.extraCharges.toFixed(2)}</td>
            <td class="text-end fw-bold text-success">${item.amount.toFixed(2)}</td>
            <td class="text-center">
                <button type="button" class="btn btn-danger btn-xs" onclick="removeGridItem(${item.idTemp})">
                    <i class="fa fa-trash"></i>
                </button>
            </td>
        `;
        DOM.gridBody().appendChild(tr);
    });

    // Update Footer Totals
    DOM.final().value = finalAmount.toFixed(2);
}

// ======================================================
// SAVE DATA
// ======================================================
async function saveTransaction(e) {
    e.preventDefault();

    // 1. Validate Form
    if (!DOM.party().value || !DOM.date().value) {
        showToast("danger", "Please fill required (*) fields.", "Bill Entry");
        return;
    }
    if (gridItems.length === 0) {
        showToast("warning", "Please add at least one LR to the details grid.", "Bill Entry");
        return;
    }

    // 2. Map DTO
    const dto = {
        Header: {
            idBill: Number(DOM.id().value || 0),
            billDate: DOM.date().value,
            idBillToParty: Number(DOM.party().value),
            idTruck: Number(DOM.truck().value || 0) || null,
            driverMobileNo: DOM.mobile().value.trim(),
            finalAmount: parseFloat(DOM.final().value) || 0,
            toPayAmount: parseFloat(DOM.toPay().value) || 0,
            commissionAmt: parseFloat(DOM.comm().value) || 0,
            remarks: DOM.remarks().value.trim()
        },
        Details: gridItems.map(i => ({
            idLR: i.idLR,
            description: i.description,
            idFromCity: i.idFromCity,
            idToCity: i.idToCity,
            freightOn: i.freightOn,
            fixAmt: i.fixAmt,
            weight: i.weight,
            rate: i.rate,
            extraCharges: i.extraCharges,
            amount: i.amount
        }))
    };

    // 3. Save
    DOM.btnSave().disabled = true;
    DOM.btnSave().innerHTML = '<i class="fa fa-spinner fa-spin"></i> Saving...';

    try {
        const res = await apiFetch(`${API}/save`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(dto)
        });

        const json = await res.json();

        if (!json.success) {
            throw new Error(json.message);
        }

        showToast("success", json.message, "Bill Entry");

        // Redirect to list page after brief delay
        setTimeout(() => window.location.href = '/Transaction/BillList', 1200);

    } catch (err) {
        showToast("danger", err.message, "Bill Entry Error");
    } finally {
        DOM.btnSave().disabled = false;
        DOM.btnSave().innerHTML = '<i class="fa fa-save"></i> Save / Update';
    }
}

// ======================================================
// LOAD FOR EDIT
// ======================================================
async function loadForEdit(id) {
    try {
        const res = await apiFetch(`${API}/get-by-id/${id}`);
        const json = await res.json();

        if (!json.success) {
            throw new Error(json.message || "Failed to load Bill");
        }

        const h = json.data.header;

        // Bind Header
        DOM.id().value = h.idBill;
        DOM.billNo().value = h.billNo || "";

        if (h.billDate) {
            DOM.date().value = h.billDate.split('T')[0];
        }

        DOM.party().value = h.idBillToParty || "";
        DOM.truck().value = h.idTruck || "";
        DOM.mobile().value = h.driverMobileNo || "";

        // Bind Footer
        DOM.final().value = parseFloat(h.finalAmount || 0).toFixed(2);
        DOM.toPay().value = parseFloat(h.toPayAmount || 0).toFixed(2);
        DOM.comm().value = parseFloat(h.commissionAmt || 0).toFixed(2);
        DOM.remarks().value = h.remarks || "";

        // Bind Grid Array
        gridItems = json.data.details.map(d => ({
            idTemp: Date.now() + Math.random(), // Unique local ID
            idLR: d.idLR,
            lrNo: d.lrNo,
            lrDate: d.lrDate,
            description: d.description,
            idFromCity: d.idFromCity,
            fromCityName: d.fromCityName,
            idToCity: d.idToCity,
            toCityName: d.toCityName,
            freightOn: d.freightOn,
            fixAmt: d.fixAmt,
            weight: d.weight,
            rate: d.rate,
            extraCharges: d.extraCharges,
            amount: d.amount
        }));

        renderGrid();

    } catch (err) {
        showToast("danger", err.message, "Bill Edit Error");
    }
}