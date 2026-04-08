// ======================================================
// CONFIGURATION & GLOBAL STATE
// ======================================================
const API = "/api/transaction/challan";
let gridItems = [];
let cachedLRData = [];

// ======================================================
// DOM ELEMENTS CACHE
// ======================================================
const DOM = {
    // Hidden Field
    id: () => document.getElementById("hdnId"),

    // Header Fields
    voucher: () => document.getElementById("txtVoucher"),
    date: () => document.getElementById("txtDate"),
    consignee: () => document.getElementById("ddlConsignee"),
    panHolder: () => document.getElementById("txtPanHolder"),
    panNo: () => document.getElementById("txtPanNo"),
    bill: () => document.getElementById("ddlBill"),
    truck: () => document.getElementById("ddlTruck"),
    driver: () => document.getElementById("txtDriver"),
    transporter: () => document.getElementById("ddlTransporter"),
    adv1: () => document.getElementById("txtAdv1"),
    adv2: () => document.getElementById("txtAdv2"),

    // Grid Inputs (Footer Row)
    inLR: () => document.getElementById("inLR"),
    inLRDate: () => document.getElementById("inLRDate"),
    inPack: () => document.getElementById("inPack"),
    inDesc: () => document.getElementById("inDesc"),
    inPkgs: () => document.getElementById("inPkgs"),
    inFreightOn: () => document.getElementById("inFreightOn"),
    inFix: () => document.getElementById("inFix"),
    inWt: () => document.getElementById("inWt"),
    inRate: () => document.getElementById("inRate"),
    inAmt: () => document.getElementById("inAmt"),

    // Grid Body & Button
    gridBody: () => document.getElementById("gridBody"),
    btnAddGridRow: () => document.getElementById("btnAddGridRow"),

    // Right Panel: Calculations
    freight: () => document.getElementById("txtFreight"),
    extra: () => document.getElementById("txtExtra"),
    final: () => document.getElementById("txtFinal"),
    cash: () => document.getElementById("txtCash"),
    chq1Amt: () => document.getElementById("txtChq1Amt"),
    chq1No: () => document.getElementById("txtChq1No"),
    chq1Date: () => document.getElementById("txtChq1Date"),
    chq2Amt: () => document.getElementById("txtChq2Amt"),
    chq2No: () => document.getElementById("txtChq2No"),
    chq2Date: () => document.getElementById("txtChq2Date"),
    advance: () => document.getElementById("txtAdvance"),
    chkPaidByParty: () => document.getElementById("chkPaidByParty"),
    balance: () => document.getElementById("txtBalance"),
    paidByParty: () => document.getElementById("txtPaidByParty"),

    // Right Panel: Balance Details
    balChqAmt: () => document.getElementById("txtBalChqAmt"),
    balChqNo: () => document.getElementById("txtBalChqNo"),
    balChqDate: () => document.getElementById("txtBalChqDate"),

    // General
    remarks: () => document.getElementById("txtRemarks"),
    btnSave: () => document.getElementById("btnSave")
};

// ======================================================
// INITIALIZATION
// ======================================================
document.addEventListener("DOMContentLoaded", async function () {

    // 1. Set default date
    const todayString = new Date().toISOString().split('T')[0];
    DOM.date().value = todayString;

    if (Number(DOM.id().value) === 0) {
        await fetchVoucherNo();
    }

    // 2. Load Dropdowns
    await loadDropdownData();

    // 3. Bind Event Listeners
    bindEvents();

    // 4. Check for Edit Mode
    const urlParams = new URLSearchParams(window.location.search);
    const editId = urlParams.get('id');

    if (editId !== null) {
        await loadChallanForEdit(editId);
    }
});

// FETCH Voucer No
// ======================================================
async function fetchVoucherNo() {
    try {
        // Adjust this endpoint to match your actual backend API route
        const res = await apiFetch(`${API}/get-voucher-no`);
        const json = await res.json();

        if (json.success) {
            // Assuming your API returns the number in json.data
            DOM.voucher().value = json.data;
        } else {
            showToast("warning", "Could not generate LR No", "LR");
        }
    } catch (err) {
        console.error("Error fetching Bill No:", err);
    }
}

