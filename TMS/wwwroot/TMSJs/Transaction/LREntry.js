// ======================================================
// CONFIG & STATE
// ======================================================
const API = "/api/transaction/lr";
const API_PARTY = "/api/master/party-account";
const API_TRUCK = "/api/master/truck";
const API_STATE = "/api/master/state";
const API_CITY = "/api/master/city";

let gridItems = []; // Holds the rows for the grid
let partyData = []; // Cache to quickly look up addresses

// DOM Cache
const DOM = {
    // Header
    id: () => document.getElementById("hdnId"),
    lrNo: () => document.getElementById("txtLRNo"),
    lrDate: () => document.getElementById("txtLRDate"),
    consignor: () => document.getElementById("ddlConsignor"),
    consignorAdd: () => document.getElementById("txtConsignorAddress"),
    fromState: () => document.getElementById("ddlFromState"),
    fromCity: () => document.getElementById("ddlFromCity"),
    invoiceNo: () => document.getElementById("txtInvoiceNo"),

    truck: () => document.getElementById("ddlTruck"),
    consignee: () => document.getElementById("ddlConsignee"),
    consigneeAdd: () => document.getElementById("txtConsigneeAddress"),
    toState: () => document.getElementById("ddlToState"),
    toCity: () => document.getElementById("ddlToCity"),
    invoiceDate: () => document.getElementById("txtInvoiceDate"),
    gstPaidBy: () => document.getElementById("ddlGSTPaidBy"),
    selectTo: () => document.getElementById("ddlSelectTo"),
    eWay: () => document.getElementById("txtEWayBillNo"),

    chkBillTo: () => document.getElementById("chkChangeBillTo"),
    billTo: () => document.getElementById("ddlBillTo"),

    // Footer
    declared: () => document.getElementById("txtDeclaredValue"),
    remarks: () => document.getElementById("txtRemarks"),
    freight: () => document.getElementById("txtFreight"),
    hamali: () => document.getElementById("txtHamali"),
    otherCharge: () => document.getElementById("txtOtherCharge"),
    total: () => document.getElementById("txtTotalAmt"),

    // Grid Inputs (TFOOT)
    inPack: () => document.getElementById("inPacking"),
    inDesc: () => document.getElementById("inDesc"),
    inPkgs: () => document.getElementById("inPackages"),
    inFreightOn: () => document.getElementById("inFreightOn"),
    inWt: () => document.getElementById("inWeight"),
    inRate: () => document.getElementById("inRate"),
    inAmt: () => document.getElementById("inAmount"),

    gridBody: () => document.getElementById("gridBody"),
    btnAddItem: () => document.getElementById("btnAddItem"),
    btnSave: () => document.getElementById("btnSave")
};

// ======================================================
// INIT
// ======================================================
document.addEventListener("DOMContentLoaded", async () => {
    // Set default dates
    const today = new Date().toISOString().split('T')[0];
    DOM.lrDate().value = today;
    DOM.invoiceDate().value = today;

    await loadDropdowns();

    // Fetch the next Sr No ONLY if it's a new entry (ID is 0)
    if (Number(DOM.id().value) === 0) {
        await fetchLRNo();
    }

    // Event Listeners
    DOM.chkBillTo().addEventListener("change", (e) => {
        const isChecked = e.target.checked;

        DOM.billTo().disabled = !isChecked;
        // you MUST refresh the selectpicker to see the visual change.
        $(DOM.billTo()).selectpicker('refresh');
    });

    // Auto-fill Party Address & State
    DOM.consignor().addEventListener("change", (e) => fillPartyDetails(e.target.value, 'consignor'));
    DOM.consignee().addEventListener("change", (e) => fillPartyDetails(e.target.value, 'consignee'));

    // Grid row math
    document.querySelectorAll('.calc-source').forEach(el => el.addEventListener('input', calculateRowAmt));
    DOM.btnAddItem().addEventListener('click', addGridItem);

    // Footer math
    document.querySelectorAll('.calc-total').forEach(el => el.addEventListener('input', calculateGrandTotal));

    DOM.btnSave().addEventListener('click', saveTransaction);

    // If Editing (check URL for ?id=123)
    const urlParams = new URLSearchParams(window.location.search);
    const editId = urlParams.get('id');
    if (editId) {
        await loadLRForEdit(editId);
    }
});