function bindEvents() {
    // Update LR Date when an LR is selected in the grid
    DOM.inLR().addEventListener("change", function (e) {
        const selectedId = e.target.value;
        const matchingLR = cachedLRData.find(x => x.idlr == selectedId);

        if (matchingLR && matchingLR.lrDate) {
            DOM.inLRDate().value = matchingLR.lrDate.split('T')[0];
        } else {
            DOM.inLRDate().value = '';
        }
    });

    // Auto-calculate the grid row amount when inputs change
    const gridCalcInputs = document.querySelectorAll('.calc-grid');
    gridCalcInputs.forEach(function (element) {
        element.addEventListener('input', calculateGridRowAmount);
    });

    // Add Grid Row button click
    DOM.btnAddGridRow().addEventListener('click', addGridItemToArray);

    // Auto-calculate the Right Panel when inputs change
    const mainCalcInputs = document.querySelectorAll('.calc-main');
    mainCalcInputs.forEach(function (element) {
        element.addEventListener('input', calculateRightPanelTotals);
    });

    // Handle Paid by Party checkbox change
    DOM.chkPaidByParty().addEventListener('change', calculateRightPanelTotals);

    // Save Button
    DOM.btnSave().addEventListener('click', handleSaveTransaction);
}

// ======================================================
// LOAD DATA FUNCTIONS
// ======================================================
async function loadDropdownData() {
    try {
        const partyResponse = await apiFetch(`/api/master/party-account/get-all`);
        const consigneeResponse = await apiFetch(`/api/master/party-account/get-all-consignee`);
        const truckResponse = await apiFetch(`/api/master/truck/get-all`);
        const billResponse = await apiFetch(`/api/transaction/bill/get-all`);
        const lrResponse = await apiFetch(`/api/transaction/lr/get-all`);

        const partyJson = await partyResponse.json();
        const consigneeJson = await consigneeResponse.json();
        const truckJson = await truckResponse.json();
        const billJson = await billResponse.json();
        const lrJson = await lrResponse.json();

        populateDropdownHtml(DOM.consignee(), consigneeJson, "idPartyAccount", "partyName", "-- Select Consignee --");
        populateDropdownHtml(DOM.transporter(), partyJson, "idPartyAccount", "partyName", "-- Select Transporter --");
        populateDropdownHtml(DOM.truck(), truckJson, "idTruck", "truckNumber", "-- Select Truck --");
        populateDropdownHtml(DOM.bill(), billJson, "idBill", "billNo", "-- Select Bill No --");

        if (lrJson.success) {
            cachedLRData = lrJson.data;
            populateDropdownHtml(DOM.inLR(), lrJson, "idlr", "lrNo", "-- Select LR --");
        }

        // CRITICAL FIX: Refresh Select Pickers
        // ==========================================
        $('.form-select, select').selectpicker('refresh');
        //$('#inLR').selectpicker('refresh');
        //$('#ddlConsignee').selectpicker('refresh');
        // If you are using Select2, use: $('select').trigger('change');
    }
    catch (err) {
        console.error("Error loading dropdown data:", err);
    }
}

function populateDropdownHtml(selectElement, jsonResponse, valueProperty, textProperty, defaultText) {
    let optionsHtml = `<option value="">${defaultText}</option>`;

    if (jsonResponse && jsonResponse.success) {
        jsonResponse.data.forEach(function (item) {
            optionsHtml += `<option value="${item[valueProperty]}">${escapeHtml(item[textProperty])}</option>`;
        });
    }

    selectElement.innerHTML = optionsHtml;
}

function refreshLRDropdownOptions() {
    const selectedLRIds = gridItems.map(x => Number(x.idlr));

    let optionsHtml = `<option value="">-- Select LR --</option>`;

    for (let i = 0; i < cachedLRData.length; i++) {
        const item = cachedLRData[i];
        const lrId = Number(item.idlr);

        if (!selectedLRIds.includes(lrId)) {
            optionsHtml += `<option value="${item.idlr}">${escapeHtml(item.lrNo)}</option>`;
        }
    }

    DOM.inLR().innerHTML = optionsHtml;
    DOM.inLR().value = "";

    $('.form-select, select').selectpicker('refresh');
}

// ======================================================
// GRID LOGIC
// ======================================================
function calculateGridRowAmount() {
    const freightType = DOM.inFreightOn().value;
    const fixAmount = parseFloat(DOM.inFix().value) || 0;
    const weight = parseFloat(DOM.inWt().value) || 0;
    const rate = parseFloat(DOM.inRate().value) || 0;

    let calculatedAmount = 0;

    if (freightType === 'Fix') {
        calculatedAmount = fixAmount;
    }
    else {
        calculatedAmount = (weight * rate);
    }

    DOM.inAmt().value = calculatedAmount.toFixed(2);
}

function addGridItemToArray() {
    // Validation
    if (DOM.inLR().value === "") {
        showToast("warning", "Please select an LR Number first.", "Challan Entry");
        return;
    }

    const selectedLRId = Number(DOM.inLR().value);

    // Prevent duplicate LR selection
    const alreadyExists = gridItems.some(x => Number(x.idlr) === selectedLRId);
    if (alreadyExists) {
        showToast("warning", "This LR is already added in details.", "Challan Entry");
        return;
    }

    // Get the display text of the selected dropdown item
    const selectedLRIndex = DOM.inLR().selectedIndex;
    const lrDisplayText = DOM.inLR().options[selectedLRIndex].text;

    // Create the object
    const newItem = {
        idTemp: Date.now(),
        idlr: selectedLRId,
        lrNo: lrDisplayText,
        lrDate: DOM.inLRDate().value,
        methodOfPacking: DOM.inPack().value,
        description: DOM.inDesc().value,
        packages: DOM.inPkgs().value,
        freightOn: DOM.inFreightOn().value,
        fixAmt: parseFloat(DOM.inFix().value) || 0,
        weight: parseFloat(DOM.inWt().value) || 0,
        rate: parseFloat(DOM.inRate().value) || 0,
        amount: parseFloat(DOM.inAmt().value) || 0
    };

    // Add to array and render
    gridItems.push(newItem);
    renderGridTable();

    // Clear input row
    DOM.inLRDate().value = "";
    DOM.inPack().value = "";
    DOM.inDesc().value = "";
    DOM.inPkgs().value = "";
    DOM.inFreightOn().value = "Fix";
    DOM.inFix().value = "0.00";
    DOM.inWt().value = "0.00";
    DOM.inRate().value = "0.00";
    DOM.inAmt().value = "0.00";

    // Rebuild LR dropdown excluding already selected LRs
    refreshLRDropdownOptions();
}

function removeGridItemFromArray(tempIdToRemove) {
    const updatedArray = [];

    for (let i = 0; i < gridItems.length; i++) {
        if (gridItems[i].idTemp !== tempIdToRemove) {
            updatedArray.push(gridItems[i]);
        }
    }

    gridItems = updatedArray;
    renderGridTable();
}

// Ensure the remove function is attached to the global window scope so inline HTML onclick can reach it
window.removeGridItemFromArray = removeGridItemFromArray;

function renderGridTable() {
    let rowsHtml = "";
    let totalFreightForGrid = 0;

    for (let i = 0; i < gridItems.length; i++) {
        const item = gridItems[i];

        totalFreightForGrid += item.amount;

        let displayDate = "-";
        if (item.lrDate) {
            displayDate = new Date(item.lrDate).toLocaleDateString('en-GB');
        }

        rowsHtml += `
            <tr>
                <td class="fw-bold">${escapeHtml(item.lrNo)}</td>
                <td>${displayDate}</td>
                <td>${escapeHtml(item.methodOfPacking)}</td>
                <td>${escapeHtml(item.description)}</td>
                <td>${escapeHtml(item.packages)}</td>
                <td>${escapeHtml(item.freightOn)}</td>
                <td class="text-end">${item.fixAmt.toFixed(2)}</td>
                <td class="text-end">${item.weight.toFixed(2)}</td>
                <td class="text-end">${item.rate.toFixed(2)}</td>
                <td class="text-end fw-bold">${item.amount.toFixed(2)}</td>
                <td class="text-center">
                    <button type="button" class="btn btn-danger btn-xs" onclick="removeGridItemFromArray(${item.idTemp})">
                        <i class="fa fa-trash"></i>
                    </button>
                </td>
            </tr>`;
    }

    DOM.gridBody().innerHTML = rowsHtml;

    // Automatically push the grid total up to the right panel's "Freight" input
    DOM.freight().value = totalFreightForGrid.toFixed(2);

    // Recalculate right panel since Freight changed
    calculateRightPanelTotals();
}