// FETCH LR No
// ======================================================
async function fetchLRNo() {
    try {
        // Adjust this endpoint to match your actual backend API route
        const res = await apiFetch(`${API}/get-lr-no`);
        const json = await res.json();

        if (json.success) {
            // Assuming your API returns the number in json.data
            DOM.lrNo().value = json.data;
        } else {
            showToast("warning", "Could not generate LR No", "LR");
        }
    } catch (err) {
        console.error("Error fetching Bill No:", err);
    }
}

// ======================================================
// LOAD DROPDOWNS
// ======================================================
async function loadDropdowns() {
    try {
        const [partyRes, truckRes, stateRes, cityRes] = await Promise.all([
            apiFetch(`${API_PARTY}/get-all`).then(r => r.json()),
            apiFetch(`${API_TRUCK}/get-all`).then(r => r.json()),
            apiFetch(`${API_STATE}/get-all`).then(r => r.json()),
            apiFetch(`${API_CITY}/get-all`).then(r => r.json())
        ]);

        if (partyRes.success) partyData = partyRes.data; // Cache for address lookup

        populateDropdown(DOM.consignor(), partyRes, "idPartyAccount", "partyName", "-- Select Consignor --");
        populateDropdown(DOM.consignee(), partyRes, "idPartyAccount", "partyName", "-- Select Consignee --");
        populateDropdown(DOM.billTo(), partyRes, "idPartyAccount", "partyName", "-- Select Bill To --");
        populateDropdown(DOM.truck(), truckRes, "idTruck", "truckNumber", "-- Select Truck --");

        populateDropdown(DOM.fromState(), stateRes, "idState", "state", "");
        populateDropdown(DOM.toState(), stateRes, "idState", "state", "");
        populateDropdown(DOM.fromCity(), cityRes, "idCity", "city", "");
        populateDropdown(DOM.toCity(), cityRes, "idCity", "city", "");

        $('.form-select, select').selectpicker('refresh');

    } catch (err) {
        console.error("Dropdown load error", err);
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
// AUTO-FILL LOGIC
// ======================================================
function fillPartyDetails(partyId, type) {
    if (!partyId) return;
    const party = partyData.find(p => p.idPartyAccount == partyId);
    if (!party) return;

    if (type === 'consignor') {
        DOM.consignorAdd().value = party.address || '';
        DOM.fromState().value = party.idState || '';
        DOM.fromCity().value = party.idCity || '';
    } else {
        DOM.consigneeAdd().value = party.address || '';
        DOM.toState().value = party.idState || '';
        DOM.toCity().value = party.idCity || '';
    }
    $('#ddlFromState').selectpicker('refresh');
    $('#ddlFromCity').selectpicker('refresh');
    $('#ddlToState').selectpicker('refresh');
    $('#ddlToCity').selectpicker('refresh');
}

// ======================================================
// GRID LOGIC
// ======================================================
function calculateRowAmt() {
    const wt = parseFloat(DOM.inWt().value) || 0;
    const rt = parseFloat(DOM.inRate().value) || 0;
    DOM.inAmt().value = (wt * rt).toFixed(2);
}

function addGridItem() {
    const desc = DOM.inDesc().value.trim();
    if (!desc) {
        showToast("warning", "Description is required", "LR Details");
        return;
    }

    const item = {
        idTemp: Date.now(), // Local tracking ID
        methodOfPacking: DOM.inPack().value.trim(),
        description: desc,
        packages: DOM.inPkgs().value.trim(),
        freightOn: DOM.inFreightOn().value,
        weight: parseFloat(DOM.inWt().value) || 0,
        rate: parseFloat(DOM.inRate().value) || 0,
        amount: parseFloat(DOM.inAmt().value) || 0
    };

    gridItems.push(item);
    renderGrid();

    // Clear inputs
    DOM.inPack().value = "";
    DOM.inDesc().value = "";
    DOM.inPkgs().value = "";
    DOM.inWt().value = "0.00";
    DOM.inRate().value = "0.00";
    DOM.inAmt().value = "0.00";
}

function removeGridItem(idTemp) {
    gridItems = gridItems.filter(i => i.idTemp !== idTemp);
    renderGrid();
}

function renderGrid() {
    DOM.gridBody().innerHTML = "";
    let totalFreight = 0;

    gridItems.forEach(item => {
        totalFreight += item.amount;
        const tr = document.createElement("tr");
        tr.innerHTML = `
            <td>${escapeHtml(item.methodOfPacking)}</td>
            <td>${escapeHtml(item.description)}</td>
            <td>${escapeHtml(item.packages)}</td>
            <td>${escapeHtml(item.freightOn)}</td>
            <td class="text-end">${item.weight.toFixed(2)}</td>
            <td class="text-end">${item.rate.toFixed(2)}</td>
            <td class="text-end">${item.amount.toFixed(2)}</td>
            <td class="text-center">
                <button type="button" class="btn btn-danger btn-xs" onclick="removeGridItem(${item.idTemp})"><i class="fa fa-trash"></i></button>
            </td>
        `;
        DOM.gridBody().appendChild(tr);
    });

    // Update Footer Freight automatically based on grid
    DOM.freight().value = totalFreight.toFixed(2);
    calculateGrandTotal();
}

// ======================================================
// FOOTER TOTAL LOGIC
// ======================================================
function calculateGrandTotal() {
    const fr = parseFloat(DOM.freight().value) || 0;
    const ham = parseFloat(DOM.hamali().value) || 0;
    const oth = parseFloat(DOM.otherCharge().value) || 0;

    DOM.total().value = (fr + ham + oth).toFixed(2);
}

// ======================================================
// SAVE TRANSACTION (Master-Detail DTO)
// ======================================================
async function saveTransaction(e) {
    e.preventDefault();

    // 1. Validation
    if (!DOM.consignor().value || !DOM.consignee().value || !DOM.lrDate().value) {
        showToast("danger", "Please fill all required (*) fields.", "LR Entry");
        return;
    }
    if (gridItems.length === 0) {
        showToast("warning", "Please add at least one item to the LR details.", "LR Entry");
        return;
    }

    // 2. Build DTO exactly matching C# TransactionLRDto
    const dto = {
        Header: {
            idLR: Number(DOM.id().value || 0),
            lrDate: DOM.lrDate().value,
            idConsignor: Number(DOM.consignor().value),
            consignorAddress: DOM.consignorAdd().value,
            idFromState: Number(DOM.fromState().value || 0) || null,
            idFromCity: Number(DOM.fromCity().value || 0) || null,
            invoiceNo: DOM.invoiceNo().value,

            idTruck: Number(DOM.truck().value || 0) || null,
            idConsignee: Number(DOM.consignee().value),
            consigneeAddress: DOM.consigneeAdd().value,
            idToState: Number(DOM.toState().value || 0) || null,
            idToCity: Number(DOM.toCity().value || 0) || null,
            invoiceDate: DOM.invoiceDate().value || null,

            selectTo: DOM.selectTo().value,
            eWayBillNo: DOM.eWay().value,
            gstPaidBy: DOM.gstPaidBy().value,
            changeBillToParty: DOM.chkBillTo().checked,
            billedTo: DOM.chkBillTo().checked ? DOM.billTo().value : null, // Assuming BilledTo holds Party Name or ID as a string depending on your SQL schema choice. I kept it as a dropdown for better data integrity.

            declaredValue: DOM.declared().value,
            remarks: DOM.remarks().value,
            freightAmt: parseFloat(DOM.freight().value) || 0,
            hamaliAmt: parseFloat(DOM.hamali().value) || 0,
            otherChargeAmt: parseFloat(DOM.otherCharge().value) || 0,
            totalAmt: parseFloat(DOM.total().value) || 0
        },
        Details: gridItems.map(i => ({
            methodOfPacking: i.methodOfPacking,
            description: i.description,
            packages: i.packages,
            freightOn: i.freightOn,
            weight: i.weight,
            rate: i.rate,
            amount: i.amount
        }))
    };

    DOM.btnSave().disabled = true;
    DOM.btnSave().innerHTML = '<i class="fa fa-spinner fa-spin"></i> Saving...';

    try {
        const res = await apiFetch(`${API}/save`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(dto)
        });

        const json = await res.json();
        if (!json.success) throw new Error(json.message);

        showToast("success", json.message, "LR Entry");
        setTimeout(() => window.location.href = '/Transaction/LRList', 1500); // Redirect to list on success

    } catch (err) {
        showToast("danger", err.message, "LR Entry");
    } finally {
        DOM.btnSave().disabled = false;
        DOM.btnSave().innerHTML = '<i class="fa fa-save"></i> Save / Update';
    }
}

// ======================================================
// LOAD FOR EDIT
// ======================================================
async function loadLRForEdit(id) {
    try {
        const res = await apiFetch(`${API}/get-by-id/${id}`);
        const json = await res.json();
        if (!json.success) throw new Error("Could not load LR");

        const data = json.data;
        const h = data.header;

        

        // Bind Header
        DOM.id().value = h.idlr;
        DOM.lrNo().value = h.lrNo || "";
        if (h.lrDate) DOM.lrDate().value = h.lrDate.split('T')[0];

        DOM.consignor().value = h.idConsignor || "";
        DOM.consignorAdd().value = h.consignorAddress || "";
        DOM.fromState().value = h.idFromState || "";
        DOM.fromCity().value = h.idFromCity || "";
        DOM.invoiceNo().value = h.invoiceNo || "";

        DOM.truck().value = h.idTruck || "";
        DOM.consignee().value = h.idConsignee || "";
        DOM.consigneeAdd().value = h.consigneeAddress || "";
        DOM.toState().value = h.idToState || "";
        DOM.toCity().value = h.idToCity || "";
        if (h.invoiceDate) DOM.invoiceDate().value = h.invoiceDate.split('T')[0];

        DOM.gstPaidBy().value = h.gstPaidBy || "";
        DOM.selectTo().value = h.selectTo || "";
        DOM.eWay().value = h.eWayBillNo || "";

        DOM.chkBillTo().checked = h.changeBillToParty;
        DOM.billTo().disabled = !h.changeBillToParty;
        DOM.billTo().value = h.billedTo || ""; // Assumes billedTo stored the party ID

        // Bind Footer
        DOM.declared().value = h.declaredValue || "";
        DOM.remarks().value = h.remarks || "";
        DOM.freight().value = h.freightAmt || "0.00";
        DOM.hamali().value = h.hamaliAmt || "0.00";
        DOM.otherCharge().value = h.otherChargeAmt || "0.00";
        DOM.total().value = h.totalAmt || "0.00";

        // Bind Grid
        gridItems = data.details.map(d => ({
            idTemp: Date.now() + Math.random(),
            methodOfPacking: d.methodOfPacking,
            description: d.description,
            packages: d.packages,
            freightOn: d.freightOn,
            weight: d.weight,
            rate: d.rate,
            amount: d.amount
        }));

        renderGrid();
        // refresh dropdown
        $('.form-select, select').selectpicker('refresh');

    } catch (err) {
        showToast("danger", err.message, "LR Edit");
    }
}