// ======================================================
// RIGHT PANEL CALCULATION LOGIC
// ======================================================
function calculateRightPanelTotals() {
    // 1. Calculate Final Amount
    const freightAmount = parseFloat(DOM.freight().value) || 0;
    const extraAmount = parseFloat(DOM.extra().value) || 0;

    const finalAmount = freightAmount + extraAmount;
    DOM.final().value = finalAmount.toFixed(2);

    // 2. Calculate Advance
    const cashAmount = parseFloat(DOM.cash().value) || 0;
    const cheque1Amount = parseFloat(DOM.chq1Amt().value) || 0;
    const cheque2Amount = parseFloat(DOM.chq2Amt().value) || 0;

    const totalAdvance = cashAmount + cheque1Amount + cheque2Amount;
    DOM.advance().value = totalAdvance.toFixed(2);

    // 3. Calculate Balance and Party Payment
    let currentBalance = finalAmount - totalAdvance;
    let paidByPartyAmount = 0;

    const isPaidByPartyChecked = DOM.chkPaidByParty().checked;

    if (isPaidByPartyChecked) {
        paidByPartyAmount = currentBalance;
        currentBalance = 0;

        DOM.paidByParty().readOnly = false;
        DOM.paidByParty().classList.remove("bg-light");
    }
    else {
        DOM.paidByParty().readOnly = true;
        DOM.paidByParty().classList.add("bg-light");
    }

    DOM.paidByParty().value = paidByPartyAmount.toFixed(2);
    DOM.balance().value = currentBalance.toFixed(2);
}

// ======================================================
// SAVE TRANSACTION
// ======================================================
async function handleSaveTransaction(e) {
    e.preventDefault();

    // 1. Basic Validation
    if (DOM.date().value === "") {
        showToast("danger", "Please select a Challan Date.", "Challan Entry");
        return;
    }

    if (gridItems.length === 0) {
        showToast("warning", "Please add at least one LR to the details grid.", "Challan Entry");
        return;
    }

    // 2. Map Details Array (Ensure exact property names for C# model)
    const detailsDtoArray = [];
    for (let i = 0; i < gridItems.length; i++) {
        const row = gridItems[i];
        detailsDtoArray.push({
            idlr: row.idlr,
            methodOfPacking: row.methodOfPacking,
            description: row.description,
            packages: row.packages,
            freightOn: row.freightOn,
            fixAmt: row.fixAmt,
            weight: row.weight,
            rate: row.rate,
            amount: row.amount
        });
    }

    // 3. Create Final DTO Payload
    const payloadDto = {
        Header: {
            idChallan: Number(DOM.id().value || 0),
            voucherNo: DOM.voucher().value,
            challanDate: DOM.date().value,
            idBill: Number(DOM.bill().value) || null,
            idTruck: Number(DOM.truck().value) || null,
            driverName: DOM.driver().value,
            idTransporter: Number(DOM.transporter().value) || null,
            idConsignee: Number(DOM.consignee().value) || null,
            panCardHolder: DOM.panHolder().value,
            panCardNo: DOM.panNo().value,
            advancePayment1: DOM.adv1().value,
            advancePayment2: DOM.adv2().value,

            freightAmt: parseFloat(DOM.freight().value) || 0,
            extraAmt: parseFloat(DOM.extra().value) || 0,
            finalAmt: parseFloat(DOM.final().value) || 0,
            cashAmt: parseFloat(DOM.cash().value) || 0,
            cheque1Amt: parseFloat(DOM.chq1Amt().value) || 0,
            cheque1No: DOM.chq1No().value,
            cheque1Date: DOM.chq1Date().value || null,
            cheque2Amt: parseFloat(DOM.chq2Amt().value) || 0,
            cheque2No: DOM.chq2No().value,
            cheque2Date: DOM.chq2Date().value || null,
            advanceAmt: parseFloat(DOM.advance().value) || 0,
            isPaymentPaidByParty: DOM.chkPaidByParty().checked,
            balanceAmt: parseFloat(DOM.balance().value) || 0,
            paidByPartyAmt: parseFloat(DOM.paidByParty().value) || 0,

            balanceChequeAmt: parseFloat(DOM.balChqAmt().value) || 0,
            balanceChequeNo: DOM.balChqNo().value,
            balanceChequeDate: DOM.balChqDate().value || null,
            remarks: DOM.remarks().value
        },
        Details: detailsDtoArray
    };

    // 4. Send to Server
    DOM.btnSave().disabled = true;
    DOM.btnSave().innerHTML = '<i class="fa fa-spinner fa-spin"></i> Saving...';

    try {
        const response = await apiFetch(`${API}/save`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(payloadDto)
        });

        const responseJson = await response.json();

        if (responseJson.success) {
            showToast("success", responseJson.message, "Success");

            // Redirect to list page after a short delay
            setTimeout(function () {
                window.location.href = '/Transaction/ChallanList';
            }, 1200);
        }
        else {
            throw new Error(responseJson.message);
        }
    }
    catch (err) {
        showToast("danger", err.message, "Error");
    }
    finally {
        DOM.btnSave().disabled = false;
        DOM.btnSave().innerHTML = '<i class="fa fa-save"></i> Save / Update';
    }
}

// ======================================================
// LOAD FOR EDIT LOGIC
// ======================================================
async function loadChallanForEdit(id) {
    try {
        const response = await apiFetch(`${API}/get-by-id/${id}`);
        const responseJson = await response.json();

        if (!responseJson.success) {
            throw new Error(responseJson.message || "Failed to load Challan data.");
        }

        const headerData = responseJson.data.header;

        // 1. Bind Header Data
        DOM.id().value = headerData.idChallan;
        DOM.voucher().value = headerData.voucherNo || "";

        if (headerData.challanDate) {
            DOM.date().value = headerData.challanDate.split('T')[0];
        }

        DOM.consignee().value = headerData.idConsignee || "";
        DOM.panHolder().value = headerData.panCardHolder || "";
        DOM.panNo().value = headerData.panCardNo || "";
        DOM.bill().value = headerData.idBill || "";
        DOM.truck().value = headerData.idTruck || "";
        DOM.driver().value = headerData.driverName || "";
        DOM.transporter().value = headerData.idTransporter || "";
        DOM.adv1().value = headerData.advancePayment1 || "";
        DOM.adv2().value = headerData.advancePayment2 || "";

        // 2. Bind Right Panel Financial Data
        DOM.freight().value = (headerData.freightAmt || 0).toFixed(2);
        DOM.extra().value = (headerData.extraAmt || 0).toFixed(2);
        DOM.final().value = (headerData.finalAmt || 0).toFixed(2);

        DOM.cash().value = (headerData.cashAmt || 0).toFixed(2);

        DOM.chq1Amt().value = (headerData.cheque1Amt || 0).toFixed(2);
        DOM.chq1No().value = headerData.cheque1No || "";
        if (headerData.cheque1Date) {
            DOM.chq1Date().value = headerData.cheque1Date.split('T')[0];
        }

        DOM.chq2Amt().value = (headerData.cheque2Amt || 0).toFixed(2);
        DOM.chq2No().value = headerData.cheque2No || "";
        if (headerData.cheque2Date) {
            DOM.chq2Date().value = headerData.cheque2Date.split('T')[0];
        }

        DOM.advance().value = (headerData.advanceAmt || 0).toFixed(2);

        DOM.chkPaidByParty().checked = headerData.isPaymentPaidByParty;

        DOM.balance().value = (headerData.balanceAmt || 0).toFixed(2);
        DOM.paidByParty().value = (headerData.paidByPartyAmt || 0).toFixed(2);

        DOM.balChqAmt().value = (headerData.balanceChequeAmt || 0).toFixed(2);
        DOM.balChqNo().value = headerData.balanceChequeNo || "";
        if (headerData.balanceChequeDate) {
            DOM.balChqDate().value = headerData.balanceChequeDate.split('T')[0];
        }

        DOM.remarks().value = headerData.remarks || "";

        // 3. Rebuild Grid Array
        gridItems = [];
        const detailsArray = responseJson.data.details;

        for (let i = 0; i < detailsArray.length; i++) {
            const dbDetail = detailsArray[i];

            gridItems.push({
                idTemp: Date.now() + i, // Generate unique local UI ID
                idlr: dbDetail.idlr,
                lrNo: dbDetail.lrNo,
                lrDate: dbDetail.lrDate,
                methodOfPacking: dbDetail.methodOfPacking,
                description: dbDetail.description,
                packages: dbDetail.packages,
                freightOn: dbDetail.freightOn,
                fixAmt: dbDetail.fixAmt,
                weight: dbDetail.weight,
                rate: dbDetail.rate,
                amount: dbDetail.amount
            });
        }

        // Render the array to the screen
        renderGridTable();

        // refresh dropdown
        $('.form-select, select').selectpicker('refresh');

    } catch (err) {
        showToast("danger", err.message, "Challan Edit Error");
    }
}